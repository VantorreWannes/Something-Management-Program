using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Something_Management_Program_Remastered.Model
{
    public partial class Project : ObservableObject, INotifyPropertyChanged
    {

        [ObservableProperty]
        private ObservableCollection<ObjectiveValue> objectiveValueCollection;

        [ObservableProperty]
        private ObservableCollection<Modifier> modifierTree = new ();

        [ObservableProperty]
        private ObservableCollection<Modifier> displayedModifiers = new() { };

        [JsonConstructor]
        public Project() { }
    }
}
