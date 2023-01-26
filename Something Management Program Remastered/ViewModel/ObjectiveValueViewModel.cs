using Something_Management_Program_Remastered.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Input;

namespace Something_Management_Program_Remastered.ViewModel
{
    class ObjectiveValueViewModel
    {
        public ICommand SelectionChangedCommand { get; set; } 
        private List<ObjectiveValue> objectiveValueCollection;
        public List<ObjectiveValue> ObjectiveValueCollection
        {
            get { return objectiveValueCollection; }
            set
            {
                objectiveValueCollection = value;
                OnPropertyChanged("Name");
            }
        }

        private ObjectiveValue objectiveValueSelectedItem;
        public ObjectiveValue ObjectiveValueSelectedItem
        {
            get { return objectiveValueSelectedItem; }
            set
            {
                objectiveValueSelectedItem = value;
                OnPropertyChanged("ObjectiveValueSelectedItem");
                OnPropertyChanged("ObjectiveValueSelectedItemName");
                OnPropertyChanged("ObjectiveValueSelectedItemType");
                OnPropertyChanged("ObjectiveValueSelectedItemDescription");
                OnPropertyChanged("ObjectiveValueSelectedItemAmount");
                OnPropertyChanged("ObjectiveValueSelectedItemModifiers");
                OnPropertyChanged("ObjectiveValueSelectedItemCurrentTime");
            }
        }

        #region ObjectiveValueSelectedItem Properties
        public string ObjectiveValueSelectedItemName
        {
            get { return objectiveValueSelectedItem.Name; }
            set
            {
                objectiveValueSelectedItem.Name = value;
                OnPropertyChanged("ObjectiveValueSelectedItemName");
            }
        }
        public string ObjectiveValueSelectedItemType
        {
            get { return objectiveValueSelectedItem.Type; }
            set
            {
                objectiveValueSelectedItem.Type = value;
                OnPropertyChanged("ObjectiveValueSelectedItemType");
            }
        }
        public string ObjectiveValueSelectedItemDescription
        {
            get { return objectiveValueSelectedItem.Description; }
            set
            {
                objectiveValueSelectedItem.Description = value;
                OnPropertyChanged("ObjectiveValueSelectedItemDescription");
            }
        }
        public float ObjectiveValueSelectedItemAmount
        {
            get { return objectiveValueSelectedItem.Amount; }
            set
            {
                objectiveValueSelectedItem.Amount = value;
                OnPropertyChanged("ObjectiveValueSelectedItemAmount");
            }
        }
        public List<Modifier> ObjectiveValueSelectedItemModifiers
        {
            get { return objectiveValueSelectedItem.Modifiers; }
            set
            {
                objectiveValueSelectedItem.Modifiers = value;
                OnPropertyChanged("ObjectiveValueSelectedItemModifiers");
            }
        }
        public DateTime ObjectiveValueSelectedItemCurrentTime
        {
            get { return objectiveValueSelectedItem.CurrentTime; }
            set
            {
                objectiveValueSelectedItem.CurrentTime = value;
                OnPropertyChanged("ObjectiveValueSelectedItemCurrentTime");
            }
        }
        #endregion

        public ObjectiveValueViewModel()
        {
            //PropertyChanged += ObjectiveValueViewModel_PropertyChanged;
            ObjectiveValueCollection = ReadJsonObjectiveValueCollection();
            ObjectiveValueSelectedItem = ObjectiveValueCollection[0];
            SelectionChangedCommand = new CommandHandler((parameter) => ObjectiveValueSelectedItem = (ObjectiveValue)parameter, true);
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

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region ICommand Members 
        public class CommandHandler : ICommand
        {
            private Action<object> action;
            private bool canExecute;
            public CommandHandler(Action<object> action, bool canExecute)
            {
                this.action = action;
                this.canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return canExecute;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                action(parameter);
            }
        }
        
        #endregion
    }
}
