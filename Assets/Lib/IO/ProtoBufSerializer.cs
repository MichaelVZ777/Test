using UnityEngine;
using System.Collections;
using ProtoBuf;
using System.IO;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;

public static class ProtoBufSerializer
{
    static MetaType partModuleMeta;
    static MetaType shipCommandMeta;
    static MetaType stationCommandMeta;
    static int partModuleFieldNumber = 1;
    static int unitCommandFieldNumber = 1;

    static Dictionary<Type, int> fieldNumbers;

    public static void Setup()
    {
        //add unity type to the Protobuf model
        if (!RuntimeTypeModel.Default.IsDefined(typeof(Vector2)))
            RuntimeTypeModel.Default.Add(typeof(Vector2), false).Add("x", "y");
        if (!RuntimeTypeModel.Default.IsDefined(typeof(Vector3)))
            RuntimeTypeModel.Default.Add(typeof(Vector3), false).Add("x", "y", "z");
        if (!RuntimeTypeModel.Default.IsDefined(typeof(Color)))
            RuntimeTypeModel.Default.Add(typeof(Color), false).Add("a", "b", "g", "r");
        if (!RuntimeTypeModel.Default.IsDefined(typeof(Quaternion)))
            RuntimeTypeModel.Default.Add(typeof(Quaternion), false).Add("w", "x", "y", "z");

        fieldNumbers = new Dictionary<Type, int>();
    }

    static void AddDerivedType(Type p, Type t, int fieldNumber, string[] memberNames)
    {
        var metaType = RuntimeTypeModel.Default.Add(t, true);
        RuntimeTypeModel.Default[p].AddSubType(fieldNumber, t);

        if (memberNames != null)
            metaType.Add(memberNames);
    }

    public static T DeserializeFromFile<T>(string path)
    {
        T deserializedObject;

        using (FileStream f = new FileStream(path, FileMode.Open))
        {
            deserializedObject = Serializer.Deserialize<T>(f);
        }

        return deserializedObject;
    }

    public static T DeserializeFromBytes<T>(byte[] bytes)
    {
        T deserializedObject;

        using (var ms = new MemoryStream(bytes))
        {
            deserializedObject = Serializer.Deserialize<T>(ms);
        }

        return deserializedObject;
    }

    public static void SerializeToFile(string path, object serializedObject)
    {
        using (FileStream f = new FileStream(path, FileMode.Create))
        {
            Serializer.Serialize(f, serializedObject);
        }
    }

    public static void SerializeToFile<T>(string objPath, string fileName, T serializedObject)
    {
        //if (!Directory.Exists(objPath))
        //{
        //    Directory.CreateDirectory(objPath);
        //}

        using (FileStream f = new FileStream(objPath + fileName, FileMode.Create))
        {
            Serializer.Serialize(f, serializedObject);
        }
    }

    public static byte[] SerializeToBytes<T>(T serializedObject)
    {
        using (var ms = new MemoryStream())
        {
            Serializer.Serialize(ms, serializedObject);
            return ms.ToArray();
        }
    }

    public static Stream SerializeToStream<T>(Stream s, T serializedObject)
    {
        Serializer.Serialize(s, serializedObject);
        return s;
    }
}