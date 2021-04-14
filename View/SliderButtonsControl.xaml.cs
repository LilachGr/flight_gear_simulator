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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace flight_gear_simulator.View
{
    /// <summary>
    /// Interaction logic for SliderButtonsControl.xaml
    /// </summary>
    public partial class SliderButtonsControl : UserControl
    {
        MyViewModel vm;
        public SliderButtonsControl()
        {
            this.DataContext = vm;
            InitializeComponent();
        }
        public MyViewModel VM_ViewModel
        {
            get
            {
                return this.vm;
            }
            set
            {
                this.vm = value;
                this.DataContext = value;
            }
        }

        private void stepBack_Click(object sender, RoutedEventArgs e)
        {
            vm?.VM_SetIndexToBack();
        }

        private void play(object sender, RoutedEventArgs e)
        {
            if (vm == null)
            {
                MessageBox.Show("You need to start the flight before using this option!");
                return;
            }
            vm.VM_Play();

        }

        private void pause(object sender, RoutedEventArgs e)
        {
            vm?.VM_Pause();
        }

        private void stepOver(object sender, RoutedEventArgs e)
        {
            vm?.VM_SetIndextoFront();

        }


    }
}

