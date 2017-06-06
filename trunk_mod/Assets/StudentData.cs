using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentData : DataPiece {

    [SerializeField]
    public string firstName;
    [SerializeField]
    public string lastName;
    [SerializeField]
    public DictionaryOfStringAndString detectors;
    [SerializeField]
    public IndicatorControl indicator;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
