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
        private bool isValueChosen;
        private int time;
        private List<float> temp = new List<float>();
        const double margin = 20;
        private double canGraphHight = 0;

        public DataInvestigation(MyViewModel vm)
        {
            this.vm = vm;
            this.isValueChosen = false;
            this.time = 0;
            InitializeComponent();
            CreateBasicGraph();
            vm.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                if (e.PropertyName.Contains("liveData") && this.isValueChosen)
                {
                    string value = e.PropertyName.Split(',')[1];
                    float valueNum = float.Parse(value);
                    this.time++;
                    AddDotsToGraph(this.time, valueNum); 
                }
            };
        }

        public void CreateBasicGraph()
        {
            double xmin = margin;
            double ymax = canGraph.Height - margin;
            this.canGraphHight = canGraph.Height;
            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(margin - 10, ymax), new Point(canGraph.Width - margin, ymax)));

            // show the x line in the window.
            Path xaxis_path = new Path
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Data = xaxis_geom
            };
            //canGraph.Children.Add(xaxis_path);
            //Add_to_CanGraph(xaxis_path);


            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(new Point(xmin, margin), new Point(xmin, canGraph.Height - margin + 10)));

            // show the y line in the window.
            Path yaxis_path = new Path
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Data = yaxis_geom
            };
            // canGraph.Children.Add(yaxis_path);
            Add_to_CanGraph(xaxis_path, yaxis_path);

        }

        public void AddDotsToGraph(double x, double y)
        {
            const double dotSize = 6;
            y = this.canGraphHight - margin - y;
            GeometryGroup point_geom = new GeometryGroup();
            PointCollection points = new PointCollection();
            point_geom.Children.Add(new EllipseGeometry(new Rect(x, y, dotSize, dotSize)));

            points.Add(new Point(x, y));

            // show the line between points in the window
            Polyline polyline = new Polyline
            {
                StrokeThickness = 1,
                Stroke = Brushes.Red,
                Points = points
            };
            //canGraph.Children.Add(polyline);
            //Add_to_CanGraph(polyline);

            // show the points in the window
            Path point_geom2 = new Path
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Fill = Brushes.Black,
                Data = point_geom
            };
            //canGraph.Children.Add(point_geom2);
            Add_to_CanGraph(polyline, point_geom2);
        }

        private void Change_Value(object sender, RoutedEventArgs e)
        {
            this.isValueChosen = true;
            int size = valueRadioButtons.Children.Count;
            int valueIndex = -1;
            for (int i = 0; i < size; i++) {
                if ((valueRadioButtons.Children[i] as System.Windows.Controls.RadioButton).IsChecked == true){
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
                //add all the data until now to the graph
                Add_Old_Data(valueIndex);

                //update the index in the vm
                vm.ChosenValusIndex = valueIndex;
            }
        }

        //add all the data until now to the graph
        private void Add_Old_Data(int valueIndex)
        {
            int step = 10;
            List<List<float>> data = vm.VM_GetLiveData();
            int size = data.Count();
            for (int i = 0; i < size; i++)
            {
                AddDotsToGraph(step, data[i][valueIndex]);
                step += 10;
            }
           /* foreach (List<float> item in data)
            {
                AddDotsToGraph(step, item[valueIndex]);
                step += 10;
            }*/
            this.time = step / 10;
        }

        private void Add_to_CanGraph(UIElement FirstElement, UIElement SecondElemen)
        {
            if (canGraph.Dispatcher.CheckAccess())
            {
                if (canGraph.Children.Count > canGraph.Width)
                {
                    canGraph.Children.RemoveRange(4, 50);
                }
                canGraph.Children.Add(FirstElement);
                canGraph.Children.Add(SecondElemen);
            }
            else
            {
               /* Application.Current.Dispatcher.Invoke(() => {
                    canGraph.Children.Add(FirstElement);
                    canGraph.Children.Add(SecondElemen);
                });*/
               //canGraph.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {canGraph.Children.Add(element); }));
            }
        }
    }
}
