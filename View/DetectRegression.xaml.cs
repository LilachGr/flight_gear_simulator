using flight_gear_simulator;
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

namespace ADP2_FLIGHTGEAR.View
{
    /// <summary>
    /// Interaction logic for DetectRegression.xaml
    /// </summary>
    public partial class DetectRegression : Window
    {
        MyViewModel vm;
        VMJoystic vmJoy;
        DashBoardViewModel vmDash;
        public DetectRegression(MyViewModel vm, VMJoystic vmJoy, DashBoardViewModel vmDash)
        {
            this.vm = vm;
            this.vmJoy = vmJoy;
            this.vmDash = vmDash;
            DataContext = vm;
            InitializeComponent();
            List<string> anomalies = vm.GetAllAnomalies();
            if(anomalies.Count == 0)
            {
                anomalies.Add("There isn't anomalies in this flight!");
            }
            listAnomalies.ItemsSource = anomalies;
            //CompositionTarget.Rendering += CompositionTargetRendering;
            vm.SetUpModelAnomaliesGraph();
            vm.ButtonChosenAnomalyGraphPressed = false;
        }

     /*   private void CompositionTargetRendering(object sender, EventArgs e)
        {
            if (vm.ButtonChosenAnomalyGraphPressed)
            {
                vm.UpdateModelAnomaliesGraph();
                AnomaliesGraph.InvalidateVisual();
            }
        }*/

    private void Mainwindow_Click(object sender, RoutedEventArgs e)
        {
            //missing-disconnect the graph of anomalies
            vm.DisconnectDetectRegression();
            vm.DisconnectPlotModel();
            FlyWindow main = new FlyWindow(vm, vmJoy, vmDash);
            main.Show();
            this.Close();
        }

        private void choose_anomaly_button(object sender, RoutedEventArgs e)
        {
            int indexOfChosen = listAnomalies.SelectedIndex;
            if (indexOfChosen == -1)
            {
                MessageBox.Show("Please choose an anomaly!");
            }
            vm.AnomalyGraphButton(indexOfChosen);
            if (vm.ButtonChosenAnomalyGraphPressed)
            {
                AnomaliesGraph.InvalidateVisual();
            }
            else
            {
                MessageBox.Show("Please choose an anomaly!");
            }
        }
    }
}
