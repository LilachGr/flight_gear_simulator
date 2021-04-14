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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace flight_gear_simulator.View
{
    /// <summary>
    /// Interaction logic for Joystic.xaml
    /// </summary>
    public partial class Joystic : UserControl
    {
        // private bool mouseClick;
        //private double xPosition;
        //private double yPosition
        private Storyboard myBoard;
        private Point point = new Point();
        private VMJoystic VMjoy;
        public Joystic()
        {
            InitializeComponent();
            myBoard = (Storyboard)Knob.FindResource("CenterKnob");
            myBoard.Stop();
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

        public static readonly DependencyProperty rudderProperty = DependencyProperty.Register("Rudder", typeof(double), typeof(Joystic));

        public double Rudder
        {
            get { return (double)GetValue(rudderProperty); }
            set { SetValue(rudderProperty, value); }
        }

        public static readonly DependencyProperty elevatorProperty = DependencyProperty.Register("Elevator", typeof(double), typeof(Joystic));

        public double Elevator
        {
            get { return (double)GetValue(elevatorProperty); }
            set { SetValue(elevatorProperty, value); }
        }

        private void centerKnob_Completed(object sender, EventArgs e)
        {
            myBoard.Stop();
            knobPosition.X = 0;
            knobPosition.Y = 0;
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                double tempX = e.GetPosition(this).X - point.X;
                double tempY = e.GetPosition(this).Y - point.Y;
                double distanceKnob = Math.Sqrt(Math.Pow(tempX, 2) + Math.Pow(tempY, 2));

                if (distanceKnob <= (Base.Width / 2))
                {
                    // knobPosition.X = tempX;
                    //   knobPosition.Y = tempY;
                    knobPosition.X = tempX;
                    knobPosition.Y = tempY;
                }
                else
                {
                    //use atan function to get angle of tangent.
                    double tangentAngle = Math.Atan(tempY / tempX);

                    if (tempX >= 0)
                    {
                        knobPosition.X = Math.Cos(tangentAngle) * (Base.Width / 2);
                        knobPosition.Y = Math.Sin(tangentAngle) * (Base.Width / 2);
                    }

                }
                Rudder = knobPosition.X / (Base.Width / 2);
                Elevator = knobPosition.Y / -(Base.Width / 2);
            }
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            myBoard.Stop();
            if (e.ChangedButton == MouseButton.Left)
            {
                point = e.GetPosition(this);
                Knob.CaptureMouse();
                //mouseClick = true;
            }
        }

        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            knobPosition.Y = 0;
            knobPosition.X = 0;
            Rudder = 0;
            Elevator = 0;
            //mouseClick = false;
            Knob.ReleaseMouseCapture();
            myBoard.Begin();
        }


    }
}

