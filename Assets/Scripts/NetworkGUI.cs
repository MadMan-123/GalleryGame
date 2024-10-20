using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


//this is for debug purposes
//todo: remove once we have proper UI
public class NetworkGUI : MonoBehaviour
{

    private NetworkManager _networkManager;

    private void Awake()
    {
        _networkManager = NetworkManager.Singleton;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(9, 10, 100, 300));
        if(!_networkManager)
            _networkManager = NetworkManager.Singleton;
            
        if (!_networkManager.IsClient && _networkManager.IsServer) return;

        //host option (server + client)
        if (GUILayout.Button("Host Server")) _networkManager.StartHost();

        //client button
        if (GUILayout.Button("Start Client")) _networkManager.StartClient();

        //Server button
        if (GUILayout.Button("Start Server")) _networkManager.StartServer();
        
        GUILayout.EndArea();
    }
}
