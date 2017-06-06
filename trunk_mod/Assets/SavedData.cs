using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SavedData : MonoBehaviour {

    public static SavedData data;

    public /*Dictionary<string, DataPiece>*/DictionaryOfStringAndDataPiece central_dictionary;
    
    /*public Dictionary<string, StudentData> student_dict; //assocates a student with their info like name, progress, etc.
    public Dictionary<string, TeacherData> teacher_dict; //assocates a teacher with the classes the teacher teaches and detectors(?)
    public Dictionary<string, ClassData> class_dict; //assocates a class with the teacher teaching it and the students taking it and detectors(?)*/

    // Use this for initialization
    void Awake () {
        if (data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }
	}
	

    public bool add(string s, DataPiece o)
    {
        if (central_dictionary == null)
            central_dictionary = new DictionaryOfStringAndDataPiece(); //new Dictionary<string, DataPiece>();
        SavedData.data.central_dictionary.Add(s, o);
        return true;
    }

    public bool remove(string s)
    {
        return SavedData.data.central_dictionary.Remove(s);
    }

    public bool get(string s, out DataPiece d)
    {
        if (central_dictionary == null)
        {
            d = null;
            return false;
        }
        return SavedData.data.central_dictionary.TryGetValue(s, out d);
    }

    //might be inefficent. should maybe check if file already exists and then write to it
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/appInfo.dat");

        AppData app_data = new AppData();
        app_data.dict = central_dictionary;

        central_dictionary.OnBeforeSerialize();
        bf.Serialize(file, app_data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/appInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/appInfo.dat", FileMode.Open);

            AppData app_data = (AppData)(bf.Deserialize(file));
            file.Close();

            central_dictionary = app_data.dict;
            central_dictionary.OnAfterDeserialize();
        }
    }

}

[Serializable]
class AppData
{
    [SerializeField]
    public DictionaryOfStringAndDataPiece dict;
}

[Serializable]
public class DictionaryOfStringAndDataPiece : SerializableDictionary<string, DataPiece> { }

[Serializable]
public class DictionaryOfStringAndString : SerializableDictionary<string, string> { }

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}

[Serializable]
public class IndicatorControl
{
    [SerializeField]
    public GameObject studentIndicator;
    [SerializeField]
    public bool in_move_mode;

    // Use this for initialization
    void Start()
    {
        studentIndicator = new GameObject();//GameObject.CreatePrimitive(PrimitiveType.Sphere);
        studentIndicator.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

