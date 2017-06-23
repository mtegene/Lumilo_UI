using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


public class SavedData : MonoBehaviour {

    public static SavedData data;

    public Dictionary<string, DataPiece> central_dictionary;

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
            central_dictionary = new Dictionary<string, DataPiece>();
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
        //BinaryFormatter bf = new BinaryFormatter();
        XmlSerializer ser = new XmlSerializer(typeof(DataSerializer));
        FileStream file = File.Create(Application.persistentDataPath + "/appInfo22.dat");

        /*foreach (KeyValuePair<string, DataPiece> pair in central_dictionary)
        {
            DataPiece dt = pair.Value;
            DetectorSerializer t2 = new DetectorSerializer();
            t2.beforeSter(dt.detectors);
            dt.detector_saver = t2;
        }*/
        //Debug.LogError("In Save()");

        DataSerializer t = new DataSerializer();
        t.beforeSter(DataPieceSer.changeDataPieceDictionary(SavedData.data.central_dictionary));
        //bf.Serialize(file, t);
        ser.Serialize(file, t);
        //file.Close();
        file.Dispose();

        /*Debug.LogError("The to be saved Keys:" + SavedData.data.central_dictionary.Keys.Count);

        string debug_string = "Saved Data: ";
        foreach (KeyValuePair<string, DataPiece> pair in SavedData.data.central_dictionary)
        {
            debug_string += pair.Key + " = " + pair.Value.toString();
        }
        Debug.LogError(debug_string);*/
    }

    public void Load()
    {
        //Debug.LogError("In Load()");
        if (File.Exists(Application.persistentDataPath + "/appInfo22.dat"))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //Debug.LogError("In Load(), file exists");

            XmlSerializer ser = new XmlSerializer(typeof(DataSerializer));
            FileStream file = File.Open(Application.persistentDataPath + "/appInfo22.dat", FileMode.Open);

            //i think indicators should be reloaded????          

            DataSerializer t = (DataSerializer)ser.Deserialize(file);
            //file.Close();
            file.Dispose();
            SavedData.data.central_dictionary = t.afterSter();

            /*foreach (KeyValuePair<string, DataPiece> pair in central_dictionary)
            {
                DataPiece dt = pair.Value;
                DetectorSerializer t2 = dt.detector_saver;
                dt.detectors = t2.afterSter();
            }*/
            /*string debug_string = "Loaded Data: ";
            foreach (KeyValuePair<string, DataPiece> pair in SavedData.data.central_dictionary)
            {
                debug_string += pair.Key;// + " = " + pair.Value.toString();
            }
            Debug.LogError(debug_string);*/
        }
    }

}

public class DataPiece
{
    //All data types will have this
    public string datatype; //will indicate if it is a student, class, or teacher

    //Students and Classes will have this
    public string id; //unique identifier for classes and students

    //Teahcers and students will have this
    public string firstName;
    public string lastName;
    /*NOTE: if firstName = class and lastName = summary, it is a class summary object*/

    //Teachers will have this
    public List<string> classes = new List<string>(); //keeps track of what classes this teacher teaches (by class id)

    //Classes will have this
    public string class_summary; //this is the id for the class_summary [treated like a student]
    public List<string> students = new List<string>(); //keeps track of the students in this class (by student id)

    //Students will have this 
    public string infoBeingDisplayed; //inicates what info is being displayed on the student's indicator
    public Dictionary<string, string> detectors = new Dictionary<string, string>(); //Keeps track of what detectors this student has associated with them
    // indicator trackers (not to be searalized)
    public bool handRaised;
    public bool[] detectors_on_array = new bool[6];
    private string[] detectors_names = {"", "student_doing_well", "struggle", "system_misuse", "idle", "crtitical_struggle"};
    public bool inCrtiticalStruggleMode;
    public bool inIdleMode;
    public bool inSystemMisuseMode;
    public bool inStruggleMode;
    public bool inStudentDoingWellMode;

