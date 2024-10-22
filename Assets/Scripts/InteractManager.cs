using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
    
    
    void Update()
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
             
        
         if(hitCount > 0) 
         {
             //for loop to find the first interactable object
             
             for(int i = 0; i < hitCount; i++)
             {
                 if (hits[i].collider.TryGetComponent(out interactableCache))
                 {
                     //set the crosshair color to the onHoverColor
                     crosshair.color = onHoverColor;
                     if (ShouldDebug)
                         Debug.Log($"Hit: {hits[i].collider.gameObject.name}");
                     hitPosition = hits[0].point;
                     break;
                 }
                 else
                 {
                     crosshair.color = defaultColor;
                 }


             }
             
                
        }
        else
        {
            crosshair.color = defaultColor;
        }
         
        //if e is pressed, try to interact with the object
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractServerRpc();
        }
    }

    public bool ShouldDebug = false;

    public float InteractRadius = 0.5f;
    [ServerRpc]
    public void TryInteractServerRpc()
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
        Interactable interactable = null;
        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.TryGetComponent(out interactable))
            {
                break;
            }
            if(i == hitCount - 1)
            {
                return;
            }
        }


        if (!interactable) return;
        if (!interactable.HasItem)
        {
            interactable.Interact(gameObject);
        }
        else
        {
            GameObject go = gameObject;
            //try to add the item to the inventory
            //interactable.ConnectedItem.PickUp(ref go);
            return;
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
