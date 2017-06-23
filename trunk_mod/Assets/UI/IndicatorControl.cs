using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;

public class IndicatorControl : MonoBehaviour
{
    public GameObject normalStudentIndicator;
    public static GameObject normalStudentIndicatorStatic;
    public GameObject criticalStruggleIndicator;
    public static GameObject criticalStruggleIndicatorStatic;
    public GameObject idleIndicator;
    public static GameObject idleIndicatorStatic;
    public GameObject systmeMisuseIndicator;
    public static GameObject systemMisuseIndicatorStatic;
    public GameObject struggleIndicator;
    public static GameObject struggleIndicatorStatic;
    public GameObject studentDoingWellIndicator;
    public static GameObject studentDoingWellIndicatorStatic;
    public GameObject invisibleHandRaiseIndicator;
    public static GameObject invisibleHandRaiseIndicatorStatic;

    public static bool inMoveMode = false;

    private static int counter = 1;
    private static WorldAnchorStore anchorStore;

    private static Dictionary<string, int> precidence = new Dictionary<string, int>();

    // Use this for initialization
    void Start()
    {
        //establish the precidence chain
        precidence.Add("", 0);
        precidence.Add("student_doing_well", 1);
        precidence.Add("struggle", 2);
        precidence.Add("system_misuse", 3);
        precidence.Add("idle", 4);
        precidence.Add("critical_struggle", 5);


        //To allows the prefab to be drag and drop
        normalStudentIndicatorStatic = normalStudentIndicator;
        criticalStruggleIndicatorStatic = criticalStruggleIndicator;
        idleIndicatorStatic = idleIndicator;
        systemMisuseIndicatorStatic = systmeMisuseIndicator;
        struggleIndicatorStatic = struggleIndicator;
        studentDoingWellIndicatorStatic = studentDoingWellIndicator;
        invisibleHandRaiseIndicatorStatic = invisibleHandRaiseIndicator;


        //Anchoring stuff??? not sure if it works yet
        Debug.Log("Go into GetAysnc Thing");
        WorldAnchorStore.GetAsync(StoreLoaded);
        Debug.Log("Go into GetAysnc Thing Part 2");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Anchor stuff
    /*void OnSelect()
    {
        if (inMoveMode)
        {
            GameObject studentIndicator = this.gameObject;
            DataPiece si;
            SavedData.data.get(studentIndicator.name, out si);
            if (si == null)
                Debug.LogError("This student does not exist in dictionary, but indicator exists for them.");

            //changeWorldAnchor(studentIndicator, si);
        }
        else
        {
            //code to display more info (like the 3 panel thing)
        }
    }*/

    //Anchor stuff
    private void StoreLoaded(WorldAnchorStore store)
    {
        Debug.Log("In StoreLoaded. This is the store: " + store);
        anchorStore = store;
    }

    //Anchor stuff
    public static void createWorldAnchor(GameObject studentIndicator, string si_id)
    {
        if (anchorStore == null)
        {
            Debug.LogError("The anchorStore is null.");
            //studentIndicator.GetComponent<MeshRenderer>().material.color = Color.white;
            return;
        }
        WorldAnchor wa = anchorStore.Load(si_id, studentIndicator);
        if (wa == null)
        {
            wa = studentIndicator.AddComponent<WorldAnchor>();
            anchorStore.Save(si_id, wa); //outside
            anchorStore.Load(si_id, studentIndicator); //not there
        }
    }

    //Anchor stuff
    public static void removeWorldAnchor(GameObject studentIndicator, string si_id)
    {
        if (anchorStore == null)
        {
            Debug.LogError("The anchorStore is null.");
            //studentIndicator.GetComponent<MeshRenderer>().material.color = Color.white;
            return;
        }
        //if we're not in move mode, we do not wanna be able to change the world anchor
        if (inMoveMode)
        {
            WorldAnchor wa = anchorStore.Load(si_id, studentIndicator);
            if (wa == null)
                Debug.LogError("You're trying to change the anchor attached to this indicator when there was no indicator to begin with.");
            else
            {
                anchorStore.Delete(si_id);
                Destroy(studentIndicator.GetComponent<WorldAnchor>());
            }

            //some code to actually move the indicator

            //This will save (achor) the new location you have moved to
            //createWorldAnchor(studentIndicator, si_id);
        }
        else
            Debug.LogError("You're trying to change the anchor attached to this indicator when you're not in move mode.");


    }

    //Indicator Stuff
    public static GameObject modifyIndicator(GameObject obj, string changedDetector)
    {
        //actually do legit stuff
        //destory the obj passed to you bc you're gonna be creating a new one to display
        //make sure to modify the student info (like in data piece) as in what mode you're in and what info is being displayed
        return obj;
    }

    //Indicator Stuff
    public static GameObject createStudentIndicator(DataPiece si)
    {
        GameObject studentIndicator = Instantiate(normalStudentIndicatorStatic);

        //Set the name to unique id so that it's easy to find later in updateIndicator
        studentIndicator.name = si.id;

        //Sets position and scale
        studentIndicator.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        studentIndicator.transform.position = new Vector3((counter * 0.5f) - 3, 0, 7 - (counter * 0.6f));

        //Adds a WorldAnchor to the game object (student indicator)
        createWorldAnchor(studentIndicator, si.id);

        counter++;

        //If there is anything we wanna display on the sphere
        si.infoBeingDisplayed = "";

        return studentIndicator;
    }

    private static void createInvisibleHandIndicatorIndicator(GameObject otherIndicator)
    { 
        GameObject invisibleHandIndicator = Instantiate(invisibleHandRaiseIndicatorStatic);

        //Set the name to unique id so that it's easy to find later in updateIndicator
        invisibleHandIndicator.name = otherIndicator.name + "IH"; //IH for invisible hand

        //Make the hand a child of the other indicator
        invisibleHandIndicator.transform.parent = otherIndicator.transform;

        //Sets position and scale
        invisibleHandIndicator.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        invisibleHandIndicator.transform.localPosition = (new Vector3(-.6f, -.4f, .3f));
        //rotation: 180, 60, 20
    }

    private static void removeInvisibleHandIndicatorIndicator(DataPiece si)
    {
        GameObject invisibleHandIndicator = GameObject.Find(si.id + "IH");
        if (invisibleHandIndicator == null)
            Debug.LogError("Tryting to remove a handRaiseIndicator that does not exist");

        Destroy(invisibleHandIndicator);
    }

    //need to deal with the hadn indicator
    public static void updateStudentIndicator(string changedDetector, DataPiece si)
    {
        //if changedDetector is being displayed, modify the indicator
        //othersiwse do nothing

        //get the game indicator we're gonna be dealing with
        GameObject indicator = GameObject.Find(si.id);
        if (indicator == null)
        {
            indicator = createStudentIndicator(si);
        }

        //deal with old invalid detectors cuz code will crash
        if (changedDetector.Equals("current_attempt_count") || changedDetector.Equals("help_model_try_if_low"))
            return;

        //get the value of the detector
        string detector_value;
        si.detectors.TryGetValue(changedDetector, out detector_value);
        if (detector_value == null || detector_value == "")
            Debug.LogError("No valid value assoicated with the detector");

        //determine if detector is on
        bool detector_on = detector_value.StartsWith("1");

        //NEED TO CHANGE THIS. NEED A SPEARATE INDICATOR FOR THE RAISED HAND THING
        if (changedDetector.Equals("invisible_hand_raise"))
        {
            if (detector_on && !si.handRaised)
            {
                createInvisibleHandIndicatorIndicator(indicator);
                si.handRaised = true;
            }
            else if (!detector_on && si.handRaised)
            {
                removeInvisibleHandIndicatorIndicator(si);
                si.handRaised = false;
            }
            return;
        }

        //Get's the precidence ordering of these detectors
        int precidence_of_changedDetector = precidence[changedDetector.ToLower()];
        int precidence_of_currentDetector = precidence[si.infoBeingDisplayed.ToLower()];

        //string current_detector, new_detector;
        //makes it so that if the new detector has an equal or lower precidence as the one being displayed rn
        //don't change the indicator
        if (precidence_of_changedDetector <= precidence_of_currentDetector)
            return;

        //if the detector is on and we know it has higher precidence, then we know we're gonna switch to it
        if(detector_on)
            si.infoBeingDisplayed = changedDetector;

        //this is to make the code easier to deal with in DataPiece
        si.detectors_on_array[precidence_of_changedDetector] = detector_on;
        switch (changedDetector.ToLower())
        {
            case "critical_struggle":
                if (detector_on)
                {
                    indicator.GetComponent<MeshRenderer>().material = (Material)Resources.Load("UI_Resources/Materials/CriticalStruggleIndicator", typeof(Material));
                    return;
                }
                break;
            case "idle":
                if (detector_on)
                {
                    indicator.GetComponent<MeshRenderer>().material = (Material)Resources.Load("UI_Resources/Materials/IdleIndicator", typeof(Material));
                    return;
                }
                break;
            case "system_misuse":
                if (detector_on)
                {
                    indicator.GetComponent<MeshRenderer>().material = (Material)Resources.Load("UI_Resources/Materials/SystemMisuseIndicator", typeof(Material));
                    return;
                }
                break;
            case "struggle":
                if (detector_on)
                {
                    indicator.GetComponent<MeshRenderer>().material = (Material)Resources.Load("UI_Resources/Materials/StruggleIndicator", typeof(Material));
                    return;
                }
                break;
            case "student_doing_well":
                if(detector_on)
                {
                    indicator.GetComponent<MeshRenderer>().material = (Material)Resources.Load("UI_Resources/Materials/StudentDoingWellIndicator", typeof(Material));
                    return;
                }
                break;
            default:
                Debug.LogError("detector did not match an existing detector (detector unknow)");
                break;
        }

        //if none of the cases applied, then the detecotr must be off so we gotta update
        //Destroy(indicator);
        si.stateUpdate(precidence_of_currentDetector);
    }
}
