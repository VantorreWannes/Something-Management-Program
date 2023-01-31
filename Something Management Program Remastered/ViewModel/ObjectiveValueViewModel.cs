using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Something_Management_Program_Remastered.Model;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Threading;

namespace Something_Management_Program_Remastered.ViewModel
{
    public partial class ObjectiveValueViewModel : ObservableObject, INotifyPropertyChanged
    {

        [ObservableProperty]
        private Project projectInfo;

        [ObservableProperty]
        private ObjectiveValue selectedObjectiveValue;

        [ObservableProperty]
        private Modifier selectedModifier;


        partial void OnSelectedObjectiveValueChanged(ObjectiveValue value)
        {
            ProjectInfo.ModifierTree.Clear();
            if (value is not null)
            {
                ProjectInfo.ModifierTree.Add(value);
                ProjectInfo.DisplayModifiers = (ProjectInfo.ModifierTree[0] as ObjectiveValue).Modifiers;
            }
            WriteProjectInfo(ProjectInfo);
        }

        [RelayCommand]
        private void NewObjectiveValue()
        {
            ProjectInfo.ObjectiveValueCollection.Add(item: new ObjectiveValue());
            WriteProjectInfo(ProjectInfo);
        }

        [RelayCommand]
        private void DeleteObjectiveValue()
        {
            if (SelectedObjectiveValue is not null && ProjectInfo.ObjectiveValueCollection.Count > 0)
            {
                ProjectInfo.ObjectiveValueCollection.Remove(SelectedObjectiveValue);
                WriteProjectInfo(ProjectInfo);
            }

        }

        [RelayCommand]
        private void NewModifier()
        {
            if (SelectedObjectiveValue is not null)
            {
                if (ProjectInfo.ModifierTree.Count == 1) { (ProjectInfo.ModifierTree[0] as ObjectiveValue).Modifiers.Add(new Modifier()); }
                else { (ProjectInfo.ModifierTree[^1] as Modifier).Modifiers.Add(new Modifier()); }
                WriteProjectInfo(ProjectInfo);
            }
        }

        [RelayCommand]
        private void DeleteModifier()
        {
            if (SelectedObjectiveValue is not null && ProjectInfo.DisplayModifiers.Count >= 1)
            {
                if (ProjectInfo.ModifierTree.Count == 1) { (ProjectInfo.ModifierTree[0] as ObjectiveValue).Modifiers.Remove(SelectedModifier); }
                else { (ProjectInfo.ModifierTree[^1] as Modifier).Modifiers.Remove(SelectedModifier); }
                WriteProjectInfo(ProjectInfo);
            }
        }

        /*
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
        */

        public void Timer_Tick(object sender, EventArgs e)
        {
            foreach (ObjectiveValue item in ProjectInfo.ObjectiveValueCollection)
            {
                item.CurrentTime = DateTime.Now;
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e) => WriteProjectInfo(ProjectInfo);

        /*

        [RelayCommand]
        private void AddModifierTreeItem()
        {
            if (SelectedModifier is not null)
            {
                SelectedModifierTree.Add(SelectedModifier);
                SelectedObjectiveValue.Modifiers = SelectedModifier.Modifiers;
            }
        }

        [RelayCommand]
        private void SelectModifierTree(Modifier mod)
        {
            int index = SelectedModifierTree.IndexOf(mod);
            SelectedObjectiveValue.Modifiers = mod.Modifiers;
            SelectedModifierTree = new ObservableCollection<Modifier>(SelectedModifierTree.TakeWhile(x => SelectedModifierTree.IndexOf(x) <= index));
        }
        private void ResetModifierTree()
        {

            SelectedModifierTree = new ObservableCollection<Modifier>();
        }

        */

        public ObjectiveValueViewModel()
        {
            ProjectInfo = ReadProjectInfo();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        #region Json Functions
        private Project ReadProjectInfo()
        {
            string text = File.ReadAllText(@"Project.json");
            return JsonSerializer.Deserialize<Project>(text);
        }

        public void WriteProjectInfo(Project obj)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
            string jsonObjString = JsonSerializer.Serialize<Project>(obj, options);
            File.WriteAllText(@"Project.json", jsonObjString);
        }
        #endregion

    }
}
