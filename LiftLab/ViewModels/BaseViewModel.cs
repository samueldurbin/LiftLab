using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.ViewModels
{
    // this was learnt from a YouTube Learning video
    // https://www.youtube.com/watch?v=XmdBXuNPShs&t=685s
    public class BaseViewModel : INotifyPropertyChanged  // INotifyPropertyChanged allows the class to use PropertyChanged events
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isBusy;

        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "") // method for property setters
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                // prevents uneccessary updates, compares new value to old value
                return false;
            } 

            backingField = value;
            OnPropertyChanged(propertyName); // calls for update
            return true;
        }

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) // notifys property changes
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
