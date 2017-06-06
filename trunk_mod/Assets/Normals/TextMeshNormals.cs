using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshNormals : MonoBehaviour
{
    private TextMesh textMesh;

    // Use this for initialization
    void Start()
    {
        // reassign font texture to our material
        textMesh = transform.GetComponent<TextMesh>();
        GetComponent<Renderer>().material.mainTexture = textMesh.font.material.mainTexture;
    }
}
