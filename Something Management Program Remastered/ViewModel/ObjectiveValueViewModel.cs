using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Something_Management_Program_Remastered.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

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
                SelectedObjectiveValue.Modifiers.Remove(SelectedModifier);
                WriteJsonObjectiveValueCollection(ObjectiveValueCollection);
            }
        }

        [RelayCommand]
        private void SkipTime()
        {
            WriteJsonObjectiveValueCollection(ObjectiveValueCollection);
            if (SelectedObjectiveValue is not null)
            {
                foreach (Modifier mod in SelectedObjectiveValue.Modifiers)
                {
                    if (mod.Interval == TimeSpan.Zero){ return; }
                    double amount = (SelectedObjectiveValue.SetTime - SelectedObjectiveValue.CurrentTime).Ticks / (double)mod.Interval.Ticks;
                    int amount_rounded = (int)Math.Round(amount);
                    Debug.WriteLine(amount_rounded);
                    if (amount_rounded == 0 ) { return; }
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
                    return;
                }
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e) => WriteJsonObjectiveValueCollection(ObjectiveValueCollection);

        public ObjectiveValueViewModel()
        {
            ObjectiveValueCollection = ReadJsonObjectiveValueCollection();
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
