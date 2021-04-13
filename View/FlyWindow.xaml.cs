using ADP2_FLIGHTGEAR.View;
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
    /// Interaction logic for FlyWindow.xaml
    /// </summary>
    public partial class FlyWindow : Window
    {
        MyViewModel vm;
        private static bool isFlyStarted = false;
        private static bool isConnectDll = false;
        public FlyWindow(MyViewModel vm)
        {
            this.vm = vm;
            InitializeComponent();
            connect.IsEnabled = false;
        }

        private void Button_JustCsvFlyOption(object sender, RoutedEventArgs e)
        {
            if (isFlyStarted)
            {
                vm.VM_disconnect();
                isFlyStarted = false;
            }
            isFlyStarted = true;
            //flying by the user story1 definition (one time) without controls
            vm.VM_Start1();
            //MessageBox.Show("fly ended choose another option!");
            MessageBox.Show("Started the flight in the FlightGear application!");
        }

        private void Button_StopFly(object sender, RoutedEventArgs e)
        {
            stopFly.IsEnabled = false;
            flyCsv.IsEnabled = false;
            connect.IsEnabled = true;
            vm.VM_disconnect();
        }

        private void Mainwindow_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Vm_isDisconnected())
            {
                // vm.VM_disconnect();
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                vm.VM_disconnect();
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            //vm.VM_disconnect();
            if (vm.Vm_isDisconnected())
            {
                Application.Current.Shutdown();
            }
            else
            {
                vm.VM_disconnect();
                Application.Current.Shutdown();
            }
        }

        private void Button_Connect(object sender, RoutedEventArgs e)
        {
            Connection connect = new Connection(vm);
            connect.Show();
            this.Close();
            //stopFly.IsEnabled = true;
            //flyCsv.IsEnabled = true;

            //Connection connect = new Connection(vm);
            // connect.Show();
            // this.Close();
        }

        private void Button_DataInvestigation(object sender, RoutedEventArgs e)
        {
            if (isFlyStarted && isConnectDll)
            {
                DataInvestigation investigation = new DataInvestigation(vm);
                investigation.Show();
                this.Close();
            } else if (!isFlyStarted)
            {
                MessageBox.Show("You need to start the flight before using this option!");
            } else if (!isConnectDll)
            {
                MessageBox.Show("You need to connect a dll before using this option!");
            }
        }

        private void Button_ConnectDll(object sender, RoutedEventArgs e)
        {
            isConnectDll = true;
            DllConnection connect = new DllConnection(vm);
            connect.Show();
            this.Close();
        }
    }
}
