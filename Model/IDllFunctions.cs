using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADP2_FLIGHTGEAR.Model
{
    interface IDllFunctions
    {
        //get csv file (with a title) and a thresholdOfCorrelative and return a class that contain the most correlated feature for each feature. 
        //return IntPtr.Zero when error happen.
        IntPtr Dll_SetAllCorrelatedFeature(string csvFile, float threshold);

        //get MyCorrelatedFeature class and a feature and return the index of his most correlated feature. 
        //return -1 when error happen.
        int Dll_GetCorrelatedFeature(IntPtr myCF, string feature);

        //get MyCorrelatedFeature class and a feature and return double array like that: {startX, startY, endX, endY}. 
        //return null when error happen.
        float[] Dll_GetRegressionLine(IntPtr myCF, string feature);

        //get MyCorrelatedFeature class and add all the anomalies in the csvFileAnomaly to the file placeForAns. return 0 if failed otherwise return 1.
        int Dll_GetAnomalies(IntPtr myCF, string csvFileAnomaly, string placeForAns);

        //get MyCorrelatedFeature class and a feature and return float array like that: {centerX, centerY, radius}.
        //return null when error happen.
        float[] Dll_GetRegressionCircle(IntPtr myCF, string feature);

        //return 0 if it is regerssion algorithem otherwise if it is circle algorhitem return 1.
        //return -1 when error happen.
        int Dll_GetTypeDll();

        //disconnect from the dll
        void DllDisconnect();

        //return if the dll is connected
        bool IsDllConnected();
    }
}
