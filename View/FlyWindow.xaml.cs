using ADP2_FLIGHTGEAR.View;
using flight_gear_simulator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace flight_gear_simulator
{
    /// <summary>
    /// Interaction logic for FlyWindow.xaml
    /// </summary>
    public partial class FlyWindow : Window
    {
        MyViewModel vm;
        private static bool isFlyStarted = false;
        private static bool isConnectDll = false;
        VMJoystic vmJoy;
        DashBoardViewModel vmDash;
        bool isFlying = false;
        public FlyWindow(MyViewModel vm, VMJoystic vmJoy, DashBoardViewModel vmDash)
        {
            this.vm = vm;
            this.vmJoy = vmJoy;
            this.vmDash = vmDash;
            InitializeComponent();
            connect.IsEnabled = false;
            speed1.IsEnabled = false;
            speed2.IsEnabled = false;
            speed3.IsEnabled = false;
            speed4.IsEnabled = false;
            speed5.IsEnabled = false;
            speed6.IsEnabled = false;

        }

        private void Button_JustCsvFlyOption(object sender, RoutedEventArgs e)
        {
            if (isFlyStarted)
            {
                vm.VM_disconnect();
                isFlyStarted = false;
            }
            isFlyStarted = true;
            controler.VM_ViewModel = vm;
            controler.DataContext = vm;
            joystic.VM_JOYViewModel = vmJoy;
            dash.VM_dashViewModel = vmDash;
            slidermy.VM_sliderViewModel = vm;
            speed1.IsEnabled = true;
            speed2.IsEnabled = true;
            speed3.IsEnabled = true;
            speed4.IsEnabled = true;
            speed5.IsEnabled = true;
            speed6.IsEnabled = true;

            //flying by the user story1 defention(one time)without controls
            // slidero.IsEnabled = true;
            if (!isFlying)
            {
                vm.VM_Start1();
                isFlying = true;
                MessageBox.Show("Started the flight in the FlightGear application!");
            }
            else {
                vm.VM_SetIndex = 0;                
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
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

        private void Exit_Click_3(object sender, RoutedEventArgs e)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            vm.VM_Pause();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Connection connect = new Connection(vm,vmJoy,vmDash);
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
                DataInvestigation investigation = new DataInvestigation(vm, vmJoy, vmDash);
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
            DllConnection connect = new DllConnection(vm, vmJoy, vmDash);
            connect.Show();
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            vm.VM_SetIndexToBack();
        }

        private void Button_Click5(object sender, RoutedEventArgs e)
        {
            vm.VM_SetIndextoFront();
        }

        private void Button_Click_Play(object sender, RoutedEventArgs e)
        {
            vm.VM_Play();
        }


        //speed change in the flight
        private void Speedchange_Click_05(object sender, RoutedEventArgs e)
        {
            speedTex.Text = "0.5";
            vm.VM_speedsend = 0.5;
        }
        private void Speedchange_Click_075(object sender, RoutedEventArgs e)
        {
            speedTex.Text = "0.75";
            vm.VM_speedsend = 0.75;
        }

     
        private void Speedchange_Click_1(object sender, RoutedEventArgs e)
        {
            speedTex.Text = "1.0";
            vm.VM_speedsend = 1;
        }

        private void Speedchange_Click_125(object sender, RoutedEventArgs e)
        {
            speedTex.Text = "1.25";
            vm.VM_speedsend = 1.25;
        }

        private void Speechange_Click_15(object sender, RoutedEventArgs e)
        {
            speedTex.Text = "1.5";
            vm.VM_speedsend = 1.5;
        }

        private void Speedchange_Click_2(object sender, RoutedEventArgs e)
        {
            speedTex.Text = "2";
            vm.VM_speedsend = 2;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

