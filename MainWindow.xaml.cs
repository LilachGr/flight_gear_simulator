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
using ADP2_FLIGHTGEAR;
using flight_gear_simulator.Model;
using flight_gear_simulator.ViewModel;
using Microsoft.Win32;
using ADP2_FLIGHTGEAR;

namespace flight_gear_simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      public MyViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new MyViewModel(new MyModel(new MyTelnetClient()));
            DataContext = vm;
        }

        private void Button_UploadCsv(object sender, RoutedEventArgs e)
        {
             vm.UploadPath();
            if (vm.VMcorrectCsv())
            {
                MessageBox.Show("csv uploaded successfuly");
                Connection menu = new Connection(this.vm);
                menu.DataContext = vm;
                menu.Show();
                this.Close();
            }
        }
    
        //button to get the csv path by opening window to search usning fileDialog
        private void Button_ChooseFile(object sender, RoutedEventArgs e)
        {
            csvpath.Text += ((Button)sender).Content.ToString();
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.DefaultExt = ".csv";
            fileDialog.Filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            Nullable<bool> dialogOk = fileDialog.ShowDialog();

            if (dialogOk == true)
            {
                string csvName = "";
                foreach(string csv  in fileDialog.FileNames)
                {
                    csvName += ";" + csv;
                }
                csvName = csvName.Substring(1); //delete the ;
                csvpath.Text = csvName;

                vm.VMCsvPath = csvpath.Text;
            }
        }
    }
}