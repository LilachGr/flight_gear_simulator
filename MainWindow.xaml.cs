using flight_gear_simulator.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ADP2_FLIGHTGEAR;

namespace flight_gear_simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyViewModel vm;
        private MyModel model;
        private DashBoardViewModel vmdash;
        private VMJoystic vmjoy;
        // public event PropertyChangedEventHandler PropertyChanged;
        public MainWindow()
        {
            InitializeComponent();
            this.model = new MyModel(new MyTelnetClient());
            vm = new MyViewModel(model);
            this.vmdash = new DashBoardViewModel(model);
            this.vmjoy = new VMJoystic(model);

            DataContext = vm;
        }

        private void Button_UploadCsv(object sender, RoutedEventArgs e)
        {
            if (vm.VMCsvPath == null || vm.VMxmlpath == null)
            {
                MessageBox.Show("Please enter csv and xml files!");
                return;
            }
            string str = vm.VMCsvPath.Substring(vm.VMCsvPath.Length - 4);
            if (!String.Equals(".csv", str))
            {
                MessageBox.Show("Error!" + "\n" + "choose correct csv file path!");
                return;
            }
            string str2 = vm.VMxmlpath.Substring(vm.VMxmlpath.Length - 4);
            if (!String.Equals(".xml", str2))
            {
                MessageBox.Show("Error!" + "\n" + "choose correct xml file path!");
                return;
            }
            vm.SetValuesXML();
            vm.UploadPath();
            if (vm.VMcorrectCsv()&&vm.VMcorrectXml())
            {
                MessageBox.Show("Uploaded successfuly");
                Connection menu = new Connection(this.vm,this.vmjoy,this.vmdash);

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


                foreach (string csv in fileDialog.FileNames)
                {
                    csvName += ";" + csv;
                }
                csvName = csvName.Substring(1); //delete the ;
                csvpath.Text = csvName;

                vm.VMCsvPath = csvpath.Text;

            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            textxml.Text += ((Button)sender).Content.ToString();
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.DefaultExt = ".xml";
            fileDialog.Filter = "CSV file (*.xml)|*.xml| All Files (*.*)|*.*";
            Nullable<bool> dialogOk = fileDialog.ShowDialog();

            if (dialogOk == true)
            {
                string xmlName = "";


                foreach (string xml in fileDialog.FileNames)
                {
                    xmlName += ";" + xml;
                }
                xmlName = xmlName.Substring(1); //delete the ;
                textxml.Text = xmlName; 
                vm.VMxmlpath = textxml.Text;
            }
        }
    }
}

