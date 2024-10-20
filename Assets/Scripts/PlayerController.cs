using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    //idk what to set the values just yet so for now 1 :)
    [SerializeField] private float speed = 1f, mouseSpeed = 1f;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private int angleConstraint = 90;
    //movement data
    private float _horizontalInput, _verticalInput, _mouseX, _mouseY, _verticalRotation;
    private Vector3 _movement = Vector3.zero;


    public override void OnNetworkSpawn()
    {
        //lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        if (!IsOwner)
        { 
            //ensure there is only one camera enabled locally per player
            playerCamera.enabled = false;
            enabled = false;
            
        }
    }

    private void FixedUpdate()
    {
        //if were not the owner we shouldn't perform any actions on this player
        if (IsOwner)
        {
            //move the players position
            MovePlayer(); 
                    
            //rotate the players camera
            RotateCamera();
            
            
            
        }
        
        
        

    }

    private void RotateCamera()
    {
        //record the mouse input
        _mouseX = Input.GetAxis("Mouse X") * mouseSpeed;
        _mouseY = Input.GetAxis("Mouse Y") * mouseSpeed;
        
        
        //rotate around {0,1,0}
        transform.Rotate(Vector3.up, _mouseX);
       
        
        //set the tracked vertical rotation
        _verticalRotation -= _mouseY;
        //clamp between the angle constraint (should be between (-90,90))
        _verticalRotation = Mathf.Clamp(_verticalRotation, -angleConstraint, angleConstraint);
        //set the cameras local rotation based of the vertical rotation¬
        playerCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        
        
    }


    private void MovePlayer()
    {
        //record the horizontal and vertical input
        //todo: Change this so it uses the new Unity Input system, for now a simple controller works
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");  
    
        //if there is no movement detected, surprise surprise don't move
        if(_horizontalInput == 0 && _verticalInput == 0) return;
        
        //calculate the current movement direction
        _movement = transform.right * _horizontalInput + transform.forward * _verticalInput;
        
        
        //apply movement to the transform position
        transform.position += _movement * (speed * Time.deltaTime);
        
        
    }
    
    


    
    
}
