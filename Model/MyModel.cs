using ADP2_FLIGHTGEAR.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace flight_gear_simulator.Model
{
    class MyModel : IModel
    {
        private List<List<float>> csvData;
        private List<List<(DateTime, float)>> liveData = new List<List<(DateTime, float)>> ();
        private List<string> DataFlight = new List<string>(); // the data with the ","
        ITelnetClient telnetClient;
        IDllFunctions dllFunc;
        private string csvPath;
        bool correctCSV = true;
        private string xmlPath;
        bool correctXML = true;
        private int port;
        private string ip;
        StreamReader sr;
        bool correctIP_port = true;
        bool isDllHasProblem = false;
        Thread thread;
        Dictionary<string, int> xmlValues = new Dictionary<string, int>();
        List<string> xmlValuesInOrder = new List<string>();
        private IntPtr ptrForDll = IntPtr.Zero;
        public int setIndex=0;
        string setIndexo;
        bool isStoped = false;
        bool PauseUnitlStart = false;
        ManualResetEvent AirPlainStop_Start = new ManualResetEvent(false);
        public event PropertyChangedEventHandler PropertyChanged;
        //lists for the features
        List<float> listUpdated;

        public MyModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            this.csvData = new List<List<float>>();
        }

        public void NotifyPropertyChanged(string propName)
        {
           // if (this.PropertyChanged != null)
             //   this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public int Port
        {
            get { return this.port; }
            set
            {
                this.port = value;
                NotifyPropertyChanged("Port");
            }
        }

        public void CsvpathSet(string value)
        {
            csvPath = value;
            ReadData();
        }

        public string Ip
        {
            get { return this.ip; }
            set
            {

                this.ip = value;
                NotifyPropertyChanged("Ip");
            }
        }

        //loop stoped
        public bool IsStopedLoop
        {
            get
            {
                return this.isStoped;
            }
            set
            {
                isStoped = value;
            }
        }

        //slider//
        public int SetIndex
        {
            get { return this.setIndex; }
            set
            {

                this.setIndex = value;
                NotifyPropertyChanged("SetIndex");


            }
        }
        public int csvSize
        {
            get { return this.DataFlight.Count; }
        }
        // start to fly by csv one time
      /*  public void Start1()
        {
            thread = new Thread(new ThreadStart(delegate ()
            {
                this.telnetClient.Start(this.csvPath, this);
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }*/


        public void Disconnect()
        {
            if (thread != null)
            {
                thread.Abort();
                this.telnetClient.Disconnect();
            }
        }
        public void SetIndexToBack()
        {
            if (setIndex > 10)
            {
                setIndex -= 10;
            }
            else
            {
                setIndex = 0;
            }

        }
        public void SetIndextoFront()
        {
            if (setIndex <= DataFlight.Count)
            {

                setIndex += 10;
            }

        }           
            

        public string Setindxo
        {

            get { return this.setIndex.ToString(); }
            set
            {
                setIndexo = value;
                NotifyPropertyChanged("Setindxo");
            }
        }
        //pause the video
        public void Pause()
        {
            this.PauseUnitlStart = true;
            AirPlainStop_Start.Reset();
        }
        //play the video
        public void Play()
        {
            this.PauseUnitlStart = false;
            AirPlainStop_Start.Set();

        }
        private string time = "00:00:00";
        public string Time
            {
            get
            {
                return time;
            }
            set
            {
                time = value;
                NotifyPropertyChanged("Time");
            }

        }


        // The function sets the current time according to the index row which is screening in FG
        public void setTime()
        {
            int miliSeconds = SetIndex % 10;
            int secends = (SetIndex / 10) % 60;
            int minutes = SetIndex / 600;
            Time = (minutes.ToString("D2")) + ":" + (secends.ToString("D2")) + ":" + ((miliSeconds * 10).ToString("D2"));

        }
        private int speedsend = 50;
        public int Speedsend
        {
            get
            {
                return speedsend;
            }
            set
            { 
                speedsend = value;
            }
        }

        public void changeSpeed(double speed)
        {
            speedsend = (int)(50 / speed);
        }

        public Thread myThread
        {
            get { return this.thread; }
        }
        /// <summary>
        /// 
        /// </summary>

        //the loop
        public void Start1()
        {
            thread = new Thread(new ThreadStart(delegate ()
            {
                
                while (true)
                {
                    if (setIndex < DataFlight.Count)
                    {
                        this.telnetClient.Write(DataFlight[setIndex] + "\n");
                        UpdateDataLive(DataFlight[setIndex]);
                        SetIndex++;
                        Setindxo = SetIndex.ToString();
                        setTime();
                        Aileron = updateElement("aileron");
                        Elevator = updateElement("elevator");
                        Rudder = updateElement("rudder");
                        Throttle = updateElement("throttle");
                        Altimeter = updateElement("altimeter_indicated-altitude-ft");
                        Airspeed = updateElement("airspeed-kt");
                        Roll = updateElement("roll-deg");
                        Pitch = updateElement("pitch-deg");
                        Yaw = updateElement("side-slip-deg");
                        Direction = updateElement("heading-deg");
                        Thread.Sleep(speedsend);
                    }
                    Thread.Sleep(100);
                    if (PauseUnitlStart)
                    {
                        AirPlainStop_Start.WaitOne();
                    }
                }
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }





       

        public void EnqueueMsg(double val, string message)
        {
            throw new NotImplementedException();
        }


        private void ReadData()
        {
            try
            {
                string emptycsv = null;
                string emptyxml = null;
                if (String.Equals(emptycsv, this.csvPath)&&String.Equals(emptyxml,this.xmlPath))
                {
                    MessageBox.Show("Error!" + "\n" + "choose correct files path!");
                    this.correctCSV = false;
                    this.correctXML = false;
                    return;
                }
                string csvfile = ".csv";
                string xmlfile = ".xml";
                string strcsv = this.csvPath.Substring(csvPath.Length - 4);
                string strxml = this.xmlPath.Substring(xmlPath.Length - 4);

                if (String.Equals(csvfile, strcsv)&&String.Equals(xmlfile,strxml))
                {
                    sr = new StreamReader(this.csvPath);
                    Xmlfeatures(this.xmlPath);
                }
                else
                {
                    MessageBox.Show("Error!" + "\n" + "choose correct files path!");
                    this.correctCSV = false;
                    this.correctXML = false;
                    return;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error!" + "\n" + "choose correct  files path!");
                this.correctCSV = false;
                this.correctXML = false;
                return;

            }
            this.correctCSV = true;
            this.correctXML = true;
            string line;
            // without the ","

            while ((line = sr.ReadLine()) != null)
            {
                this.DataFlight.Add(line);
                List<float> data = new List<float>();
                var values = line.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    data.Add(float.Parse(values[i]));
                }
                this.csvData.Add(data);
            }

        }
        public bool CorrectCSV
        {
            get { return this.correctCSV; }
        }
        public bool CorrectXml
        {

            get
            {
                return this.correctXML;
            }
        }
        public bool CorrectIp_Port
        {
            get { return this.correctIP_port; }
        }
        public List<List<float>> GetData()
        {
            return this.csvData;
        }

        //set the paths of the files
        public void pathsSet(string csvFormat,string xmlFormat)
        {
            csvPath = csvFormat;
            xmlPath = xmlFormat;
            ReadData();
        }
        public void IPSet(string value)
        {
            ip = value;
        }
        public void PortSet(int value)
        {
            port = value;
        }

        public void Connect()
        {
            this.telnetClient.Connect(ip, port);
            this.correctIP_port = this.telnetClient.CorrectIp_port;
        }

        public List<List<(DateTime, float)>> GetLiveData()
        {
            return liveData;
        }

        public void UpdateDataLive(string line)
        {
            List<(DateTime, float)> data = new List<(DateTime, float)>();
            var values = line.Split(',');
            int size = values.Length;
            DateTime time = DateTime.Now;
            for (int i = 0; i < size; i++)
            {
                data.Add((time, float.Parse(values[i])));
            }
            this.liveData.Add(data);
            NotifyPropertyChanged("liveData");
        }

        public void CreateValuesFromXML(string xmlpath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlpath);
            XmlNodeList nodeInput = doc.GetElementsByTagName("input");
            int i = 0;
            foreach (XmlNode nodeChunk in nodeInput[0])
            {
                if (nodeChunk.Name == "chunk")
                {
                    string text = nodeChunk.ChildNodes[0].InnerText;
                    int j = 2;
                    while (xmlValues.ContainsKey(text))
                    {
                        text = text + j;
                        j++;
                    }
                    xmlValues.Add(text, i);
                    xmlValuesInOrder.Add(text);
                    i++;
                }
               
            }
        }
        public Dictionary<string, int> GetXmlValue()
        {
            return this.xmlValues;
        }

        public IntPtr PtrForDll
        {
            get
            {
                return ptrForDll;
            }
            set
            {
                if (value != IntPtr.Zero)
                {
                    ptrForDll = value;
                }
            }
        }
        //set the dll address
        public void DllConnect(string dllAdr, string csvLearnPath, float threshold)
        {
            if (this.dllFunc != null && this.dllFunc.IsDllConnected())
            {
                DllDisconnect();
            }
            this.dllFunc = new MyDllFunctions(dllAdr);
            isDllHasProblem = !this.dllFunc.IsDllConnected();
            if (!isDllHasProblem)
            {
                string newCsvPath = CreateCsvWithTitle(csvLearnPath);
                PtrForDll = this.dllFunc.Dll_SetAllCorrelatedFeature(newCsvPath, threshold);
            }
            if (PtrForDll == IntPtr.Zero)
            {
                isDllHasProblem = true;
            }
        }

        private string CreateCsvWithTitle(string csvLearnPath)
        {
            string newCsvFile = "csvLearnPathWithTitle.csv";
            var csv = new StringBuilder();
            StreamReader readCsv;
            if (csvLearnPath == null)
            {
                return null;
            }
            try
            {
                string empty = null;
                if (String.Equals(empty, csvLearnPath))
                {
                    MessageBox.Show("Error!" + "\n" + "choose correct csv file path!");
                    return null;
                }
                string csvfile = ".csv";
                string str = csvLearnPath.Substring(csvLearnPath.Length - 4);

                if (String.Equals(csvfile, str))
                {
                    readCsv = new StreamReader(csvLearnPath);
                }
                else
                {
                    MessageBox.Show("Error!" + "\n" + "choose correct csv file path!");
                    return null;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error!" + "\n" + "choose correct csv file path!");
                return null;
            }
            StringBuilder title = new StringBuilder();
            if (this.xmlValuesInOrder.Count <= 0)
            {
                return null;
            }
            int size = this.xmlValuesInOrder.Count;
            for (int i = 0; i < size; i++)
            {
                title.Append(this.xmlValuesInOrder[i]);
                if (i != size - 1)
                {
                    title.Append(",");
                }
            }
            csv.AppendLine(title.ToString());
            string line;
            while ((line = readCsv.ReadLine()) != null)
            {
                csv.AppendLine(line);
            }
            File.WriteAllText(newCsvFile, csv.ToString());
            return newCsvFile;
         }

        public void DllDisconnect()
        {
            if (this.dllFunc.IsDllConnected())
            {
                this.dllFunc.DllDisconnect();
            }
        }

        public bool IsDllConnected()
        {
            return !isDllHasProblem;
        }

        public string GetCorrelatedValue(string feature)
        {
            int index = GetCorrelatedIndex(feature);
            if (index == -1)
            {
                isDllHasProblem = false;
                return null;
            }
            return xmlValuesInOrder[index];
        }
        public int GetCorrelatedIndex(string feature)
        {
            return this.dllFunc.Dll_GetCorrelatedFeature(this.PtrForDll, feature);
        }


        // JOYSTIC//
        ////// Joystick //////



        //property of the Aileron 
        private float aileron = 125;
        public float Aileron
        {
            get { return aileron; }
            set
            {
                aileron = 125 + 100 * value;
                NotifyPropertyChanged("Aileron");
            }
        }

        //property of the Elevator
        private float elevator = 125;
        public float Elevator
        {
            get { return elevator; }
            set
            {
                elevator = 125 + 100 * value;
                NotifyPropertyChanged("Elevator");
            }

        }

        //property of the Rudder
        private float rudder;
        public float Rudder
        {
            get
            {
                return rudder;
            }
            set
            {
                rudder = value;
                NotifyPropertyChanged("Rudder");
            }

        }

        //property of the throttle
        private float throttle;
        public float Throttle
        {
            get
            {
                return throttle;
            }
            set
            {
                throttle = value;
                NotifyPropertyChanged("Throttle");
            }

        }


        // property of the list features
        List<string> xfeatures = new List<string>();
        public List<string> ColumnList
        {
            get
            {
                return xfeatures;
            }
        }
      
        // updating the elment in the flight
        public float updateElement(string feature)
        {
            int index = xfeatures.FindIndex(a => a.Contains(feature)); // the index number of the feature 
            if (SetIndex < DataFlight.Count)
            {
                this.listUpdated = csvData.ElementAt(SetIndex);
            }
            return listUpdated.ElementAt(index);
        }
        //get the features from the xml
        public void Xmlfeatures(string path)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path);
            XmlNodeList name = xdoc.GetElementsByTagName("name");
            int size = name.Count / 2; // read only the first part(the input) in the XML file
            for (int i = 0; i < size; i++)
            {
                string theName = name[i].InnerText;
               xfeatures.Add(theName);
            }
        }



        /// <summary>
        /// 
        /// </summary>










        //DASHBOARD

        //property of the Airspeed
        private float airspeed;
        public float Airspeed
        {
            set
            {
                airspeed = value;
                NotifyPropertyChanged("Airspeed");
            }
            get
            {
                return airspeed;
            }
        }

        //propert of the Roll
        private float roll;
        public float Roll
        {
            set
            {
                roll = value;
                NotifyPropertyChanged("Roll");
            }
            get
            {
                return roll;
            }
        }

        //property of the Direction
        private float direction;
        public float Direction
        {
            set
            {
                direction = value;
                NotifyPropertyChanged("Direction");
            }
            get
            {
                return direction;
            }
        }

        //property of the Pitch
        private float pitch;
        public float Pitch
        {
            set
            {
                pitch = value;
                NotifyPropertyChanged("Pitch");
            }
            get
            {
                return pitch;

            }
        }

        //property of the Yaw
        private float yaw;
        public float Yaw
        {
            set
            {
                yaw = value;
                NotifyPropertyChanged("Yaw");
            }

            get
            {
                return yaw;
            }
        }

        //property of the Altimeter
        private float altimeter;

        public float Altimeter
        {
            get { return altimeter; }
            set
            {
                altimeter = value;
                NotifyPropertyChanged("Altimeter");
            }
        }










    }
}
