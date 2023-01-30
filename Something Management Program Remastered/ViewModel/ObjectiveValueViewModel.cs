using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Something_Management_Program_Remastered.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Threading;

namespace Something_Management_Program_Remastered.ViewModel
{
    public partial class ObjectiveValueViewModel : ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        private ObservableCollection<ObjectiveValue> objectiveValueCollection;

        [ObservableProperty]
        private ObjectiveValue selectedObjectiveValue;

        [ObservableProperty]
        private Modifier selectedModifier;

        [ObservableProperty]
        private ObservableCollection<Modifier> selectedModifierTimeLine = new ObservableCollection<Modifier>();

        [RelayCommand]
        private void NewObjectiveValue()
        {
            ObjectiveValueCollection.Add(item: new ObjectiveValue());
            WriteJsonObjectiveValueCollection(ObjectiveValueCollection);
        }

        [RelayCommand]
        private void DeleteObjectiveValue()
        {
            if (SelectedObjectiveValue is not null && ObjectiveValueCollection.Count > 0)
            {
                ObjectiveValueCollection.Remove(SelectedObjectiveValue);
                WriteJsonObjectiveValueCollection(ObjectiveValueCollection);
            }

        }

        [RelayCommand]
        private void NewModifier()
        {
            if (SelectedObjectiveValue is not null)
            {
                SelectedObjectiveValue.Modifiers.Add(item: new Modifier());
                WriteJsonObjectiveValueCollection(ObjectiveValueCollection);
            }
        }

        [RelayCommand]
        private void DeleteModifier()
        {
            if (SelectedObjectiveValue is not null && SelectedObjectiveValue.Modifiers.Count >= 1)
            {
                SelectedObjectiveValue.Modifiers.Remove(item: SelectedModifier);
                WriteJsonObjectiveValueCollection(ObjectiveValueCollection);
            }
        }

        [RelayCommand]
        private void SkipTime()
        {
            SelectedObjectiveValue.CurrentTime = DateTime.Now;
            WriteJsonObjectiveValueCollection(ObjectiveValueCollection);
            if (SelectedObjectiveValue is not null)
            {
                foreach (Modifier mod in SelectedObjectiveValue.Modifiers)
                {
                    if (mod.Interval == TimeSpan.Zero || SelectedObjectiveValue.SetTime < SelectedObjectiveValue.CurrentTime) { continue; }
                    double amount = (SelectedObjectiveValue.SetTime - SelectedObjectiveValue.CurrentTime).Ticks / (double)mod.Interval.Ticks;
                    int amount_rounded = (int)Math.Round(amount);
                    if (amount_rounded == 0) { continue; }
                    switch (mod.ModType)
                    {
                        case modTypeEnum.Add:
                            SelectedObjectiveValue.Amount += (mod.Amount * amount_rounded);
                            break;
                        case modTypeEnum.Remove:
                            SelectedObjectiveValue.Amount -= (mod.Amount * amount_rounded);
                            break;
                        case modTypeEnum.Multiply:
                            SelectedObjectiveValue.Amount *= (mod.Amount * amount_rounded);
                            break;
                        case modTypeEnum.Divide:
                            SelectedObjectiveValue.Amount = (int)Math.Round(SelectedObjectiveValue.Amount / (mod.Amount * amount_rounded));
                            break;
                        default:
                            Debug.WriteLine("ERROR");
                            break;
                    }
                }
            }
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < ObjectiveValueCollection.Count; i++)
            {
                ObjectiveValue val = ObjectiveValueCollection[i];
                val.CurrentTime = DateTime.Now;
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e) => WriteJsonObjectiveValueCollection(ObjectiveValueCollection);

        [RelayCommand]
        private void AddModifierTimeStamp()
        {
            if (SelectedModifier is not null)
            {
                SelectedModifierTimeLine.Add(SelectedModifier);
                SelectedObjectiveValue.Modifiers = SelectedModifier.Modifiers;
            }
        }

        [RelayCommand]
        private void SelectModifierTimeStamp(object mod)
        {
            if (mod is Modifier)
            {
                Modifier modifier = (Modifier)mod;
                int index = SelectedModifierTimeLine.IndexOf(modifier);
                SelectedObjectiveValue.Modifiers = modifier.Modifiers;
                SelectedModifierTimeLine = new ObservableCollection<Modifier>(SelectedModifierTimeLine.TakeWhile(x => SelectedModifierTimeLine.IndexOf(x) <= index));
            }
            else if (mod is ObjectiveValue)
            {
                ObjectiveValue objectiveValue = (ObjectiveValue)mod;
                SelectedObjectiveValue.Modifiers = objectiveValue.Modifiers;
                SelectedModifierTimeLine = new ObservableCollection<Modifier>(); 
            }
        }

        public ObjectiveValueViewModel()
        {
            ObjectiveValueCollection = ReadJsonObjectiveValueCollection();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        #region Json Functions
        private ObservableCollection<ObjectiveValue> ReadJsonObjectiveValueCollection()
        {
            string text = File.ReadAllText(@"ObjectiveValueCollection.json");
            return JsonSerializer.Deserialize<ObservableCollection<ObjectiveValue>>(text);
        }

        public void WriteJsonObjectiveValueCollection(ObservableCollection<ObjectiveValue> obj)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
            string jsonObjString = JsonSerializer.Serialize<ObservableCollection<ObjectiveValue>>(obj, options);
            File.WriteAllText(@"ObjectiveValueCollection.json", jsonObjString);
        }
        #endregion

    }
}
