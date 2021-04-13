using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace flight_gear_simulator.Model
{
    public interface IModel : INotifyPropertyChanged
    {
        //connect to server
        List<List<float>> GetData();
        void Connect();
        //start the fly
        void Start1();
        //disconnect from server
        void Disconnect();
        //add a message to server to queue.
        void EnqueueMsg(double val, string message);
        void CsvpathSet(string value);
        //get the liveData list.
        List<List<(DateTime, float)>> GetLiveData();
        //update the liveData.
        void UpdateDataLive(string line);
        //create the values from the playback_small xml
        void CreateValuesFromXML();
        Dictionary<string, int> GetXmlValue();
        //get dll Address and create a connection to the dll
        void DllConnect(string dllAdr, string CsvLearnPath, float Threshold);
        //disconnect from the dll
        void DllDisconnect();
        //return if dll is connected
        bool IsDllConnected();
        //get the correlated value of specific feature
        string GetCorrelatedValue(string feature);
        int GetCorrelatedIndex(string feature);


        //connection
        int Port { get; set; }
        string Ip { get; set; }
        bool CorrectCSV { get;}
        bool CorrectIp_Port { get; }
    }
}
