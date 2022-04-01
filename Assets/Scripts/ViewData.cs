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
        // ���ûص�
        set_renderdata_callback(onCallBack);

        // ��������·��������·����Ҫ��nds_ds.dll ��֮�������ص� �����ڲ���c++���ص�����
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

        // ����mx_dll�з�װ���߼��ǲ��ͷţ������ڴ��ͷ�����
        Marshal.FreeHGlobal(intPtr);
        // structArray ��洢�˵�       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
