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
using System.Windows.Shapes;

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
            InitializeComponent();
            CreateBasicGraph();
        }

        public void CreateBasicGraph()
        {
            const double margin = 20;
            double xmin = margin;
            double xmax = canGraph.Width - margin;
            double ymax = canGraph.Height - margin;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(margin - 10, ymax), new Point(canGraph.Width - margin, ymax)));

            // show the x line in the window.
            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;
            canGraph.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(new Point(xmin, margin), new Point(xmin, canGraph.Height - margin + 10)));

            // show the y line in the window.
            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;
            canGraph.Children.Add(yaxis_path);
        }

        public void AddDotsToGraph()
        {
            const double dotSize = 6;
            GeometryGroup point_geom = new GeometryGroup();
            PointCollection points = new PointCollection();
            double x = 100;
            double y = 120;
            point_geom.Children.Add(new EllipseGeometry(new Rect(x, y, dotSize, dotSize)));

            points.Add(new Point(x, y));

            // show the line between points in the window
            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 1;
            polyline.Stroke = Brushes.Red;
            polyline.Points = points;
            canGraph.Children.Add(polyline);

            // show the points in the window
            Path point_geom2 = new Path();
            point_geom2.StrokeThickness = 1;
            point_geom2.Stroke = Brushes.Black;
            point_geom2.Fill = Brushes.Black;
            point_geom2.Data = point_geom;
            canGraph.Children.Add(point_geom2);
        }

        private void Change_Value(object sender, RoutedEventArgs e)
        {
            int val = valueRadioButtons.Children.Count;
            Console.WriteLine(val);
        }
    }
}
