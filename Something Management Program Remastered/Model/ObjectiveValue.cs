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
        private string name;

        [ObservableProperty]
        private string type;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private float amount;

        [ObservableProperty]
        private ObservableCollection<Modifier> modifiers;

        [ObservableProperty]
        private DateTime currentTime;

        [JsonConstructor]
        public ObjectiveValue() { }

        

    }
}
