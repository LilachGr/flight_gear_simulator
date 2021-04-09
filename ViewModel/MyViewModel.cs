using flight_gear_simulator.Model;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flight_gear_simulator.ViewModel
{
   public  class MyViewModel : INotifyPropertyChanged
    {
        private IModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        bool isDisconnected = false;
        private PlotModel plotBasicGraph;

        public MyViewModel(IModel model)
        {
            this.model = model;
            PlotBasicGraph = new PlotModel();
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                /* if (e.PropertyName == "liveData")
                 {
                     if (model.GetLiveData().Count() != 0 && ButtonChosenValueGraphPressed)
                     {
                         //float value = model.GetLiveData().Last()[ChosenValusIndex];
                         UpdateModel();
                        // NotifyPropertyChanged("VM_" + e.PropertyName);
                     }
                 }
                 else
                 {
                     NotifyPropertyChanged("VM_" + e.PropertyName);
                 }*/
                if (e.PropertyName != "liveData")
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                }
            };
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void DisconnectPlotModel()
        {
            plotBasicGraph.ResetAllAxes();
            LineSerie.Points.Clear();
            LineSerie.ClearSelection();
            plotBasicGraph.Series.Clear();
            plotBasicGraph = new PlotModel();
        }

        public void SetUpModel()
        {
            PlotBasicGraph.LegendTitle = "The Basic Graph";
            PlotBasicGraph.LegendOrientation = LegendOrientation.Horizontal;
            PlotBasicGraph.LegendPlacement = LegendPlacement.Outside;
            PlotBasicGraph.LegendPosition = LegendPosition.TopCenter;
            PlotBasicGraph.LegendBackground = OxyColors.LightBlue;
            PlotBasicGraph.LegendBorder = OxyColors.LightBlue;
            PlotBasicGraph.Background = OxyColors.LightBlue;

            var dateAxis = new DateTimeAxis() {StringFormat = "HH:mm:ss", MajorGridlineStyle = LineStyle.Solid, Title = "Time", MinorGridlineStyle = LineStyle.Dot, Position = AxisPosition.Bottom };
            PlotBasicGraph.Axes.Add(dateAxis);
            var valueAxis = new LinearAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            PlotBasicGraph.Axes.Add(valueAxis);
            PlotBasicGraph.Series.Add(LineSerie);
        }

        public void UpdateOldData()
        {
            LineSerie.Points.Clear();
            List<List<(DateTime, float)>> allData = model.GetLiveData();
            int sizeAllData = allData.Count;
            if (sizeAllData <= 0) {
                return;
            }
            for (int i = 0; i < sizeAllData; i++)
            {
                LineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(allData[i][this.ChosenValusIndex].Item1), allData[i][this.ChosenValusIndex].Item2));
            }
        }

        private LineSeries LineSerie = new LineSeries
        {
            StrokeThickness = 2,
            Color = OxyColors.DarkBlue,
            //MarkerStroke = OxyColors.DarkBlue,
            //MarkerFill = OxyColors.DarkBlue,
            //MarkerType = MarkerType.Circle,
            CanTrackerInterpolatePoints = false,
            // Title = string.Format("Detector {0}", data.Key),
        };

        public void UpdateModel()
        {
            (DateTime, float) LastData = model.GetLiveData().Last()[this.ChosenValusIndex];
            LineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(LastData.Item1), LastData.Item2));
            PlotBasicGraph.InvalidatePlot(true);
        }

        public PlotModel PlotBasicGraph
        {
            get { return plotBasicGraph; }
            set
            {
                plotBasicGraph = value;
                if (model.GetLiveData().Count() > 0)
                {
                    NotifyPropertyChanged("VM_PlotBasicGraph");
                }
            }
        }

        //property to bind data with the textbox
        public string VMCsvPath { get; set; }

        //property to bind data with the comboBox
        public ICollection<string> ValuesXML { get; set; }

        public void SetValuesXML()
        {
            model.CreateValuesFromXML();
            ValuesXML = model.GetXmlValue().Keys;
        }

        // check if the path is for csv file
        public bool VMcorrectCsv()
        {
            return model.CorrectCSV;
        }

        //checking if we can connect to the givn Ip and Port
        public bool VMcorrectIP_Port()
        {
            return model.CorrectIp_Port;
        }

        // initialize the csvPath
        public void UploadPath()
        {
            model.CsvpathSet(VMCsvPath);
        }

        //set the ip
        public string VM_Ip { get; set; }
       
        //set the port
        public int VM_Port { get; set; }

        // initialize the model ip
        public void ModelIP()
        {
            model.Ip = VM_Ip;
        }

        // initialize the model port
        public void ModelPort()
        {
            model.Port = VM_Port;
        }

        //to connect to the FlightGear
        public void VM_connect()
        {
            this.model.Connect();
        }

        public void VM_disconnect()
        {
            this.model.Disconnect();
            isDisconnected = true;
        }

        public bool Vm_isDisconnected()
        {
            return this.isDisconnected;
        }

        //before to connect set all of the IP and the Port
        public void VM_BeforeConnection()
        {
            ModelIP();
            ModelPort();
        }
         
        // the csv flying one time without sitting
        public void VM_Start1()
        {
            this.model.Start1();
        }

        //property to the index of the chosen value
        public int ChosenValusIndex { get; set; }

        //property to bind if the Button pressed
        public bool ButtonChosenValueGraphPressed { get; set; }

        public void GraphValueButton(string option)
        {
            int valueIndex = -1;
            if (model.GetXmlValue().Count() > 0)
            {
                if(model.GetXmlValue().ContainsKey(option)){
                    valueIndex = model.GetXmlValue()[option];
                }
            }
            if (valueIndex == -1)
            {
                ChosenValusIndex = valueIndex;
            }
            else
            {
                if (ChosenValusIndex != valueIndex)
                {
                    ButtonChosenValueGraphPressed = true;
                    //update the index in the vm
                    ChosenValusIndex = valueIndex;
                    //add all the data until now to the graph
                    UpdateOldData();
                }
            }
        }
    }
}
