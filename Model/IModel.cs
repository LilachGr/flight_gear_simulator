using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;


namespace flight_gear_simulator.Model
{
    public interface IModel : INotifyPropertyChanged
    {
        //connect to server
        List<List<float>> GetData();
        // is stoped loop
        bool IsStopedLoop { get; set; }
        void Connect();
        //start the fly
        void Start1();
        //disconnect from server
        void Disconnect();
        //add a message to server to queue.
        void EnqueueMsg(double val, string message);
         void pathsSet(string csvFormat, string xmlFormat);
        void CsvpathSet(string value);
        //get the liveData list.
        List<List<(DateTime, float)>> GetLiveData();
        //update the liveData.
        void UpdateDataLive(string line);
        //create the values from the playback_small xml
        void CreateValuesFromXML(string VMxmlpath);
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
        //get MyCorrelatedFeature class and a feature and return double array like that: {startX, startY, endX, endY}. 
        //return null when error happen.
        float[] GetRegressionLine(string feature);


    //connection
    Thread myThread { get; }
        int Port { get; set; }
        string Ip { get; set; }
        bool CorrectCSV { get; }
         bool CorrectXml { get; }
        bool CorrectIp_Port { get; }

        //Control Flight Buttons

        // the setp back button
        void SetIndexToBack();
        //the step over button
        void SetIndextoFront();
        //pause the video
        void Pause();
        //play the video
        void Play();
        //property for the slider
        int SetIndex { get; set; }
        int csvSize { get; }
        string Setindxo { get; set; }
        void changeSpeed(double speed);
         int Speedsend { get; set; }


         string Time { get; set; }

        // set the time
         void setTime();

        ///DASHBOARD
        //property of the Airspeed
        float Airspeed { get; set; }
        //propert of the Roll
        float Roll { get; set; }
        //property of the Direction
        float Direction { get; set; }
        //property of the Pitch
         float Pitch { get; set; }
        //property of the Yaw
         float Yaw { get; set; }
        //property of the Altimeter
         float Altimeter { get; set; }

        //joystic
        ////// Joystick //////

        //property of the Aileron 
         float Aileron { get; set; }
        //property of the Elevator
         float Elevator { get; set; }
        //property of the Rudder
         float Rudder { get; set; }
        //property of the throttle
         float Throttle { get; set; }
        // property of the list features
        
        //updating the faetures in the xaml
        float updateElement(string feature);
        // get the features from the xaml
        void Xmlfeatures(string path);
    }
}