    public void stateUpdate(int start)
    {
        for (int i = start - 1; i > 0; i--)
        {
            if (detectors_on_array[i])
            {
                IndicatorControl.updateStudentIndicator(detectors_names[i], this);
                return;
            }
        }

        GameObject indicator = GameObject.Find(this.id);
        if (indicator == null)
            Debug.LogError("??????? Something is wrong regarding indicators");

        indicator.GetComponent<MeshRenderer>().material = (Material)Resources.Load("UI_Resources/Materials/NormalIndicator", typeof(Material));
    }

    //Methods for classes variable
    public void addToClasses(string c)
    {
        if (datatype.Equals("teacher"))
        {
            if (classes == null)
                classes = new List<string>();
            classes.Add(c);
        }
        else
            Debug.LogError("You are adding to the classes variable, when the data piece is not a teacher");
    }

    public bool removeFromClasses(string c)
    {
        if (datatype.Equals("teacher"))
        {
            return classes.Remove(c);
        }
        else
        {
            Debug.LogError("You are removing to the classes variable, when the data piece is not a teacher");
            return false;
        }
    }

    public bool containedInClasses(string c)
    {
        if (datatype.Equals("teacher"))
        {
            return classes.Contains(c);
        }
        else
        {
            Debug.LogError("You are getting information from the classes variable, when the data piece is not a teacher");
            return false;
        }
    }

    //Methods for class variable
    public void addToStudents(string c)
    {
        if (datatype.Equals("class"))
        {
            if (students == null)
                students = new List<string>();
            students.Add(c);
        }
        else
            Debug.LogError("You are adding to the students variable, when the data piece is not a class");
    }

    public bool removeFromStudents(string c)
    {
        if (datatype.Equals("class"))
        {
            return students.Remove(c);
        }
        else
        {
            Debug.LogError("You are removing to the students variable, when the data piece is not a class");
            return false;
        }
    }

    public bool containedInStudents(string c)
    {
        if (datatype.Equals("class"))
        {
            return students.Contains(c);
        }
        else
        {
            Debug.LogError("You are getting information from the students variable, when the data piece is not a class");
            return false;
        }
    }

    public String toString()
    {
        string s = "";
        s += "datatype: " + datatype + ", ";
        s += "id: " + id + ", ";
        s += "firstName: " + firstName + ", ";
        s += "lastName: " + lastName + ", ";
        s += "classes: " + classes.ToString() + ", ";
        s += "class_summary: " + class_summary + ", ";
        s += "students: " + students.ToString() + ", ";
        s += "infoBeingDisplayed: " + infoBeingDisplayed + ", ";
        s += "detectors: " + detectors.ToString() + ".";
        return s;
    }
}

public class DataPieceSer
{
    //All data types will have this
    public string datatype; //will indicate if it is a student, class, or teacher

    //Students and Classes will have this
    public string id; //unique identifier for classes and students

    //Teahcers and students will have this
    public string firstName;
    public string lastName;
    /*NOTE: if firstName = class and lastName = summary, it is a class summary object*/

    //Teachers will have this
    public string[] classes; //keeps track of what classes this teacher teaches (by class id)

    //Classes will have this
    public string class_summary; //this is the id for the class_summary [treated like a student]
    public string[] students; //keeps track of the students in this class (by student id)

    //Students will have this 
    public string infoBeingDisplayed; //inicates what info is being displayed on the student's indicator
    public DetectorSerializer detector_saver; //Use this to serialze the dectors [different procedure bc it's a dictionary]

    public static DataPiece changeDataPieceSertoDataPiece(DataPieceSer d)
    {
        DataPiece ans = new DataPiece();
        ans.datatype = d.datatype;
        ans.id = d.id;
        ans.firstName = d.firstName;
        ans.lastName = d.lastName;

        if (d.classes == null)
            ans.classes = new List<string>();
        else
            ans.classes = new List<string>(d.classes);

        ans.class_summary = d.class_summary;

        if (d.students == null)
            ans.students = new List<string>();
        else
            ans.students = new List<string>(d.students);

        ans.infoBeingDisplayed = d.infoBeingDisplayed;
        ans.detectors = d.detector_saver.afterSter();
        return ans;
    } 

