using flight_gear_simulator.ViewModel;
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

namespace flight_gear_simulator
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : Window
    {
        MyViewModel vm;
        VMJoystic vmJoy;
        DashBoardViewModel vmDash;
        public Connection(MyViewModel vm, VMJoystic vmJoy, DashBoardViewModel vmDash)
        {
            this.vm = vm;
            this.vmJoy = vmJoy;
            this.vmDash = vmDash;
            InitializeComponent();
           // connect.KeyDown += OnKeyDownHandler;
            DataContext = vm;
        }


        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Button_Click_1(sender, e);
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //initialize the IP and the Port
            vm.VM_BeforeConnection();
            //connect
            vm.VM_connect();
            // if we can connect to the port respectivliy with the Ip then connect
            if (vm.VMcorrectIP_Port())
            {
                MessageBox.Show("connected successfully");
                FlyWindow fly = new FlyWindow(vm,vmJoy,vmDash);
                //fly.DataContext = this;
                fly.Show();
                this.Close();
            }
        }
    
}
}
