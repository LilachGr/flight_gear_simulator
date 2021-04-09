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

        public DataInvestigation(MyViewModel vm)
        {
            this.vm = vm;
            DataContext = vm;
            InitializeComponent();
            UpdateValueComboBox();
            CompositionTarget.Rendering += CompositionTargetRendering;
            vm.SetUpModel();
            vm.ButtonChosenValueGraphPressed = false;
        }

        private void UpdateValueComboBox()
        {
            vm.SetValuesXML();
            comboBox.ItemsSource = vm.ValuesXML;

        }
        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            if (vm.ButtonChosenValueGraphPressed)
            {
                vm.UpdateModel();
                BasicGraph.InvalidateVisual();
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
            /*int size = valueRadioButtons.Children.Count;
            int valueIndex = -1;
            for (int i = 0; i < size; i++)
            {
                if ((valueRadioButtons.Children[i] as System.Windows.Controls.RadioButton).IsChecked == true)
                {
                    valueIndex = i;
                    break;
                }
            }
            if (valueIndex == -1)
            {
                MessageBox.Show("Please choose an option!");
            }
            else
            {
                if (vm.ChosenValusIndex != valueIndex)
                {
                    this.isValueChosen = true;
                    //update the index in the vm
                    vm.ChosenValusIndex = valueIndex;
                    //add all the data until now to the graph
                    vm.UpdateOldData();
                }
            }*/
        }

        private void Mainwindow_Click(object sender, RoutedEventArgs e)
        {
            vm.DisconnectPlotModel();
            FlyWindow main = new FlyWindow(vm);
            main.Show();
            this.Close();
        }
    }
}
