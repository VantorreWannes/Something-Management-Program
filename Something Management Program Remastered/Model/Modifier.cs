using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Something_Management_Program_Remastered.Model
{
    public enum modTypeEnum
    {
        Add,
        Remove,
        Multiply,
        Divide,
    }

    public partial class Modifier : ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        private string name = "Empty Modifier";

        [ObservableProperty]
        private modTypeEnum modType = 0;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private float amount = 0;

        [ObservableProperty]
        private TimeSpan interval = TimeSpan.Zero;

        [ObservableProperty]
        private ObservableCollection<Modifier> modifiers = new ObservableCollection<Modifier>();

        [JsonConstructor]
        public Modifier() { }
    }
}
