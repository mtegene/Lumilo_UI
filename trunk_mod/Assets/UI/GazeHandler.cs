using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Academy;
using Academy.HoloToolkit.Unity;

//currenlty does nothing
public class GazeHandler : Singleton<GazeHandler> {

    private Color intialColor;
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGazeEnter()
    {
        var renderer = gameObject.GetComponent<Renderer>();
        intialColor = renderer.material.color;
        renderer.material.color = Color.white;
    }

    void OnGazeExit()
    {
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = intialColor;
    }
}
