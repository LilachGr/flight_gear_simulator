﻿using flight_gear_simulator.Model;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace flight_gear_simulator.ViewModel
{
    public class MyViewModel : INotifyPropertyChanged
    {
        private IModel model;
        bool isDisconnected = true;

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
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
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
            LineRegression.Points.Clear();
            LineRegression.ClearSelection();
            LineSerieBothFeatures.Points.Clear();
            LineSerieBothFeatures.ClearSelection();
            PlotBothFeaturesGraph.Series.Clear();
            PlotBothFeaturesGraph = new PlotModel();
        }

        public void SetUpModelBasicGraph()
        {
            PlotBasicGraph.Background = OxyColors.LightBlue;

            var dateAxis = new DateTimeAxis() {StringFormat = "HH:mm:ss", MajorGridlineStyle = LineStyle.Solid, Title = "Time", MinorGridlineStyle = LineStyle.Dot, Position = AxisPosition.Bottom };
            PlotBasicGraph.Axes.Add(dateAxis);
            var valueAxis = new LinearAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
            PlotBasicGraph.Axes.Add(valueAxis);
            PlotBasicGraph.Series.Add(LineSerie);
        }

        public void SetUpModelCorrelatedGraph() {
            PlotCorrelatedGraph.Background = OxyColors.LightBlue;

            var dateAxis = new DateTimeAxis() { StringFormat = "HH:mm:ss", MajorGridlineStyle = LineStyle.Solid, Title = "Time", MinorGridlineStyle = LineStyle.Dot, Position = AxisPosition.Bottom };
            PlotCorrelatedGraph.Axes.Add(dateAxis);
            var valueAxisY = new LinearAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = VMGetCorrelatedValue() };
            PlotCorrelatedGraph.Axes.Add(valueAxisY);
            PlotCorrelatedGraph.Series.Add(LineSerieCorrelated);
        }

        public void SetUpModelBothFeaturesGraph()
        {
            PlotBothFeaturesGraph.Background = OxyColors.LightBlue;

            var valueAxisX = new LinearAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = ChosenValue, Position = AxisPosition.Bottom };
            PlotBothFeaturesGraph.Axes.Add(valueAxisX);
            var valueAxisY = new LinearAxis() { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = VMGetCorrelatedValue() };
            PlotBothFeaturesGraph.Axes.Add(valueAxisY);
            PlotBothFeaturesGraph.Series.Add(LineRegression);
            PlotBothFeaturesGraph.Series.Add(LineSerieBothFeatures);
            PlotBothFeaturesGraph.Series.Add(Line30LastPoints);
        }

        public void UpdateOldDataBothFeaturesGraph()
        {
            LineSerieBothFeatures.Points.Clear();
           //{ startX, startY, endX, endY}
            float[] linePoints = model.GetRegressionLine(ChosenValue);
            if(linePoints == null)
            {
                return;
            }
            LineRegression.Points.Add(new DataPoint(linePoints[0], linePoints[2]));
            LineRegression.Points.Add(new DataPoint(linePoints[1], linePoints[3]));

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

        public void Update30LastPoints()
        {
            Line30LastPoints.Points.Clear();
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
            if (sizeAllData > 30)
            {
                for (int i = sizeAllData - 30; i < sizeAllData; i++)
                {
                    Line30LastPoints.Points.Add(new DataPoint(allData[i][this.ChosenValusIndex].Item2, allData[i][correlatedIndex].Item2));
                }
            }
            else
            {
                for (int i = 0; i < sizeAllData; i++)
                {
                    Line30LastPoints.Points.Add(new DataPoint(allData[i][this.ChosenValusIndex].Item2, allData[i][correlatedIndex].Item2));
                }
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
            //StrokeThickness = 2,
            Color = OxyColors.DarkBlue,
            LineStyle = LineStyle.None,
            MarkerSize = 2,
            MarkerFill = OxyColors.Gray,
            MarkerType = MarkerType.Circle,
            CanTrackerInterpolatePoints = false,
        };

        private LineSeries Line30LastPoints = new LineSeries
        {
            //StrokeThickness = 2,
            //Color = OxyColors.DarkBlue,
            LineStyle = LineStyle.None,
            MarkerSize = 2,
            MarkerFill = OxyColors.Red,
            MarkerType = MarkerType.Circle,
            CanTrackerInterpolatePoints = false,
        };

        private LineSeries LineRegression = new LineSeries
        {
            StrokeThickness = 2,
            Color = OxyColors.DarkBlue,
            CanTrackerInterpolatePoints = false,
        };

        private LineSeries LineSerieCorrelated = new LineSeries
        {
            StrokeThickness = 2,
            Color = OxyColors.DarkBlue,
            CanTrackerInterpolatePoints = false,
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
                Update30LastPoints();
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
        public string VMxmlpath { get; set; }
        public string CsvLearnPath { get; set; }

        public float Threshold { get; set; }

        public bool VMIsDllConnected() {
            return model.IsDllConnected();
        }

        //property to bind data with the comboBox
        public ICollection<string> ValuesXML { get; set; }

        public void SetValuesXML()
        {
            model.CreateValuesFromXML(VMxmlpath);
            ValuesXML = model.GetXmlValue().Keys;
        }

        // check if the path is for csv file
        public bool VMcorrectCsv()
        {
            return model.CorrectCSV;
        }
        public bool VMcorrectXml()
        {
            return model.CorrectXml;
        }
        //checking if we can connect to the givn Ip and Port
        public bool VMcorrectIP_Port()
        {
            return model.CorrectIp_Port;
        }
        // initialize the csvPath
        public void UploadPath()
        {
            model.pathsSet(VMCsvPath,VMxmlpath);
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
        //index of the place of the airplain from csv file
        public int VM_SetIndex
        {
            get { return this.model.SetIndex; }
            set
            {
                this.model.SetIndex = value;


            }
        }
        public string VMGetCorrelatedValue()
        {
            return this.model.GetCorrelatedValue(ChosenValue);
        }
        public string VM_Setindxo { get { return this.model.Setindxo; } }
        //setting the time
        public string VM_Time { get { return model.Time; } }

        public int VM_csvSize
        {
            get
            {
                return this.model.csvSize;
            }
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
            isDisconnected = false;
        }
        public void VM_disconnect()
        {
            this.model.Disconnect();
            isDisconnected = true;
        }
        public void stopIT()
        {
            this.model.IsStopedLoop = true;
        }
        public void startIT()
        {
            this.model.IsStopedLoop = false;
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
        public void VM_SetIndexToBack()
        {
            this.model.SetIndexToBack();
        }
        public void VM_SetIndextoFront()
        {
            this.model.SetIndextoFront();
        }
        public void VM_Pause()
        {
            this.model.Pause();

        }
        //play the video
        public void VM_Play()
        {

            this.model.Play();

        }

        public Thread myThread()
        {
           return this.model.myThread;
        }

        /// <summary>
        /// CHANGING IN THREAD SLEEP
        /// </summary>
        public double VM_speedsend
        {
            get { return model.Speedsend; }
            set
            {
                model.changeSpeed(value);
            }
        }
    }
}
