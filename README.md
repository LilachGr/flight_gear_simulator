# flight_gear_simulator
### Description of the files in the app

- In this project (app) we use the MVVM method.
Under the folder "Views" we have all the views and windows, as well as user controls like the joystic, sliderButtons, slider, and the dashboards.
Under the folder "ViewModel", we have all the view models connecting between the views and the models. 
Under the folder "Model" we have the MyModel interface (IModel) and implementation, a class that connects to flightgear application, a class that manages the dll connection, and their interfaces.
- In the file Mymodel we have the functions that the ViewModels use. Imodel holds the interface of all the functions and most of the properties, and MyModel implements that interface.
The most important flow is the loop started in method "Start1", that runs in the background using threads. This main loop is responsible for updating the specific features and the data, and causing view changes.
- Mymodel uses the MyTelnetClient class to connect to the flightgear app. We use the interface ITelnetClient that MyTelnetClient implements.
- We also have an IDllFunctions interface that MyDllFunctions implements. This interface exposes all the methods that we can use from the dll. MyDllFunctions implements those methods by loading the dll, calling the methods and "trasnlating" to what our application needs.

### The flow of the app

1. When the application is opened you must load csv file and xml file path
by clicking on the relevant buttons (choose csv/xml file).
2. Next, connect with correct Ip and Port to the FlightGear application (should already be running). In case of wrong Ip or port the app will handle it.
3. Now you get to the main window.
4. Now we can click on the StartFlying Button to start the flight.
5. At this moment, you will see a recorded video, and you have the option to control the flight with buttons like video control. 
You can see the data of the features: direction, airspeed, pitch, roll, yaw, throttle and rudder. 
You can see the movement of the aileron and elvator on the joystic control. You can also change the speed of the flight in every part of the flight.
6. For data investigation, you first need to load a dll using "Dll connection" button, and then you can click the "Show data investigation" button.
7. Now you can choose a feature and click the "Change value" button, then you will see 3 graphs that will continue updating over time.
    - The first graph is for the chosen feature and the time.
    - The second graph is for its correlated feature (if exists) and the time
    - The third graph if for the chosen feature, its correlated feature and their line regression. In the third graph the gray points are all the data until now and the red points are the last 30 points.
8. In this window you can also choose to see the anomalies, for this you will need to press the button "Detect Regression".
You will see a list of all the anomalies and the time they appeared.
You can choose one anomaly and press the button below and you will see a graph that changes according to the chosen dll. 
    - If you enter Regression Dll then you will see a regression line between the chosen features, where all the gray points are the data from the all flight and all the red points are the anomalies. 
    - If you enter Circle Dll then you will see a circle between the chosen features, when all the gray points are the data from the all flight and all the red points are the anomalies.

### Requirements for clients to run the app

- Use .NET framework version 4.
- Use flightgear 2020 - the last version.
- Dll for anomaly detection and correlation of features (stories 7-9), should be loaded from Git project's "plugins" folder: plugins\RegressionBasedAlgorithm.dll or plugins\CircleBasedAlgorithm.dll
- It was compiled for Release x64 - so your main project must run with that.
- The code also uses package OxyPlot.Core.2.0.0 for showing the graphs- it is a NuGet package, you should make sure that you have these Nuget packages: OxyPlot.Core, OxyPlot.Wpf
- In the settings in the flightgear you need to set:
"--generic=socket,in,10,127.0.0.1,6300,tcp,playback_small
--fdm=null". 6300 is the port, the application will handle any correct port.
- You also need to put the playback_small.aml (you can find it in the git project) to the folder that you uploaded the flightgear, for example: "C:\Program Files\FlightGear 2020.3.6\data\Protocol".


### Uml file

The Uml is in the below pdf file, it contains the main classes with the main functionality used in the app.
The link to the url: https://github.com/LilachGr/flight_gear_simulator/blob/main/UML.pdf

### Video of the app

The video is in two parts. The link to the folder that contains these videos:
https://github.com/LilachGr/flight_gear_simulator/tree/main/video
