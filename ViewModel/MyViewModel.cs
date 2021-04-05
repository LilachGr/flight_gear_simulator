using flight_gear_simulator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flight_gear_simulator.ViewModel
{
   public  class MyViewModel : INotifyPropertyChanged
    {
        private IModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        bool isDisconnected = false;

        public MyViewModel(IModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) { NotifyPropertyChanged("VM_" + e.PropertyName); };
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        /*public string Path3
        {
            get;
            set => model.CsvpathSet(value);
        }*/

        //property to bind data with the textbox
        public string VMCsvPath { get; set; }

        // check if the path is for csv file
        public bool VMcorrectCsv()
        {
            return model.CorrectCSV;
        }

        //checking if we can connect to the givn Ip and Port
        public bool VMcorrectIP_Port()
        {
            return model.CorrectIp_Port;
        }

        // initialize the csvPath
        public void UploadPath()
        {
            model.CsvpathSet(VMCsvPath);
        }

        //set the ip
        public string VM_Ip { get; set; }
       
        //set the port
        public int VM_Port { get; set; }

        // initialize the model ip
        public void modelIP()
        {
            model.Ip = VM_Ip;
        }

        // initialize the model port
        public void modelPort()
        {
            model.Port = VM_Port;
        }

        //to connect to the FlightGear
        public void VM_connect()
        {
            this.model.connect();
        }

        public void VM_disconnect()
        {
            this.model.disconnect();
            isDisconnected = true;
        }

        public bool Vm_isDisconnected()
        {
            return this.isDisconnected;
        }

        //before to connect set all of the IP and the Port
        public void VM_BeforeConnection()
        {
            modelIP();
            modelPort();
        }
         
        // the csv flying one time without sitting
        public void VM_Start1()
        {
            this.model.start1();
        }
    }
}
