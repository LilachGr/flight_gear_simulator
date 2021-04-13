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
        private PlotModel plotCorrelatedGraph;
        private PlotModel plotBothFeaturesGraph;

        public MyViewModel(IModel model)
        {
            this.model = model;
            PlotBasicGraph = new PlotModel();
            plotCorrelatedGraph = new PlotModel();
            plotBothFeaturesGraph = new PlotModel();
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
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

            PlotCorrelatedGraph.ResetAllAxes();
            LineSerieCorrelated.Points.Clear();
            LineSerieCorrelated.ClearSelection();
            PlotCorrelatedGraph.Series.Clear();
            PlotCorrelatedGraph = new PlotModel();

            PlotBothFeaturesGraph.ResetAllAxes();
            LineSerieBothFeatures.Points.Clear();
            LineSerieBothFeatures.ClearSelection();
            PlotBothFeaturesGraph.Series.Clear();
            PlotBothFeaturesGraph = new PlotModel();
        }

        public void SetUpModelBasicGraph()
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

        public void SetUpModelCorrelatedGraph() {
            PlotCorrelatedGraph.LegendTitle = "The Correlated Graph";
            PlotCorrelatedGraph.LegendOrientation = LegendOrientation.Horizontal;
            PlotCorrelatedGraph.LegendPlacement = LegendPlacement.Outside;
            PlotCorrelatedGraph.LegendPosition = LegendPosition.TopCenter;
            PlotCorrelatedGraph.LegendBackground = OxyColors.LightBlue;
            PlotCorrelatedGraph.LegendBorder = OxyColors.LightBlue;
            PlotCorrelatedGraph.Background = OxyColors.LightBlue;

            var dateAxis = new DateTimeAxis() { StringFormat = "HH:mm:ss", MajorGridlineStyle = LineStyle.Solid, Title = "Time", MinorGridlineStyle = LineStyle.Dot, Position = AxisPosition.Bottom };
            PlotCorrelatedGraph.Axes.Add(dateAxis);
            var valueAxisY = new LinearAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = VMGetCorrelatedValue() };
            PlotCorrelatedGraph.Axes.Add(valueAxisY);
            PlotCorrelatedGraph.Series.Add(LineSerieCorrelated);
        }

        public void SetUpModelBothFeaturesGraph()
        {
            PlotBothFeaturesGraph.LegendTitle = "Both Features Graph";
            PlotBothFeaturesGraph.LegendOrientation = LegendOrientation.Horizontal;
            PlotBothFeaturesGraph.LegendPlacement = LegendPlacement.Outside;
            PlotBothFeaturesGraph.LegendPosition = LegendPosition.TopCenter;
            PlotBothFeaturesGraph.LegendBackground = OxyColors.LightBlue;
            PlotBothFeaturesGraph.LegendBorder = OxyColors.LightBlue;
            PlotBothFeaturesGraph.Background = OxyColors.LightBlue;

            var valueAxisX = new LinearAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = ChosenValue, Position = AxisPosition.Bottom };
            PlotBothFeaturesGraph.Axes.Add(valueAxisX);
            var valueAxisY = new LinearAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = VMGetCorrelatedValue() };
            PlotBothFeaturesGraph.Axes.Add(valueAxisY);
            PlotBothFeaturesGraph.Series.Add(LineSerieBothFeatures);
        }

        public void UpdateOldDataBothFeaturesGraph()
        {
            LineSerieBothFeatures.Points.Clear();
            List<List<(DateTime, float)>> allData = model.GetLiveData();
            int sizeAllData = allData.Count;
            if (sizeAllData <= 0)
            {
                return;
            }
            int correlatedIndex = this.model.GetCorrelatedIndex(ChosenValue);
            if (correlatedIndex == -1)
            {
                return;
            }
            for (int i = 0; i < sizeAllData; i++)
            {
                LineSerieBothFeatures.Points.Add(new DataPoint(allData[i][this.ChosenValusIndex].Item2, allData[i][correlatedIndex].Item2));
            }
        }

        public void UpdateOldDataCorrelated()
        {
            LineSerieCorrelated.Points.Clear();
            List<List<(DateTime, float)>> allData = model.GetLiveData();
            int sizeAllData = allData.Count;
            if (sizeAllData <= 0)
            {
                return;
            }
            int correlatedIndex = this.model.GetCorrelatedIndex(ChosenValue);
            if (correlatedIndex == -1)
            {
                return;
            }
            for (int i = 0; i < sizeAllData; i++)
            {
                LineSerieCorrelated.Points.Add(new DataPoint(DateTimeAxis.ToDouble(allData[i][correlatedIndex].Item1), allData[i][correlatedIndex].Item2));
            }
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

        private LineSeries LineSerieBothFeatures = new LineSeries
        {
            StrokeThickness = 2,
            Color = OxyColors.DarkBlue,
            //MarkerStroke = OxyColors.DarkBlue,
            //MarkerFill = OxyColors.DarkBlue,
            //MarkerType = MarkerType.Circle,
            CanTrackerInterpolatePoints = false,
            // Title = string.Format("Detector {0}", data.Key),
        };

        private LineSeries LineSerieCorrelated = new LineSeries
        {
            StrokeThickness = 2,
            Color = OxyColors.DarkBlue,
            //MarkerStroke = OxyColors.DarkBlue,
            //MarkerFill = OxyColors.DarkBlue,
            //MarkerType = MarkerType.Circle,
            CanTrackerInterpolatePoints = false,
            // Title = string.Format("Detector {0}", data.Key),
        };

        private LineSeries LineSerie = new LineSeries
        {
            StrokeThickness = 2,
            Color = OxyColors.DarkBlue,
            CanTrackerInterpolatePoints = false,
        };

        public void UpdateModelBasicGraph()
        {
            (DateTime, float) LastData = model.GetLiveData().Last()[this.ChosenValusIndex];
            LineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(LastData.Item1), LastData.Item2));
            PlotBasicGraph.InvalidatePlot(true);
        }

        public void UpdateModelCorrelatedGraph()
        {
            int index = this.model.GetCorrelatedIndex(ChosenValue);
            if (index != -1)
            {
                (DateTime, float) LastData = model.GetLiveData().Last()[index];
                LineSerieCorrelated.Points.Add(new DataPoint(DateTimeAxis.ToDouble(LastData.Item1), LastData.Item2));
                PlotCorrelatedGraph.InvalidatePlot(true);
            }
        }

        public void UpdateModelBothFeaturesGraph()
        {
            float LastDataX = model.GetLiveData().Last()[this.ChosenValusIndex].Item2;
            int index = this.model.GetCorrelatedIndex(ChosenValue);
            if (index != -1)
            {
                float LastDataY = model.GetLiveData().Last()[index].Item2;
                LineSerieBothFeatures.Points.Add(new DataPoint(LastDataX, LastDataY));
                PlotBothFeaturesGraph.InvalidatePlot(true);
            }
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

        public PlotModel PlotCorrelatedGraph
        {
            get { return plotCorrelatedGraph; }
            set
            {
                plotCorrelatedGraph = value;
                if (model.GetLiveData().Count() > 0)
                {
                    NotifyPropertyChanged("VM_PlotCorrelatedGraph");
                }
            }
        }

        public PlotModel PlotBothFeaturesGraph
        {
            get { return plotBothFeaturesGraph; }
            set
            {
                plotBothFeaturesGraph = value;
                if (model.GetLiveData().Count() > 0)
                {
                    NotifyPropertyChanged("VM_PlotBothFeaturesGraph");
                }
            }
        }

        //property to bind data with the textbox
        public string VMCsvPath { get; set; }

        public string CsvLearnPath { get; set; }

        public float Threshold { get; set; }

        public bool VMIsDllConnected() {
            return model.IsDllConnected();
        }

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

        //set the dll address
        public string DllAdr { get; set; }

        public void VM_DllConnect()
        {
            this.model.DllConnect(DllAdr, CsvLearnPath, Threshold);
        }

        public string VMGetCorrelatedValue()
        {
            return this.model.GetCorrelatedValue(ChosenValue);
        }

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

        public string ChosenValue { get; set; }

        //property to bind if the Button pressed
        public bool ButtonChosenValueGraphPressed { get; set; }

        public void GraphValueButton(string option)
        {
            ChosenValue = option;
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
                    PlotCorrelatedGraph.Axes[1].Title = VMGetCorrelatedValue();
                    PlotBothFeaturesGraph.Axes[0].Title = ChosenValue;
                    PlotBothFeaturesGraph.Axes[1].Title = VMGetCorrelatedValue();
                    ButtonChosenValueGraphPressed = true;
                    //update the index in the vm
                    ChosenValusIndex = valueIndex;
                    //add all the data until now to the graph
                    UpdateOldData();
                    UpdateOldDataCorrelated();
                    UpdateOldDataBothFeaturesGraph();
                }
            }
        }
    }
}
