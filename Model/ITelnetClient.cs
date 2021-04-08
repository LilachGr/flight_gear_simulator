using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace flight_gear_simulator.Model
{
    interface ITelnetClient
    {
        //connect to the server.
        void Connect(string ip, int port);
        //write a message to the server.
        void Write(string command);
        //read back from the server.
        string Read();
        //disconnect from server.
        void Disconnect();
        //start flying one time without controls user story1
        void Start(string path, IModel model);
        //checking if we can connect to the given ip and port
         bool CorrectIp_port { get; }
    }
}
