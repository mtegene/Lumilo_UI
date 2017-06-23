using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using Academy.HoloToolkit.Unity;



public class MoveModeScripit :  MonoBehaviour{

    private GestureRecognizer gestureRecognizer;

    void Start()
    {
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

        gestureRecognizer.TappedEvent += (source, tapCount, ray) =>
        {
            GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;

            if (focusedObject != null && focusedObject.name.Equals("MoveModeButton"))
            {
                focusedObject.SendMessage("OnSelect");
            }
        };

        gestureRecognizer.StartCapturingGestures();

        this.gameObject.GetComponent<MeshRenderer>().material = off;
        //GestureManager.Instance.ManipulationRecognizer.StopCapturingGestures();
    }

    void OnDestroy()
    {
        gestureRecognizer.StopCapturingGestures();
    }

    public Material on;
    public Material off;

    void OnSelect(){
        IndicatorControl.inMoveMode = !IndicatorControl.inMoveMode;
        GameObject button = this.gameObject;

        if (IndicatorControl.inMoveMode)
        {
            button.GetComponent<MeshRenderer>().material = on;
            GestureManager.Instance.ManipulationRecognizer.StartCapturingGestures();
        }
        else
        {
            button.GetComponent<MeshRenderer>().material = off;
            GestureManager.Instance.ManipulationRecognizer.StopCapturingGestures();
        }
    }
}
