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
        StartCoroutine(generate_cube(structArray));
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct point_from_nds
    {
        [MarshalAs(UnmanagedType.R4)]
        public System.Single x;
        [MarshalAs(UnmanagedType.R4)]
        public System.Single y;
    };


    IEnumerator generate_cube(point_from_nds[] array)
    {
        foreach (var item in array)
        {
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(item.x, 0, item.y);

            var scale = 0.0005f / 5;
            cube1.transform.localScale = new Vector3(scale, scale / 2, scale);
            //cube1.transform.SetParent(this.transform);

            yield return null;
        }

    }

    static point_from_nds[] structArray;
    [AOT.MonoPInvokeCallback(typeof(RenderDataCallbackFunc))]
    public static void onCallBack(IntPtr intPtr, int n)
    {
        Debug.Log("come into callback point cnt= " + n.ToString());

        unchecked
        {
            if (n == (int)(0xffffffff))
            {
                process_mesh_data(intPtr);
                return;
            }
        }


        var size = Marshal.SizeOf(typeof(point_from_nds));
        structArray = new point_from_nds[n];

        for (int index = 0; index < n; index++)
        {
            IntPtr ptr = new IntPtr(intPtr.ToInt64() + index * size);
            structArray[index] = (point_from_nds)Marshal.PtrToStructure(ptr, typeof(point_from_nds));
        }

        // ����mx_dll�з�װ���߼��ǲ��ͷţ������ڴ��ͷ�����
        //Marshal.FreeHGlobal(intPtr);
        // structArray ��洢�˵�      
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct mx_vector3
    {
        [MarshalAs(UnmanagedType.R4)]
        public System.Single x;
        [MarshalAs(UnmanagedType.R4)]
        public System.Single y;

        [MarshalAs(UnmanagedType.R4)]
        public System.Single z;
    };

#if false
    [StructLayout(LayoutKind.Sequential)]
    struct mesh
    {
        unsafe Vector3_aa* vertices;
        int vertices_cnt;
        
        unsafe int* indexes;
        int index_cnt;
    };

    [StructLayout(LayoutKind.Sequential)]
    struct background
    {        
        unsafe mesh* green;
        int gnreen_cnt;
                
        unsafe mesh* water;
        int water_cnt;
    };

#else
    [StructLayout(LayoutKind.Sequential)]
    struct mesh
    {
        //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
        public IntPtr vertices;
        public int vertices_cnt;
        //[MarshalAs(UnmanagedType.LPArray)]
        public IntPtr indexes;
        public int index_cnt;
    };

    [StructLayout(LayoutKind.Sequential)]
    struct background
    {

        public IntPtr green;
        public int gnreen_cnt;


        public IntPtr water;
        public int water_cnt;
    };

#endif
    [StructLayout(LayoutKind.Sequential)]
    struct tile
    {
       public point_from_nds coor;
       public background a;
       public background b;
    };


    static void process_mesh_data(IntPtr intPtr)
    {
        object obj = Marshal.PtrToStructure(intPtr, typeof(tile));
        tile tile = (tile)obj;

        mesh[] agreen = IntPtr2StructArray<mesh>(tile.a.green, tile.a.gnreen_cnt);
        mx_vector3[] agreen_vec = IntPtr2StructArray<mx_vector3>(agreen[0].vertices, agreen[0].vertices_cnt);
    }


    public static StructType[] IntPtr2StructArray<StructType>(IntPtr intPtr, int length)
    {
        var size = Marshal.SizeOf(typeof(StructType));
        StructType[] structArray = new StructType[length];

        for (int index = 0; index < length; index++)
        {
            IntPtr ptr = new IntPtr(intPtr.ToInt64() + index * size);
            structArray[index] = (StructType)Marshal.PtrToStructure(ptr, typeof(StructType));
        }
        //Marshal.FreeHGlobal(intPtr);
        return structArray;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
