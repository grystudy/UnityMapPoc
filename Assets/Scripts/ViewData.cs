using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ViewData : MonoBehaviour
{
    [DllImport("mx_dll")]
    extern static int set_data(string data);

    public delegate void RenderDataCallbackFunc(IntPtr touchEvent, int n);

    [DllImport("mx_dll")]
    extern static void set_renderdata_callback(RenderDataCallbackFunc callback);

    // Start is called before the first frame update
    void Start()
    {
        // 设置回调
        set_renderdata_callback(onCallBack);

        // 设置数据路径，数据路径下要有nds_ds.dll ，之后便会进入回调 ，用于测试c++返回的数组
        var res = set_data("F://yy//unity (2)//unity//Data");
        Debug.Log("init result = " + res.ToString());

        // 
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct point_from_nds
    {
        [MarshalAs(UnmanagedType.R4)]
        public System.Single x;
        [MarshalAs(UnmanagedType.R4)]
        public System.Single y;
    };


    [AOT.MonoPInvokeCallback(typeof(RenderDataCallbackFunc))]
    public static void onCallBack(IntPtr intPtr, int n)
    {
        Debug.Log("come into callback point cnt= " + n.ToString());

        var size = Marshal.SizeOf(typeof(point_from_nds));
        point_from_nds[] structArray = new point_from_nds[n];

        for (int index = 0; index < n; index++)
        {
            IntPtr ptr = new IntPtr(intPtr.ToInt64() + index * size);
            structArray[index] = (point_from_nds)Marshal.PtrToStructure(ptr, typeof(point_from_nds));
        }

        // 由于mx_dll中封装的逻辑是不释放，所以在此释放数组
        Marshal.FreeHGlobal(intPtr);
        // structArray 里存储了点       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
