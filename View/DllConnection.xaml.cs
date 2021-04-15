using flight_gear_simulator;
using flight_gear_simulator.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ADP2_FLIGHTGEAR.View
{
    /// <summary>
    /// Interaction logic for DllConnection.xaml
    /// </summary>
    public partial class DllConnection : Window
    {
        MyViewModel vm;
        VMJoystic vmJoy;
        DashBoardViewModel vmDash;
        public DllConnection(MyViewModel vm, VMJoystic vmJoy, DashBoardViewModel vmDash)
        {

            this.vm = vm;
            this.vmJoy = vmJoy;
            this.vmDash = vmDash;
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_Connect(object sender, RoutedEventArgs e)
        {
            vm.VM_DllConnect();
            if (vm.VMIsDllConnected())
            {
                MessageBox.Show("connected successfully");
                FlyWindow fly = new FlyWindow(vm, vmJoy, vmDash);
                //fly.DataContext = this;
                fly.Show();
                this.Close();
            } else
            {
                MessageBox.Show("connection failed, try again!");
            }
        }

        private void Button_ChooseDllFile(object sender, RoutedEventArgs e)
        {
            dllAdr.Text += ((Button)sender).Content.ToString();
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.DefaultExt = ".csv";
            fileDialog.Filter = "Dll file (*.dll)|*.dll| All Files (*.*)|*.*";
            Nullable<bool> dialogOk = fileDialog.ShowDialog();

            if (dialogOk == true)
            {
                string adr = "";
                foreach (string csv in fileDialog.FileNames)
                {
                    adr += ";" + csv;
                }
                adr = adr.Substring(1); //delete the ;
                dllAdr.Text = adr;
                vm.DllAdr = dllAdr.Text;
            }
        }

        private void Button_ChooseCsvFile(object sender, RoutedEventArgs e)
        {
            csvFile.Text += ((Button)sender).Content.ToString();
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.DefaultExt = ".csv";
            fileDialog.Filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            Nullable<bool> dialogOk = fileDialog.ShowDialog();

            if (dialogOk == true)
            {
                string adr = "";
                foreach (string csv in fileDialog.FileNames)
                {
                    adr += ";" + csv;
                }
                adr = adr.Substring(1); //delete the ;
                csvFile.Text = adr;
                vm.CsvLearnPath = csvFile.Text;
            }
        }
    }
}
