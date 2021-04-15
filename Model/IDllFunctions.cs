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

        //disconnect from the dll
        void DllDisconnect();

        //return if the dll is connected
        bool IsDllConnected();
    }
}
