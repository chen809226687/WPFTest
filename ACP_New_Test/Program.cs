using System;
using System.Runtime.InteropServices;
using System.Reflection;
using static Program;

class Program
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AcpDeviceId
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] b;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AcpDeviceAddr
    {
        public AcpDeviceId netId;
        public ushort port;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AcpSubRequest
    {
        public ushort requestedSampleInterval;  // 采样的周期,单位为ms  
        public ushort requestedMaxSampleCount;  // 最大的采样数目  
        public uint requesedSubId;              // 请求的订阅ID  
        public IntPtr requestdUserCtx;          // 用户的参数, 使用 IntPtr  
    }

    static void Main(string[] args)
    {
        // 请将这些路径更改为您的 DLL 路径  
        string libacp1Path = "cdll\\libacp_win64.dll";
        string libacp2Path = "cdll\\libacp_win32.dll";
        string libacp3Path = "cdll\\liblibacp.dll";

        // 加载第一个 DLL 并调用  
        var aaa = libacp_init(libacp1Path);
        var aaa1 = libacp_makeAcpDeviceAddr(libacp1Path);
        var aaa2 = libacp_createInstance(libacp1Path);
        // 加载第二个 DLL 并调用  
        var bbb = libacp_init(libacp2Path);


        var ccc = libacp_init(libacp3Path);



    }

    // 定义委托类型  
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int LibAcpInitDelegate();
    static int libacp_init(string dllPath)
    {
        // 加载 DLL  
        IntPtr hLibrary = LoadLibrary(dllPath);
    
        try
        {
            // 获取函数指针  
            IntPtr pAddressOfFunctionToCall = GetProcAddress(hLibrary, "libacp_init");
            if (pAddressOfFunctionToCall == IntPtr.Zero)
            {
                Console.WriteLine($"Failed to get function address from: {dllPath}");
                return 3;
            }

            // 创建一个委托来调用该函数  
            var function = Marshal.GetDelegateForFunctionPointer<LibAcpInitDelegate>(pAddressOfFunctionToCall);

            // 调用函数  
            return function();

        }
        finally
        {
            // 卸载 DLL  
            FreeLibrary(hLibrary);
        }
    }

    // 定义委托类型  
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int libacp_makeAcpDeviceAddrdelegate(string szHost, ushort port, ref AcpDeviceAddr pDeviceAddr);

    static AcpDeviceAddr acpDeviceAddr;
    static int libacp_makeAcpDeviceAddr(string dllPath)
    {
        acpDeviceAddr = new AcpDeviceAddr();
        // 加载 DLL  
        IntPtr hLibrary = LoadLibrary(dllPath);

        try
        {
            // 获取函数指针  
            IntPtr pAddressOfFunctionToCall = GetProcAddress(hLibrary, "libacp_makeAcpDeviceAddr");
            if (pAddressOfFunctionToCall == IntPtr.Zero)
            {
                Console.WriteLine($"Failed to get function address from: {dllPath}");
                return 3;
            }

            // 创建一个委托来调用该函数  
            var function = Marshal.GetDelegateForFunctionPointer<libacp_makeAcpDeviceAddrdelegate>(pAddressOfFunctionToCall);

            // 调用函数  
            return function("192.168.110.8.1.1", (ushort)9999, ref acpDeviceAddr);

        }
        finally
        {
            // 卸载 DLL  
            FreeLibrary(hLibrary);
        }
    }


    // 定义委托类型  
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int libacp_createInstancedelegate(ref AcpDeviceAddr DeviceAddr, int int1);

    static IntPtr libacp_createInstance(string dllPath)
    {
        // 加载 DLL  
        IntPtr hLibrary = LoadLibrary(dllPath);

        try
        {
            // 获取函数指针  
            IntPtr pAddressOfFunctionToCall = GetProcAddress(hLibrary, "libacp_createInstance");
            if (pAddressOfFunctionToCall == IntPtr.Zero)
            {
                Console.WriteLine($"Failed to get function address from: {dllPath}");
                return 3;
            }

            // 创建一个委托来调用该函数  
            var function = Marshal.GetDelegateForFunctionPointer<libacp_createInstancedelegate>(pAddressOfFunctionToCall);

            // 调用函数  
            return function(ref acpDeviceAddr,0 );

        }
        finally
        {
            // 卸载 DLL  
            FreeLibrary(hLibrary);
        }
    }





    // P/Invoke 声明  
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FreeLibrary(IntPtr hModule);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
}