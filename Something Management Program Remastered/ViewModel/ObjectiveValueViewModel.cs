using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Something_Management_Program_Remastered.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Something_Management_Program_Remastered.ViewModel
{
    public partial class ObjectiveValueViewModel : ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        private List<ObjectiveValue> objectiveValueCollection;

        [ObservableProperty]
        private ObjectiveValue selectedObjectiveValue;

        [RelayCommand]
        private void NewObjectviveValue()
        {
            ObjectiveValueCollection.Add(new ObjectiveValue { Name = "Empty", Amount = 0, CurrentTime = default});
            Debug.WriteLine("added");
        }

        [RelayCommand]
        private void DeleteObjectviveValue()
        {
            ObjectiveValueCollection.RemoveAll(x => x == SelectedObjectiveValue);
            Debug.WriteLine("removed");
        }

        public ObjectiveValueViewModel()
        {
            ObjectiveValueCollection = ReadJsonObjectiveValueCollection();
        }

        #region Json Functions
        private List<ObjectiveValue> ReadJsonObjectiveValueCollection()
        {
            string text = File.ReadAllText(@"ObjectiveValueCollection.json");
            return JsonSerializer.Deserialize<List<ObjectiveValue>>(text) ?? new List<ObjectiveValue>();
        }

        public void WriteJsonObjectiveValueCollection(List<ObjectiveValue> obj)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
            string jsonObjString = JsonSerializer.Serialize<List<ObjectiveValue>>(obj, options);
            File.WriteAllText(@"ObjectiveValueCollection.json", jsonObjString);
        }
        #endregion

    }
}
