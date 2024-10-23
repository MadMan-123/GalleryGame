using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Interactable : NetworkBehaviour 
{
    public UnityEvent<GameObject> OnInteract;
    public Item ConnectedItem;
    [SyncVar] public bool HasItem = false;
    public GameObject InteractSource;
    public bool debug = false;
    private void Start()
    {
        HasItem = TryGetComponent(out ConnectedItem);
    }
    
    
    public void Interact(GameObject Source)
    {
        if(!NetworkServer.active) return;
        
        
        InteractSource = Source;
        
        
        if (OnInteract != null)
        {
 
            
            #if UNITY_EDITOR
            //check if the index is valid
            if (OnInteract.GetPersistentEventCount() == 0)
            {
                Debug.LogWarning("No Method Set for Interact Event");
                return;
            }
            var str = OnInteract.GetPersistentMethodName(0);
            if (str == "")
            {
                Debug.LogWarning("No Method Set for Interact Event");
                return;
            }

            if(debug)
                Debug.Log("Interacted with " + OnInteract.GetPersistentMethodName(0) + " on " + gameObject.name + " from " + Source.name);
            #endif

            OnInteract?.Invoke(Source);

            OnInteractRpc(Source);

        }
        else if(debug)
            Debug.LogWarning("No Interact Event Set for interaction  ");
    }
    
    [ClientRpc]
    private void OnInteractRpc(GameObject Source)
    {
        
            OnInteract?.Invoke(Source);
    }
}

