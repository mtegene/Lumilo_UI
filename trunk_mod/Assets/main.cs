﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using BestHTTP;
using BestHTTP.SocketIO;

public class main : MonoBehaviour
{     
    SocketManager manager;

    //int counter = 1;

    public void ConnectToStream()
    {
        SocketOptions options = new SocketOptions();
        options.AutoConnect = false;

        manager = new SocketManager(new Uri("http://fractions01.hcii.cs.cmu.edu:3000/socket.io/"), options);
        //Socket sockChat = manager.GetSocket("/socket.io"); 
        manager.Socket.On(SocketIOEventTypes.Error, (socket, packet, args) => Debug.LogError(string.Format("Error: {0}", args[0].ToString())));
        manager.Socket.On(SocketIOEventTypes.Connect, OnServerConnect);
        manager.Socket.On(SocketIOEventTypes.Disconnect, OnServerDisconnect);
        manager.Socket.On(SocketIOEventTypes.Error, OnError);
        manager.Socket.On("reconnect", OnReconnect);
        manager.Socket.On("reconnecting", OnReconnecting);
        manager.Socket.On("reconnect_attempt", OnReconnectAttempt);
        manager.Socket.On("reconnect_failed", OnReconnectFailed);

        manager.Socket.On("message", OnMessage);

        manager.Socket.On("update", OnUpdate);
        manager.Socket.On("init", OnInit);

        manager.Socket.On("initBack", OnInitBack);

        manager.Socket.On("error", OnSSError);

        manager.Open();
    }

    public void LogIn()
    {
       //right now, do nothing
    }


    void Start()
    {
        //ConnectToStream(LogIn());
        ConnectToStream(); 
    }

    // event handler
    void OnInit(Socket socket, Packet packet, params object[] args)
    {
       Debug.Log(string.Format("{0}", args[0] ));
    }
    // event handler
    void OnUpdate(Socket socket, Packet packet, params object[] args)
    {

        List<object> detectorResults = args[0] as List<object>;

        //iterate over detector results list ... 
        //  process each entry in order to update a central, global representation (per-student dictionary named studentModels)
        foreach (Dictionary<string, object> detectorResult in detectorResults)
        {
            Dictionary<string, object> thisUser = detectorResult["user"] as Dictionary<string, object>;
            string thisUserFirstName = thisUser["first_name"] as string;
            string thisUserLastName = thisUser["last_name"] as string;
            string thisUserUserName = thisUser["username"] as string;
            string thisDetector = detectorResult["name"] as string;
            string thisValue = detectorResult["value"] as string;

            string testOutput = string.Format("User: {0} {1} ({2}), \n Detector: {3}, \n Value: {4}", thisUserFirstName, thisUserLastName, thisUserUserName, thisDetector, thisValue);
            //Debug.Log(testOutput);

            GetComponent<TextMesh>().text = testOutput;
            DataPiece info;
            if (SavedData.data.get(thisUserUserName, out info))
            {
                //updating existing students info
                Debug.Log("updating existing student info");
                if (info.detectors == null)
                    Debug.LogError("Dictionary of detectors for this student has not been intialized");

                //update the value assocated with this detector. you rewrite the previous value.
                info.detectors.Remove(thisDetector);
                info.detectors.Add(thisDetector, thisValue);

                /*string debug;
                info.detectors.TryGetValue(thisDetector, out debug);
                Debug.Log(thisDetector + " " + debug);

                if (info.id.EndsWith("08"))
                    Debug.Log("BOOOOOOOOOOOO");
                if (info.id.EndsWith("08"))
                    Debug.Log(thisDetector + " " + debug);*/

                IndicatorControl.updateStudentIndicator(thisDetector, info);
            }
            else
            {
                //adding a new student
                info = new DataPiece();
                info.datatype = "student";
                info.firstName = thisUserFirstName;
                info.lastName = thisUserLastName;
                info.id = thisUserUserName;

                info.detectors = new Dictionary<string, string>();
                info.detectors.Add(thisDetector, thisValue);

                IndicatorControl.createStudentIndicator(info);

                SavedData.data.add(thisUserUserName, info);

                Debug.Log("CREATED NEW STUDENT: " + thisUserUserName);
            }
        }
    }

