using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForSessionInfo : MonoBehaviour {

    public GameObject sessionInfoPrefab = null;

    // Use this for initialization
    void Start()
    {
        Debug.Log(Application.dataPath);
        // if there is a session info in the game then just exit script
        if (FindObjectOfType<SessionInfo>())
        {
            Debug.Log("Found a session info object, no need to create another");
            return;
        }
        else // if there is not a session info object then create one
        {
            Instantiate(sessionInfoPrefab);
        }


    }
	
}
