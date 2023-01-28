using CommunityToolkit.Mvvm.ComponentModel;
using System;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm;
using System.ComponentModel;
using System.Collections.ObjectModel;

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
        private ObservableCollection<Modifier> modifiers = new ObservableCollection<Modifier>() { new Modifier() };

        [ObservableProperty]
        private DateTime currentTime = DateTime.Now;

        [ObservableProperty]
        private DateTime setTime = new DateTime();

        [JsonConstructor]
        public ObjectiveValue() { }
              

    }
}
