using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Something_Management_Program_Remastered.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
            ObjectiveValueCollection.Add(item: new ObjectiveValue { Name = "Empty ObjectiveValue", Amount = 0, CurrentTime = DateTime.Now });
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
                SelectedObjectiveValue.Modifiers.Add(item: new Modifier { Name = "Empty Modifier", Amount = 0, Interval = DateTime.Now });
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

        public ObjectiveValueViewModel()
        {
            ObjectiveValueCollection = ReadJsonObjectiveValueCollection();
        }

        #region Json Functions
        private ObservableCollection<ObjectiveValue> ReadJsonObjectiveValueCollection()
        {
            string text = File.ReadAllText(@"ObjectiveValueCollection.json");
            return JsonSerializer.Deserialize<ObservableCollection<ObjectiveValue>>(text) ?? new ObservableCollection<ObjectiveValue>();
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
