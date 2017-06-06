using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherData : DataPiece {

    [SerializeField]
    public string firstName;
    [SerializeField]
    public string lastName;
    [SerializeField]
    public List<string> assigments; //should make a class for assignments

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
