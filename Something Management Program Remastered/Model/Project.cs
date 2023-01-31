using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Something_Management_Program_Remastered.Model
{
    public partial class Project : ObservableObject, INotifyPropertyChanged
    {

        [ObservableProperty]
        private ObservableCollection<ObjectiveValue> objectiveValueCollection = new();

        [ObservableProperty]
        private ObservableCollection<object> modifierTree = new();

        [ObservableProperty]
        private ObservableCollection<Modifier> displayModifiers = new();

        [JsonConstructor]
        public Project()
        {

        }
    }
}
