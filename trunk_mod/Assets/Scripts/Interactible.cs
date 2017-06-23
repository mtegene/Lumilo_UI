using UnityEngine;

/// <summary>
/// The Interactible class flags a Game Object as being "Interactible".
/// Determines what happens when an Interactible is being gazed at.
/// </summary>
public class Interactible : MonoBehaviour
{
    [Tooltip("Audio clip to play when interacting with this hologram.")]
    //public AudioClip TargetFeedbackSound;
    //private AudioSource audioSource;

    private int count;

    private Material[] defaultMaterials;
    private GameObject theFocusedObject;

    public bool moving = false;
    public bool fakeMode2 = true;
    //public GameObject testingText;

    // Update is called once per frame
    void Update()
    {
        if (moving)
            this.gameObject.transform.Translate(0, 5, 0);
        //testingText.GetComponent<TextMesh>().text = "" + count;
        count++;
    }

    void Start()
    {
        moving = false;
        defaultMaterials = GetComponent<Renderer>().materials;

        if (defaultMaterials == null)
            Debug.LogError("hmmm idk tbh");
        else
            Debug.LogError("so far so good");

        // Add a BoxCollider if the interactible does not contain one.
        Collider collider = GetComponentInChildren<Collider>();
        if (collider == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        //EnableAudioHapticFeedback();
    }

    /*private void EnableAudioHapticFeedback()
    {
        // If this hologram has an audio clip, add an AudioSource with this clip.
        if (TargetFeedbackSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = TargetFeedbackSound;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
        }
    }*/

    /* TODO: DEVELOPER CODING EXERCISE 2.d */

    void GazeEntered()
    {
        if (defaultMaterials == null)
        {
            Debug.LogError("why tho>");
            Start();
        }
        else
            Debug.LogError("still okay>>");
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            // 2.d: Uncomment the below line to highlight the material when gaze enters.
            defaultMaterials[i].SetFloat("_Highlight", .25f);
        }
    }

    void GazeExited()
    {
        if (defaultMaterials == null)
        {
            Debug.LogError("why tho<");
            Start();
        }
        else
            Debug.LogError("still okay<<");
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            // 2.d: Uncomment the below line to remove highlight on material when gaze exits.
            defaultMaterials[i].SetFloat("_Highlight", 0f);
        }
    }

    void OnSelect(GameObject focusedObject)
    {
        /*if (defaultMaterials == null)
        {
            Debug.LogError("why tho?");
            Start();
        }
        else
            Debug.LogError("still okay??");
        for (int i = 0; i < defaultMaterials.Length; i++)
        {
            defaultMaterials[i].SetFloat("_Highlight", .5f);
        }*/
        this.gameObject.transform.Translate(0, 5, 0);

        //focusedObject.GetComponent<MeshRenderer>().material.color = Color.white;
        focusedObject.transform.Translate(5, 5, 0);

        theFocusedObject = focusedObject;
        this.moving = !this.moving;


        // Play the audioSource feedback when we gaze and select a hologram.
        /*if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }*/

        /* TODO: DEVELOPER CODING EXERCISE 6.a */
        // 6.a: Handle the OnSelect by sending a PerformTagAlong message.

    }

    /*void Update()
    {
        if (moving)
            theFocusedObject.transform.Translate(50, 50, 0);
    }*/


}