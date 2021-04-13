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
        ITelnetClient telnetClient;
        IDllFunctions dllFunc;
        private string csvPath;
        private int port;
        private string ip;
        StreamReader sr;
        bool correctCSV =true;
        bool correctIP_port = true;
        bool isDllHasProblem = false;
        Thread thread;
        Dictionary<string, int> xmlValues = new Dictionary<string, int>();
        List<string> xmlValuesInOrder = new List<string>();
        private IntPtr ptrForDll = IntPtr.Zero;

        public int Port
        {
            get { return this.port; }
            set
            {
                this.port = value;
                NotifyPropertyChanged("Port");
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        public MyModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            this.csvData = new List<List<float>>();
        }

        // start to fly by csv one time
        public void Start1()
        {
            thread = new Thread(new ThreadStart(delegate ()
            {
                this.telnetClient.Start(this.csvPath, this);
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void Disconnect()
        {
            if (thread != null)
            {
                thread.Abort();
                this.telnetClient.Disconnect();
            }
        }

        public void EnqueueMsg(double val, string message)
        {
            throw new NotImplementedException();
        }

        private void ReadData()
        {
            try
            {
                string empty = null;
                if (String.Equals(empty, this.csvPath))
                {
                    MessageBox.Show("Error!" + "\n" + "choose correct csv file path!");
                    this.correctCSV = false;
                    return;
                }
                    string csvfile = ".csv";
                    string str = this.csvPath.Substring(csvPath.Length - 4);
                
                if (String.Equals(csvfile, str))
                {
                    sr = new StreamReader(this.csvPath);
                }
                else
                {
                    MessageBox.Show("Error!" + "\n" + "choose correct csv file path!");
                    this.correctCSV = false;
                    return;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error!"+"\n"+"choose correct csv file path!");
                this.correctCSV = false;
                return;
                
            }
            this.correctCSV = true;
            string line;
            while ((line = sr.ReadLine()) != null)
            {
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

        public bool CorrectIp_Port
        {
            get { return this.correctIP_port; }
        }

        public List<List<float>> GetData()
        {
            return this.csvData;
        }

        public void CsvpathSet(string value)
        {
            csvPath = value;
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

        public void CreateValuesFromXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\biu_exercise\advandedProgramming2\flight_gear_simulator\playback_small.xml");
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
    }
}
