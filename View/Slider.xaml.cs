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
    /// Interaction logic for Slider.xaml
    /// </summary>
    public partial class Slider : UserControl
    {
        MyViewModel vm;
        public Slider()
        {
            InitializeComponent();
        }
        public MyViewModel VM_sliderViewModel
        {
            get
            {
                return vm;
            }
            set
            {
                this.vm = value;
                this.DataContext = value;
                // prog1.DataContext = value;
                // prog2.DataContext = value;
            }


        }



    }
}
