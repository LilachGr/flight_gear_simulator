using flight_gear_simulator;
using flight_gear_simulator.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ADP2_FLIGHTGEAR
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : Window
    {
        MyViewModel vm;
        public Connection(MyViewModel vm)
        {
            this.vm = vm;
            
            InitializeComponent();
        }

        private void Button_Connect(object sender, RoutedEventArgs e)
        {    
             //initialize the IP and the Port
             vm.VM_BeforeConnection();
             //connect
             vm.VM_connect();
            // if we can connect to the port respectivliy with the Ip then connect
            if (vm.VMcorrectIP_Port())
            {
                MessageBox.Show("connected successfully");
                FlyWindow fly = new FlyWindow(vm);
                fly.DataContext = this;
                fly.Show();
                this.Close();
            }

        }
    }
}
