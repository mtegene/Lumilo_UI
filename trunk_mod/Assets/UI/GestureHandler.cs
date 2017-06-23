using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Academy;
using Academy.HoloToolkit.Unity;

public class GestureHandler : Singleton<GestureHandler>
{
    private bool isActive = false;
    //private int count = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            gameObject.transform.position = GazeManager.Instance.Position;
            //gameObject.transform.position = new Vector3(0, 0, 0);
            //count++;
        }
        //else
        //gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    void OnAirTapped()
    {
        isActive = !isActive;
    }

    void OnSelect(GameObject focusedObject)
    {
        isActive = !isActive;
        //focusedObject.GetComponent<MeshRenderer>().material.color = Color.green;
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
