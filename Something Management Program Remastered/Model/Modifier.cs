using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

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
