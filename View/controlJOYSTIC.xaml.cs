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
    /// Interaction logic for controlJOYSTIC.xaml
    /// </summary>
    public partial class controlJOYSTIC : UserControl
    {
        VMJoystic VMjoy;
        public controlJOYSTIC()
        {
            InitializeComponent();
        }
        public VMJoystic VM_JOYViewModel
        {
            get
            {
                return VMjoy;
            }
            set
            {
                this.VMjoy = value;
                this.DataContext = value;
                // prog1.DataContext = value;
                // prog2.DataContext = value;
            }


        }
    }
}
