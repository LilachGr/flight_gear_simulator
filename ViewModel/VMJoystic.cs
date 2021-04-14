using flight_gear_simulator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flight_gear_simulator.ViewModel
{
  public class VMJoystic: INotifyPropertyChanged
    {
        IModel model;
        public VMJoystic(IModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);

            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


        public float VM_Throttle
        {
            get
            {
                int max = 1;
                int min = 0;
                return (model.Throttle - min) / (max - min) * 100;

            }

        }
        public float VM_Rudder
        {
            get
            {
                int max = 1;
                int min = -1;
                return (model.Rudder - min) / (max - min) * 100;
            }

        }
        public float VM_Aileron
        {
            get
            {

                return model.Aileron;

            }

        }
        public float VM_Elevator
        {
            get
            {

                return model.Elevator;

            }

        }
    }
}
