# flight_gear_simulator
INFO ABOUT THE APP
//Talking about the running of the app
First of all when the application is opened you must load csv file and xml file path
by clicking on (choose csv/xml file) after that comes the phase of the connection with
correct Ip and Port to open the FlightGear( else the app will handle this situation).
After these stages we are now in the FlyWindow.
Now we can click on the StartFlying! Button to start the flying.
At these moment you will be in a video that recorded that means you
have the option to control the flight with buttons like video control.
You will show the data of the featuers like direction,airspeed,pitch,roll,yaw,throttle,rudder
you will show the movement of the aileron and Elvator on the joystic control.
You can change the speed of the flight in every part of the flight.
For data investigation, you first need to load a dll using dll load button, and then you can click the show data investigation button.
After you choose a feature and click the button you will see 3 graphs that will continue updating over time.

//Talking about the files that we have
First of all in this project(app)  we use the MVVM that we learn.
we have a file of views that contains the usercontrols like the joystic,sliderButtons,slider,and the dashboards.
we have a file of modelViews this is a important part that contains the different viewModels of every part that we used MVVM with like VmJoystic
this part have the throttle and the rudder and the aileron and the elevator what i mean that this is a one frome the viewModels that we did.
in the file of the Mymodel we have every thing that the ViewModel gets from there (MVVM) and the very important thing is the loop of start that
stating in the background using threads and updating the featuers and other thing that must have to do in the Mymodel we have classes to connect to the flightgear app
we use the interface ITelnetClient and the MyTelnetClient implements it and the same with Imodel and Mymodel that implments it, in Imodel we have all the functions and the properties
that we used in MyModel. its very important to say that without the model we didnt have anything.


//Talking about things that the client must have to run the app
we use .NET framework version 4.
use flightgear 2020 the last version.
Dll for anomaly detection and correlation of features(stories 7-9), should be loaded from Git project's "plugins" folder: plugins\RegressionBasedAlgorithm.dll
It was compied for Debug x64 runs - so your main project must run with that.
The code also uses package OxyPlot.Core.2.0.0 for showing the graphs- it is a NuGet package, you should install these Nuget packages: OxyPlot.Core, OxyPlot.Wpf

//THE Uml is an a pdf file
contains just the classes we have a large numbers of functions and properties we cant put them... .












 
