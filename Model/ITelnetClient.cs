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
        void connect(string ip, int port);
        //write a message to the server.
        void write(string command);
        //read back from the server.
        string read();
        //disconnect from server.
        void disconnect();
        //start flying one time without controls user story1
        void start(string path);
        //checking if we can connect to the given ip and port
         bool correctIp_port { get; }
    }
}
