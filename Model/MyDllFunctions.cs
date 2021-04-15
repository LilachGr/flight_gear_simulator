using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ADP2_FLIGHTGEAR.Model
{
    class MyDllFunctions : IDllFunctions
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        //get csv file (with a title) and a thresholdOfCorrelative and return a class that contain the most correlated feature for each feature. 
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SetAllCorrelatedFeature([MarshalAs(UnmanagedType.LPStr)] string csvFile, float threshold);

        //get MyCorrelatedFeature class and a feature and return the index of his most correlated feature. 
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetCorrelatedFeature(IntPtr myCF, [MarshalAs(UnmanagedType.LPStr)] string feature);

        //get MyCorrelatedFeature class and a feature and return double array like that: {startX, startY, endX, endY}.
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr GetRegressionLine(IntPtr myCF, [MarshalAs(UnmanagedType.LPStr)] string feature);

        private IntPtr pDll;
        private bool isSetAllCorrelatedFeature = false;
        private bool isConnected = false;

        public MyDllFunctions(string dllAdr)
        {
            if (dllAdr == null)
            {
                isConnected = false;
                return;
            }
            this.pDll = LoadLibrary(dllAdr);
            if (pDll != IntPtr.Zero)
            {
                isConnected = true;
            }
        }

        public bool IsDllConnected() { return isConnected; }

        //get csv file (with a title) and a thresholdOfCorrelative and return a class that contain the most correlated feature for each feature.
        //return IntPtr.Zero when error happen.
        public IntPtr Dll_SetAllCorrelatedFeature(string csvFile, float threshold)
        {
            if(csvFile == null)
            {
                isSetAllCorrelatedFeature = false;
                return IntPtr.Zero;
            }
            isSetAllCorrelatedFeature = true;
            if (pDll == IntPtr.Zero)
            {
                isSetAllCorrelatedFeature = false;
                return IntPtr.Zero;
            }
            IntPtr pAddressOfFunctionToCall = GetProcAddress(this.pDll, "SetAllCorrelatedFeature");
            if(pAddressOfFunctionToCall == IntPtr.Zero)
            {
                isSetAllCorrelatedFeature = false;
                return IntPtr.Zero;
            }
            SetAllCorrelatedFeature setAllCorrelatedFeature = (SetAllCorrelatedFeature)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(SetAllCorrelatedFeature));
            IntPtr ans = IntPtr.Zero;
            try
            {
                ans = setAllCorrelatedFeature(csvFile, threshold);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return ans;
        }

        //get MyCorrelatedFeature class and a feature and return the index of his most correlated feature. 
        //return -1 when error happen.
        public int Dll_GetCorrelatedFeature(IntPtr myCF, string feature)
        {
            if (feature == null)
            {
                isSetAllCorrelatedFeature = false;
                return -1;
            }
            /*if (!isSetAllCorrelatedFeature) {
                return -1;
            }*/
            IntPtr pAddressOfFunctionToCall = GetProcAddress(this.pDll, "GetCorrelatedFeature");
            if (pAddressOfFunctionToCall == IntPtr.Zero)
            {
                return -1;
            }
            GetCorrelatedFeature getCorrelatedFeature = (GetCorrelatedFeature)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(GetCorrelatedFeature));
            return getCorrelatedFeature(myCF, feature);
        }

        //get MyCorrelatedFeature class and a feature and return double array like that: {startX, startY, endX, endY}. 
        //return null when error happen.
        public float[] Dll_GetRegressionLine(IntPtr myCF, string feature)
        {
            if (feature == null)
            {
                isSetAllCorrelatedFeature = false;
                return null;
            }
            /*if (!isSetAllCorrelatedFeature)
            {
                return null;
            }*/
            IntPtr pAddressOfFunctionToCall = GetProcAddress(this.pDll, "GetRegressionLine");
            if (pAddressOfFunctionToCall == IntPtr.Zero)
            {
                return null;
            }
            GetRegressionLine getRegressionLine = (GetRegressionLine)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(GetRegressionLine));
            IntPtr pArray = getRegressionLine(myCF, feature);
            if (pArray == IntPtr.Zero)
            {
                return null;
            }
            float[] result = new float[4];
            Marshal.Copy(pArray, result, 0, 4);
            return result;
        }

        public void DllDisconnect()
        {
            if (isConnected)
            {
                FreeLibrary(this.pDll);
            }
        }
    }
}

