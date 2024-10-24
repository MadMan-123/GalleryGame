using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class InteractManager : NetworkBehaviour 
{
    [SerializeField] private Transform InteractOrigin;
    [SerializeField] private float interactDistance = 1f;
    [SerializeField] private Image crosshair;
    [SerializeField] private Color onHoverColor = Color.green;
    [SerializeField] private Color defaultColor = new Color(0,0,0,0);
    [SerializeField] private Color canDamageColor = Color.red;
    Vector3 hitPosition;
    [SerializeField] LayerMask dontHit;    
    [SerializeField] private RaycastHit[] hits = new RaycastHit[10];
    
    int hitCount;
    
    private Interactable interactableCache;
    
   
    public bool ShouldDebug = false;

    public float InteractRadius = 0.5f; 
    void Update()
    {
        if (isLocalPlayer)
        {
            CheckForInteractable();

            //if e is pressed, try to interact with the object
            if (Input.GetKeyDown(KeyCode.E))
            {
                var interactable = GetInteractableFromRaycast();

                if (interactable)
                {
                    
                    CmdTryInteract(interactable.netId);
                    if (interactable.HasItem)
                    {
                        GameObject go = gameObject;
                        //try to add the item to the inventory
                        //interactable.ConnectedItem.PickUp(ref go);
                        return;
                    }

                }
            }
        }
    }

    [Command]
    private void CmdTryInteract(uint id)
    {
        //todo: fix how we pass data to the server
        var identity = NetworkIdentity.GetSceneIdentity(id);
        
        

        if (!identity)
        {
            Debug.LogError(("Identity was not found for id: [" + id + "]"));
            return;
        }

        if (identity.gameObject.TryGetComponent(out Interactable interactable))
        {
            interactable.Interact(gameObject);
        }
    }

    private Interactable GetInteractableFromRaycast()
    {
        hitCount = Physics.SphereCastNonAlloc(
            InteractOrigin.position,
            InteractRadius,
            InteractOrigin.forward,
            hits,
            interactDistance,
            ~dontHit
        ); 
        //get the first hit interactable object
        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.TryGetComponent(out Interactable interactable))
            {
                return interactable;
            }
        } 
        
        //nothing found
        return null;
    }
    
    private void CheckForInteractable()
    {
        // Casting ray
        hitCount = (Physics.SphereCastNonAlloc(
            InteractOrigin.position,
            InteractRadius ,
            InteractOrigin.forward, 
            hits, 
            interactDistance,
            ~dontHit
            )
            );
        //do a sphere cast to check if the player is looking at an interactable object
        bool foundInteractable = false;
         if(hitCount > 0) 
         {
             //for loop to find the first interactable object
             for(int i = 0; i < hitCount; i++)
             {
                 if (hits[i].collider.TryGetComponent(out interactableCache))
                 {
                     //set the crosshair color to the onHoverColor
                     foundInteractable = true;
                     hitPosition = hits[i].point;
                     
                     if (ShouldDebug)
                         Debug.Log($"Hit: {hits[i].collider.gameObject.name}");
                     break;
                 }
             }
        }

        if (crosshair)
        {
            crosshair.color = foundInteractable ? onHoverColor : defaultColor;
        } 
    }
    //draw the ray in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(InteractOrigin.position, InteractOrigin.forward * interactDistance);
        //draw the sphere cast
        Gizmos.DrawWireSphere(InteractOrigin.position + InteractOrigin.forward * interactDistance, InteractRadius); 
        
        if(hitPosition != Vector3.zero)
            Gizmos.DrawSphere(hitPosition, 0.1f);
    }
}
