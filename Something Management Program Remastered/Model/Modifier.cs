using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Something_Management_Program_Remastered.Model
{
    public enum Type { Add, Remove, Divide, Multiply}

    public class Modifier
    {
        private string name;
        private Type modType;
        private string description;
        private float amount;
        private DateTime interval;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public Type ModType
        {
            get
            {
                return modType;
            }
            set
            {
                modType = value;
                OnPropertyChanged("ModType");
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        public float Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
                OnPropertyChanged("Description");
            }
        }

        public DateTime Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
                OnPropertyChanged("Interval");
            }
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
    }
}
