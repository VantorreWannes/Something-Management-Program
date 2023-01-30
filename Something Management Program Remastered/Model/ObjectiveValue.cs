using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Something_Management_Program_Remastered.Model
{
    public partial class ObjectiveValue : ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        private string name = "Empty ObjectiveValue";

        [ObservableProperty]
        private string type = "?";

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private float amount = 0;

        [ObservableProperty]
        private ObservableCollection<Modifier> modifiers = new ObservableCollection<Modifier>() {};

        [ObservableProperty]
        private DateTime currentTime = DateTime.Now;

        [ObservableProperty]
        private DateTime setTime = DateTime.Now;

        [JsonConstructor]
        public ObjectiveValue() { }



    }
}
