
#region ModuleInfo
/************************************************************************
 *      vic.MINg    2021-12-09
 *      C# 和 C++ 直接的相关 转化 函数封装    
 ***********************************************************************/
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System;
using System.IO;

namespace DncKernel
{
    public class ConverHandle
    {
        // 结构体 转 Bytes
        public static byte[] Struct2Bytes(object objStruct)
        {
            int length = Marshal.SizeOf(objStruct);
            IntPtr ptr = Marshal.AllocHGlobal(length);
            try
            {
                Marshal.StructureToPtr(objStruct, ptr, false);
                byte[] bytes = new byte[length];
                Marshal.Copy(ptr, bytes, 0, length);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

        }

        // Bytes 转 结构体
        public static StructType Bytes2Struct<StructType>(byte[] bytesBuffer)
        {
            if(bytesBuffer.Length != Marshal.SizeOf(typeof(StructType)))
            {
                throw new ArgumentException("bytesBuffer is not same as structObject for the length of byte.");
            }
            IntPtr bufferHandler = Marshal.AllocHGlobal(bytesBuffer.Length);
            for(int index = 0; index < bytesBuffer.Length;index++)
            {
                Marshal.WriteByte(bufferHandler, index, bytesBuffer[index]);
            }
            StructType structObject = (StructType)Marshal.PtrToStructure(bufferHandler, typeof(StructType));
            Marshal.FreeHGlobal(bufferHandler);
            return structObject;
        }

        // 结构体数组 转 Bytes
        public static byte[] StructArray2Bytes<StructType>(StructType[] stuctArray)
        {
            if (null == stuctArray || stuctArray.Length == 0)
                return null;
            int nStructSize = Marshal.SizeOf(typeof(StructType));
            byte[] bytes = new byte[nStructSize * stuctArray.Length];
            for(int index = 0; index < stuctArray.Length; index++)
            {
                Struct2Bytes(stuctArray[index]).CopyTo(bytes, index * nStructSize);
            }
            return bytes;
        }

        // Bytes 转 结构体数组
        public static StructType[] Bytes2StructArray<StructType>(byte[] bytesBufffer)
        {
            int nStructSize = Marshal.SizeOf(typeof(StructType));
            int nLength = bytesBufffer.Length / nStructSize;
            if (bytesBufffer.Length % nStructSize != 0 || nLength <= 1)
            {
                throw new ArgumentException("bytesBuffer is not have StructTypes for length.");
            }
            StructType[] structObjects = new StructType[nLength];
            for (int index = 0; index < nLength; index++)
            {
                byte[] bytes = new byte[nStructSize];
                Array.Copy(bytesBufffer, nStructSize * index, bytes, 0, nStructSize);
                structObjects[index] = Bytes2Struct<StructType>(bytes);
            }
            return structObjects;
        }

        public static StructType[] Bytes2StructArray<StructType>(byte[] bytesBufffer, int length)
        {
            int nStructSize = Marshal.SizeOf(typeof(StructType));
            int nLength = length;
            if (bytesBufffer.Length % nStructSize != 0 || nLength <= 1)
            {
                throw new ArgumentException("bytesBuffer is not have StructTypes for length.");
            }
            StructType[] structObjects = new StructType[nLength];
            for (int index = 0; index < nLength; index++)
            {
                byte[] bytes = new byte[nStructSize];
                Array.Copy(bytesBufffer, nStructSize * index, bytes, 0, nStructSize);
                structObjects[index] = Bytes2Struct<StructType>(bytes);
            }
            return structObjects;
        }

        // 结构体 转 IntPtr
        // 由于 转成 IntPtr 后，需要自己手动 释放申请的 IntPtr （Marshal.FreeHGlobal(IntPtr);）
        // 因此这里 传人 引用类型 以提示 释放 IntPtr 目的
        public static void Struct2IntPtr<StructType>(ref IntPtr intPtr,[In] StructType structObject)
        {
            int size = Marshal.SizeOf(structObject);
            byte[] bytes = new byte[size];
            intPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObject, intPtr, true);
            Marshal.Copy(intPtr, bytes, 0, size);
        }

        // IntPtr 转 结构体
        // 传入的 IntPrt 会在函数内部释放 暂时注释掉 
        public static StructType IntPtr2Struct<StructType>(IntPtr intPtr)
        {
            object obj = Marshal.PtrToStructure(intPtr, typeof(StructType));
            //Marshal.FreeHGlobal(intPtr);
            // if (obj is StructType structObject)
            //    return structObject;
           // else
            {
                throw new ArgumentException("IntPtr is not conver Struct int IntPtr2Struct.");
            }
        }

        // 结构体数组 转 IntPtr
        // 同样需要手动释放 IntPtr
        public static void StructArray2IntPtr<StructType>(ref IntPtr intPtr,[In] StructType[] structArray)
        {
            byte[] bytes = StructArray2Bytes<StructType>(structArray);
            if (null == bytes)
            {
                throw new ArgumentException("strucArray conver IntPtr error StructArray2Bytes.");
            }
            intPtr = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, intPtr, bytes.Length);
        }

        // IntPtr 转 结构体数组
        // 由于 IntPtr是 指针，因此需要传入 结构体长度
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
    }
}