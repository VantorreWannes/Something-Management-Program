using Something_Management_Program_Remastered.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace Something_Management_Program_Remastered.ViewModel
{
    class ObjectiveValueViewModel
    {

        private List<ObjectiveValue> objectiveValueCollection;

        public ObjectiveValueViewModel()
        {
            /*
            ObjectiveValueCollection = new List<ObjectiveValue>
            {
                new ObjectiveValue { Name = "ObjectiveValueOne", Amount = 100,
                    Modifiers = new List<Modifier>
                    {
                        new Modifier { Name="ModifierOne", Amount=10 },
                        new Modifier { Name = "ModifierTwo", Amount = 5 }
                    }
                },
                new ObjectiveValue { Name = "ObjectiveValueTwo", Amount = 50,
                    Modifiers = new List<Modifier>
                    {
                        new Modifier {Name="ModifierOne", Amount=10 },
                    }
                }
            };
            WriteJsonObjectiveValueCollection(ObjectiveValueCollection);
            */
            ObjectiveValueCollection = ReadJsonObjectiveValueCollection();
            
        }
        
        public List<ObjectiveValue> ObjectiveValueCollection
        {
            get
            {
                return objectiveValueCollection;
            }
            set
            {
                objectiveValueCollection = value;
                OnPropertyChanged("Name");
            }
        }

        private List<ObjectiveValue> ReadJsonObjectiveValueCollection()
        {
            string text = File.ReadAllText(@"ObjectiveValueCollection.json");
            return JsonSerializer.Deserialize<List<ObjectiveValue>>(text) ?? new List<ObjectiveValue>();
        }

        public void WriteJsonObjectiveValueCollection(List<ObjectiveValue> obj)
        {
            var options = new JsonSerializerOptions(){ WriteIndented = true };
            string jsonObjString = JsonSerializer.Serialize<List<ObjectiveValue>>(obj, options);
            File.WriteAllText(@"ObjectiveValueCollection.json", jsonObjString);
        }

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
            private Action action;
            private bool canExecute;
            public CommandHandler(Action action, bool canExecute)
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
                action();
            }
        }
        #endregion
    }
}