    // event handler
    void OnInitBack(Socket socket, Packet packet, params object[] args)
    {
        SavedData.data.Load();

        Dictionary<string, object> initInfo = args[0] as Dictionary<string, object>;
        Dictionary<string, object> initClassInfo = initInfo["class"] as Dictionary<string, object>;
        Dictionary<string, object> initTeacherInfo = initClassInfo["teacher"] as Dictionary<string, object>;

        Debug.Log(string.Format("Hello {0} {1}! Showing data for the class: {2}", initTeacherInfo["first_name"], initTeacherInfo["last_name"], initClassInfo["name"]));


        List<object> detectorInitializations = initInfo["detector_results"] as List<object>;

        //iterate over detector results list ... 
        //  process each entry in order to update a central, global representation (per-student dictionary named studentModels)
        foreach (Dictionary<string,object> detectorInit in detectorInitializations)
        {
            Dictionary<string,object> thisUser = detectorInit["user"] as Dictionary<string,object>;
            string thisUserFirstName = thisUser["first_name"] as string;
            string thisUserLastName = thisUser["last_name"] as string;
            string thisUserUserName = thisUser["username"] as string;
            string thisDetector = detectorInit["name"] as string;
            string thisValue = detectorInit["value"] as string;

            string testOutput = string.Format("User: {0} {1} ({2}), \n Detector: {3}, \n Value: {4}", thisUserFirstName, thisUserLastName, thisUserUserName, thisDetector, thisValue);
            //Debug.Log(testOutput);

            GetComponent<TextMesh>().text = testOutput;


            DataPiece info;
            if (SavedData.data.get(thisUserUserName, out info))
            {
                //updating existing students info
                if (info.detectors == null)
                   Debug.LogError("Dictionary of detectors for this student has not been intialized");

                //update the value assocated with this detector. you rewrite the previous value.
                info.detectors.Remove(thisDetector);
                info.detectors.Add(thisDetector, thisValue);

                //string debug;
                //info.detectors.TryGetValue(thisDetector, out debug);
                //Debug.Log(thisDetector + " " + debug);

                //if (info.id.EndsWith("08"))
                //    Debug.Log("AHHHHHHHHHHHHHHHHH");
                //if (info.id.EndsWith("08"))
                //    Debug.Log(thisDetector + " " + debug);

                IndicatorControl.updateStudentIndicator(thisDetector, info);
            }
            else
            {
                //adding a new student
                info = new DataPiece();
                info.datatype = "student";
                info.firstName = thisUserFirstName;
                info.lastName = thisUserLastName;
                info.id = thisUserUserName;

                info.detectors = new Dictionary<string, string>();
                info.detectors.Add(thisDetector, thisValue);

                IndicatorControl.createStudentIndicator(info);

                SavedData.data.add(thisUserUserName, info);

                Debug.Log("CREATED NEW STUDENT: " + thisUserUserName);
            }

        }

        SavedData.data.Save();
    }

    // event handler
    void OnSSError(Socket socket, Packet packet, params object[] args)
    {
        Debug.Log(string.Format("{0}", args[0]));

    }

    // event handler
    void OnMessage(Socket socket, Packet packet, params object[] args)
    {
        // args[0] is the nick of the sender
        // args[1] is the message
        Debug.Log(string.Format("Message from {0}: {1}", args[0], args[1]));
    }


    void OnDestroy()
    {
        manager.Close();
    }

    void OnServerConnect(Socket socket, Packet packet, params object[] args)
    {
        Debug.Log("Connected");

        //WWWForm classIDJSON = new WWWForm();
        //classIDJSON.AddField("classId", 4);

        manager.Socket.Emit("init", 4 );

    }

    void OnServerDisconnect(Socket socket, Packet packet, params object[] args)
    {
        Debug.Log("Disconnected");
    }

    void OnError(Socket socket, Packet packet, params object[] args)
    {
        Error error = args[0] as Error;

        switch (error.Code)
        {
            case SocketIOErrors.User:
                Debug.LogWarning("Exception in an event handler!");
                break;
            case SocketIOErrors.Internal:
                Debug.LogWarning("Internal error!");
                break;
            default:
                Debug.LogWarning("server error!");
                break;
        }
    }

    void OnReconnect(Socket socket, Packet packet, params object[] args)
    {
        Debug.Log("Reconnected");
    }

    void OnReconnecting(Socket socket, Packet packet, params object[] args)
    {
        Debug.Log("Reconnecting");
    }

    void OnReconnectAttempt(Socket socket, Packet packet, params object[] args)
    {
        Debug.Log("ReconnectAttempt");
    }

    void OnReconnectFailed(Socket socket, Packet packet, params object[] args)
    {
        Debug.Log("ReconnectFailed");
    }
}
