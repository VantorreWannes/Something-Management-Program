using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Something_Management_Program_Remastered.Model
{
    public enum Type { Add, Remove, Divide, Multiply}

    public partial class Modifier : ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private Type modType;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private float amount;

        [ObservableProperty]
        private DateTime interval;

        [JsonConstructor]
        public Modifier() { }
    }
}
