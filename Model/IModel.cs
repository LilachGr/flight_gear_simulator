using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flight_gear_simulator.Model
{
    public interface IModel : INotifyPropertyChanged
    {
        //connect to server
        List<List<float>> GetData();
        void connect();
        //start the fly
        void start1();
        //disconnect from server
        void disconnect();
        //add a message to server to queue.
        void EnqueueMsg(double val, string message);
        void CsvpathSet(string value);

        //connection
        int Port { get; set; }
        string Ip { get; set; }
        bool CorrectCSV { get;}
         bool CorrectIp_Port { get; }
    }
}
