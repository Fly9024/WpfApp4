using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4.Classes
{
    internal class DiagramClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double maxX { get; set; } = 0;

        public double maxY { get; set; } = 0;
        
       public void Raise()
        {
           // PropertyChanged(this, new PropertyChangedEventArgs("maxX"));
           // PropertyChanged(this, new PropertyChangedEventArgs("maxY"));
        }
    }
}
