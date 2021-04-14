INFO ABOUT THE APP
//Talking about the running of the app
firts of all when the application is opend you must load csv file and xml file path
by clicking on (choose csv/xml file) after that become the phase of the connection with
correct Ip and Port(And oppening the FlightGear APP) else the app will handel this situation.
AND know after these stages we know in the FLyWindow.
Know we can click on the StartFlying! Button to start the flying.
in these moment you will be in a Video that recorded thats mean you
have the option to control the flight with button like video control.
you will show the data of the featuers like direction,airspeed,pitch,roll,yaw,throttle,rudder
you will show the movement of the aileron and Elvator on the joystic control.
you will can change the speed of the flight in every section in the flight.

//Talking about the files that we have
first of all in this project(app)  we use the MVVM that we learn.
we have a file of views that contains the usercontrols like the joystic,sliderButtons,slider,and the dashboards.
we have a file of modelViews this is a important part that contains the different viewModels of every part that we used MVVM with like VmJoystic
this part have the throttle and the rudder and the aileron and the elevator what i mean that this is a one frome the viewModels that we did.
in the file of the Mymodel we have every thing that the ViewModel gets from there (MVVM) and the very important thing is the loop of start that
stating in the background using threads and updating the featuers and other thing that must have to do in the Mymodel we have classes to connect to the flightgear app
we use the interface ITelnetClient and the MyTelnetClient implements it and the same with Imodel and Mymodel that implments it, in Imodel we have all the functions and the properties
that we used in MyModel. its very important to say that without the model we didnt have anything.


//Talking about things that the client must have to run the app
we use NETframeworks version 4.
use flightgear 2020 the last version.

//THE Uml is an a pdf file
contains just the classes we have a large numbers of functions and properties we cant put them... .












 