    public static DataPieceSer changeDataPiecetoDataPieceSer(DataPiece d)
    {
        DataPieceSer ans = new DataPieceSer();
        ans.datatype = d.datatype;
        ans.id = d.id;
        ans.firstName = d.firstName;
        ans.lastName = d.lastName;

        if (d.classes == null)
            ans.classes = null;
        else
            ans.classes = d.classes.ToArray();

        ans.class_summary = d.class_summary;
        if (d.students == null)
            ans.students = null;
        else
            ans.students = d.students.ToArray();

        ans.infoBeingDisplayed = d.infoBeingDisplayed;
        ans.detector_saver = new DetectorSerializer();
        ans.detector_saver.beforeSter(d.detectors);
        return ans;
    }

    public static Dictionary<string, DataPieceSer> changeDataPieceDictionary (Dictionary<string, DataPiece> dic)
    {
        Dictionary<string, DataPieceSer> ans = new Dictionary<string, DataPieceSer>();
        foreach (KeyValuePair<string, DataPiece> pair in dic)
        {
            ans.Add(pair.Key, changeDataPiecetoDataPieceSer(pair.Value));
        }
        return ans;
    }

    public static Dictionary<string, DataPiece> changeDataPieceSerDictionary(Dictionary<string, DataPieceSer> dic)
    {
        Dictionary<string, DataPiece> ans = new Dictionary<string, DataPiece>();
        foreach (KeyValuePair<string, DataPieceSer> pair in dic)
        {
            ans.Add(pair.Key, changeDataPieceSertoDataPiece(pair.Value));
        }
        return ans;
    }
}

//Objective/Function of this class:
//It is used to serialize a DataPiece
[Serializable]
public class DataSerializer
{
    //[XmlIgnore]
    //[SerializeField]
    [XmlAttribute("keys")]
    public string[] keys;// = new List<string>();
    //[SerializeField]
    [XmlArrayAttribute("values")]
    public DataPieceSer[] values; //= new List<DataPiece>();

    public void beforeSter(Dictionary<string, DataPieceSer> dic)
    {
        //keys.Clear();
        //values.Clear();
        keys = new string[dic.Count];
        values = new DataPieceSer[dic.Count];
        int i = 0;
        foreach (KeyValuePair<string, DataPieceSer> pair in dic)
        {
            keys[i] = pair.Key;
            values[i] = pair.Value;
            i++;
        }
    }

    public Dictionary<string, DataPiece> afterSter()
    {
        Dictionary<string, DataPiece> dic = new Dictionary<string, DataPiece>();
        for (int i = 0; i < keys.Length; i++)
        {
            dic.Add(keys[i], DataPieceSer.changeDataPieceSertoDataPiece(values[i]));
        }
        return dic;
    }
}

//Objective/Function of this class:
//It is used to serialize each student's
//detector dictionary (which is a <string, string>
//dictionary). 
[Serializable]
public class DetectorSerializer
{
    [XmlAttribute("keys")]
    public string[] keys;// = new List<string>();
    [XmlAttribute("values")]
    public string[] values;// = new List<string>();

    public void beforeSter(Dictionary<string, string> dic)
    {
        keys = new string[dic.Count];
        values = new string[dic.Count];
        int i = 0;
        foreach (KeyValuePair<string, string> pair in dic)
        {
            keys[i] = pair.Key;
            values[i] = pair.Value;
            i++;
        }
    }

    public Dictionary<string, string> afterSter()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        for (int i = 0; i < keys.Length; i++)
        {
            dic.Add(keys[i], values[i]);
        }
        return dic;
    }
}
