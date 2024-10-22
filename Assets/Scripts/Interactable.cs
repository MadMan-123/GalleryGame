using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Interactable : MonoBehaviour
{
    public UnityEvent<GameObject> OnInteract;
    public Item ConnectedItem;
    public bool HasItem = false;
    public GameObject InteractSource;
    public bool debug = false;
    private void Start()
    {
        HasItem = TryGetComponent(out ConnectedItem);
    }

    public void Interact(GameObject Source)
    {
        InteractSource = Source;
        if (OnInteract != null)
        {
            OnInteract?.Invoke(Source);
            
            
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
        }
        else if(debug)
            Debug.LogWarning("No Interact Event Set for interaction  ");
    }
}

