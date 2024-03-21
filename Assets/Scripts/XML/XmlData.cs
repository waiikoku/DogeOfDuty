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
            FileStream stream = CreateStream(path, FileMode.Create); //���ҧ�����¹���������������˹�
            serializer.Serialize(stream, data); //�ŧ������ T ���������¹�����Xml
            stream.Close();//�Դ�����¹
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
            FileStream stream = CreateStream(path, FileMode.Open); //���ҧ�����ҹ���������������˹�
            T data = serializer.Deserialize(stream) as T; //���ҧ����ŧ������Xml��ٻẺ������ T
            stream.Close(); //�ŧ������Xml�Ѻ���繢����� T
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
