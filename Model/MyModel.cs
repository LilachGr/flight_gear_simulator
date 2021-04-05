﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace flight_gear_simulator.Model
{
    class MyModel : IModel
    {
        private List<List<float>> csvData;
        ITelnetClient telnetClient;
        private string csvPath;
        private int port;
        private string ip;
        StreamReader sr;
        bool correctCSV =true;
        bool correctIP_port = true;
        Thread thread;

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
        public void start1()
        {
             thread = new Thread(new ThreadStart(delegate ()
            {
                this.telnetClient.start(this.csvPath);
            }));
            thread.Start();
            
            /**
            new Thread(delegate ()
            {
                this.telnetClient.start(this.csvPath);
            }).Start();
            */
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
   
        public void disconnect()
        {
            thread.Abort();
            this.telnetClient.disconnect();
        }

        public void EnqueueMsg(double val, string message)
        {
            throw new NotImplementedException();
        }

        //we need to check if its not very big data that will kill the memory.
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

        public void connect()
        {
            this.telnetClient.connect(ip, port);
            this.correctIP_port = this.telnetClient.correctIp_port;
        }
    }
}