using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
	[SerializeField] public Rigidbody rb;
	[SerializeField] private Quaternion pickupRotation = Quaternion.identity;
	[SerializeField] private Vector3 offset;

	public int type;
	public bool IsPerformingAction { get; set; }

	private void Start()
	{
		//the rigidbody is required for the item to be picked up
		rb = GetComponent<Rigidbody>();
		
	}

	public virtual void Use()
	{
		IsPerformingAction = true;
	}
	
	/*public void PickUp(ref GameObject Source)
	{
		if(!Source.TryGetComponent(out Inventory inventory))
		{
			#if UNITY_EDITOR
				Debug.Log("No Inventory Found on Source");
			#endif
		}
		if (inventory == null)
		{
			#if UNITY_EDITOR
				Debug.LogWarning("No Inventory Found");
			#endif
				return;
		}
		CurrentInventory = inventory;
		//add the item to the inventory
		if (inventory.AddItem(this))
		{
			//set the parent of the item to the inventory ItemHolder
			Transform transform1;
			(transform1 = transform).SetParent(inventory.ItemHolder, true);
		
			//set the local position and rotation to 0 + the offset
			transform1.localPosition = Vector3.zero + offset;
		
			//if the item is a firearm, set the rotation to the quaternion identity
			transform1.localRotation = pickupRotation;	
			
			//play the sound
			
			AudioManager.PlayAudioClip("PickUp",transform.position,1f);
				
			//check if there is an interactable on the game object and call the interact method
		
        		if(TryGetComponent(out Interactable interactable))
        		{
        			interactable.Interact(Source);
        		}	
		}
		
		
		
	}*/
	



}

