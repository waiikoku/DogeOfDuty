using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public abstract class XmlData<T> where T : class
{
    public static void Save(string path, T data, Action<bool> callback = null)
    {
        try
        {
            XmlSerializer serializer = CreateSerializer();
            FileStream stream = CreateStream(path, FileMode.Create); //สร้างตัวเขียนไฟล์ตามที่อยู่ที่กำหนด
            serializer.Serialize(stream, data); //แปลงข้อมูล T ที่ส่งมาเขียนเป็นไฟล์Xml
            stream.Close();//ปิดการเขียน
            callback?.Invoke(true);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            callback?.Invoke(false);
            throw;
        }

    }

    public static T Load(string path, Action<bool> callback = null)
    {
        if(File.Exists(path) == false)
        {
            return null;
        }
        try
        {
            XmlSerializer serializer = CreateSerializer();
            FileStream stream = CreateStream(path, FileMode.Open); //สร้างตัวอ่านไฟล์ตามที่อยู่ที่กำหนด
            T data = serializer.Deserialize(stream) as T; //สร้างตัวแปลงข้อมูลXmlในรูปแบบข้อมูล T
            stream.Close(); //แปลงข้อมูลXmlกับมาเป็นข้อมูล T
            callback?.Invoke(true);
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            callback?.Invoke(false);
            return null;
            throw;
        }
    }

    private static XmlSerializer CreateSerializer()
    {
        return new XmlSerializer(typeof(T));
    }

    private static FileStream CreateStream(string fileName, FileMode mode)
    {
        return new FileStream(fileName, mode);
    }
}
