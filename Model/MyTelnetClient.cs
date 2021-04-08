using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace flight_gear_simulator.Model
{
    class MyTelnetClient : ITelnetClient
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        private Mutex mutex = new Mutex();
        private bool correctIp_Port = true;

        //Connecting to the server.
       
        //check if we can use this port
        public static bool IpPorTInUse(string ip,int port)
        {
            bool inUse = false;
           
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
          
            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                string IPendpoint = endPoint.Address.ToString();
                if (endPoint.Port == port&&String.Equals(IPendpoint, ip))
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }

        //check if we can use this ip   
        public void Connect(string ip, int port)
        {
            this.tcpClient = new TcpClient();
            bool IpPortReady = IpPorTInUse(ip,port);
            //bool portReady = PortInUse(port);
            //Initialize the tcpClient.
            //Try to establish a connection to the server.

            try
            {                
                if (IpPortReady)
                {
                    tcpClient.Connect(ip, port);
                    stream = tcpClient.GetStream();
                }
                else
                {
                    this.correctIp_Port = false;
                    MessageBox.Show("Wrong port or IP" + "\n" + "try again!");
                    return;
                }
                this.correctIp_Port = true;
            }
            //Catch an exception in case establishing doesn't work.
            catch (IOException)
            {
                this.correctIp_Port = false;
                MessageBox.Show("Wrong port or IP" + "\n" + "try again!");
            }
        }

        public void Disconnect()
        {
            tcpClient.GetStream().Close();
           // this.tcpClient.Close();
            tcpClient.Close(); //disconnect from the server.
            MessageBox.Show("disconnected!");
        }

        public bool CorrectIp_port { get { return this.correctIp_Port; } }
   
        public string Read()
        {
            throw new NotImplementedException();
        }

        //Send a message to the server.
        public void Write(string command)
        {
            Byte[] encodedMsg = Encoding.ASCII.GetBytes(command);
            //try to send the message to the server.
            try
            {
                tcpClient.GetStream().Write(encodedMsg, 0, encodedMsg.Length);
            }
            catch
            {
                Console.WriteLine();
            }
        }

        //to start the flying by the csv file
        public void Start(string path, IModel model)
        {
            // connect("127.0.0.1", 5400);           
            StreamReader sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Write(line + "\n");
                model.UpdateDataLive(line);
                Thread.Sleep(100);
            }           
        }
    }
}
