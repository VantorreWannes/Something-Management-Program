using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm;
using System.ComponentModel;

namespace Something_Management_Program_Remastered.Model
{
    public partial class ObjectiveValue : ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string type;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private float amount;

        [ObservableProperty]
        private List<Modifier> modifiers;

        [ObservableProperty]
        private DateTime currentTime;

        [JsonConstructor]
        public ObjectiveValue() { }

    }
}
