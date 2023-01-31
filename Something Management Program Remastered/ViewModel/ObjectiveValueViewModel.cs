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


        [RelayCommand]
        private void SkipTime()
        {
            SelectedObjectiveValue.CurrentTime = DateTime.Now;
            WriteProjectInfo(ProjectInfo);
            foreach (Modifier mod in SelectedObjectiveValue.Modifiers)
            {
                if (mod.Interval == TimeSpan.Zero || SelectedObjectiveValue.SetTime < SelectedObjectiveValue.CurrentTime) { continue; }
                mod.Amount = ProccessModifiers(mod);
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
        private int ProccessModifiers(Modifier mod)
        {
            foreach (Modifier mod2 in mod.Modifiers) { mod2.Amount = ProccessModifiers(mod2); }

            if (mod.Interval == TimeSpan.Zero || SelectedObjectiveValue.SetTime < SelectedObjectiveValue.CurrentTime) { return mod.Amount; }
            double amount = (SelectedObjectiveValue.SetTime - SelectedObjectiveValue.CurrentTime).Ticks / (double)mod.Interval.Ticks;
            int amount_rounded = (int)Math.Round(amount);
            if (amount_rounded == 0) { return mod.Amount; }
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

        public void Timer_Tick(object sender, EventArgs e)
        {
            foreach (ObjectiveValue item in ProjectInfo.ObjectiveValueCollection)
            {
                item.CurrentTime = DateTime.Now;
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e) => WriteProjectInfo(ProjectInfo);



        [RelayCommand]
        private void AddModifierTreeItem()
        {
            if (SelectedModifier is not null)
            {
                ProjectInfo.ModifierTree.Add(SelectedModifier);
                ProjectInfo.DisplayModifiers = (ProjectInfo.ModifierTree[^1] as Modifier).Modifiers;
            }
        }



        [RelayCommand]
        private void SelectModifierTree(object mod)
        {
            int index = ProjectInfo.ModifierTree.IndexOf(mod);
            ProjectInfo.ModifierTree = new ObservableCollection<object>(ProjectInfo.ModifierTree.TakeWhile(x => ProjectInfo.ModifierTree.IndexOf(x) <= index));
            Debug.WriteLine(ProjectInfo.ModifierTree.Last());
            if (ProjectInfo.ModifierTree.Count == 1) { ProjectInfo.DisplayModifiers = (ProjectInfo.ModifierTree.Last() as ObjectiveValue).Modifiers ?? new(); }
            else { ProjectInfo.DisplayModifiers = (ProjectInfo.ModifierTree.Last() as Modifier).Modifiers ?? new(); }

        }

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
