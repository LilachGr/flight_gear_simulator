using flight_gear_simulator;
using flight_gear_simulator.Model;
using flight_gear_simulator.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ADP2_FLIGHTGEAR.View
{
    /// <summary>
    /// Interaction logic for DataInvestigation.xaml
    /// </summary>
    public partial class DataInvestigation : Window
    {
        MyViewModel vm;
        VMJoystic vmJoy;
        DashBoardViewModel vmDash;
        FlyWindow flyStart;
        public DataInvestigation(MyViewModel vm, VMJoystic vmJoy, DashBoardViewModel vmDash, FlyWindow FlyStart)
        {
            this.flyStart = FlyStart;
            this.vm = vm;
            this.vmJoy = vmJoy;
            this.vmDash = vmDash;
            DataContext = vm;
            InitializeComponent();
            UpdateValueComboBox();
            CompositionTarget.Rendering += CompositionTargetRendering;
            vm.SetUpModelBasicGraph();
            vm.SetUpModelCorrelatedGraph();
            vm.SetUpModelBothFeaturesGraph();
            vm.ButtonChosenValueGraphPressed = false;
        }

        private void UpdateValueComboBox()
        {
            comboBox.ItemsSource = vm.ValuesXML;
        }
        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            if (vm.ButtonChosenValueGraphPressed)
            {
                vm.UpdateModelBasicGraph();
                vm.UpdateModelCorrelatedGraph();
                vm.UpdateModelBothFeaturesGraph();
                BasicGraph.InvalidateVisual();
                CorrelatedGraph.InvalidateVisual();
                BothFeaturesGraph.InvalidateVisual();
            }
        }

        private void Change_Value(object sender, RoutedEventArgs e)
        {
            string option = comboBox.Text;
            vm.GraphValueButton(option);
            if (vm.ChosenValusIndex == -1)
            {
                MessageBox.Show("Please choose an option!");
            }
        }

        private void Mainwindow_Click(object sender, RoutedEventArgs e)
        {
            vm.DisconnectPlotModel();
            //FlyWindow main = new FlyWindow(vm, vmJoy, vmDash);
            //main.Show();
            this.Close();
        }

        private void DetectRegression_Click(object sender, RoutedEventArgs e)
        {
            DetectRegression investigation = new DetectRegression(vm, vmJoy, vmDash, flyStart);
            investigation.Show();
            this.Close();
        }
    }
}
