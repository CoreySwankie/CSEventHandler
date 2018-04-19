using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionInfo : MonoBehaviour {

    public static SessionInfo instance;
    // make sure there is only one session info on the scene ever
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("there is already a session info loaded");
            return;
        }
        instance = this;

        // create session id and user id here so any scripts that may been access to these right away can get them.
        CreateSessionID();
        userID = SystemInfo.deviceName; //for now just use the device name

        // tag this object so that it will not be destroyed when transitioning between scenes.
        DontDestroyOnLoad(instance);
    }

    string sessionID;
    string userID;

    [HideInInspector]
    char[] validCharacters = new char[] {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z',
        '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'};

    // Use this for initialization
    void Start ()
    {
        // Application Start event
        CSEventHandler.BasicEvent(
            GetSessionID(),
            GetUserID(),
            "ApplicationStart"
            );
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // simple function that takes the current time along with the device name and ranomdises them together to create a "unique" session id
    void CreateSessionID()
    {
        string time = System.DateTime.Now.ToString();
        string systemName = SystemInfo.deviceName;

        string stringToRandomised = time + systemName;
        
        List<char> buf = new List<char>();
        foreach (char item in stringToRandomised)
        {
            //if (item != ' ' && item != '/' && item != ':')
            //{
            //    buf.Add(item);
            //}

            // loop through all the valid characters and compare the new char against it, if its valid then use it and exit the loop
            foreach (char validChar in validCharacters)
            {
                if (item == validChar)
                {
                    buf.Add(item);
                    break;
                }
            }
        }

        int numOfCharsToGet = buf.Count;
        string randomised = "";
        for (int i = 0; i < numOfCharsToGet; i++)
        {
            int randNum = Random.Range(0, buf.Count);
            randomised += buf[randNum];
            buf.RemoveAt(randNum);
        }

        sessionID = randomised;
    }


    public string GetSessionID()
    {
        return sessionID;
    }

    public string GetUserID()
    {
        return userID;
    }


    // when the application quits this is called sending an event to the json event 
    void OnApplicationQuit()
    {
        CSEventHandler.BasicDictionaryEvent(
            GetSessionID(),
            GetUserID(),
            "ApplicationExit",
            new Dictionary<string, object> {
                { "RunTime", Time.time }

            });

        Debug.Log("session quit");
    }
}
