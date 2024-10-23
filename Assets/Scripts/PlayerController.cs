using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerController : NetworkBehaviour
{
    //idk what to set the values just yet so for now 1 :)
    [SerializeField] private float speed = 1f, mouseSpeed = 1f;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private int angleConstraint = 90;
    //movement data
    private float _horizontalInput, _verticalInput, _mouseX, _mouseY, _verticalRotation;
    private Vector3 _movement = Vector3.zero;
    private GameObject cameraRef = null;

    public override void OnStartClient()
    {
        base.OnStartClient();
    
        //lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        if (!isLocalPlayer)
        { 
            //ensure there is only one camera enabled locally per player
            if (playerCamera != null)
            {
                cameraRef = playerCamera.gameObject;
                playerCamera.enabled = false;
            }
            enabled = false;
        }
        else
        {
            // Ensure the camera is assigned for the owner
            if (playerCamera == null)
            {
                playerCamera = GetComponentInChildren<Camera>();

            }

            playerCamera.enabled = true;
                
            
        }
        

    }

    private void Update()
    {

        //if were not the owner we shouldn't perform any actions on this player
        if (isLocalPlayer)
        {
            //move the players position
            MovePlayer(); 
                    
            //rotate the players camera
            RotateCamera();
            
            //if the player presses escape, unlock the cursor
            if (Input.GetKeyDown(KeyCode.Escape))
            { 
                Cursor.lockState = CursorLockMode.None;
            }
           
         
        }
        
        
    }

    private void RotateCamera()
    {
        //record the mouse input
        _mouseX = Input.GetAxis("Mouse X") * mouseSpeed;
        _mouseY = Input.GetAxis("Mouse Y") * mouseSpeed;
        //set the tracked vertical rotation
        _verticalRotation -= _mouseY;
        //clamp between the angle constraint (should be between (-90,90))
        _verticalRotation = Mathf.Clamp(_verticalRotation, -angleConstraint, angleConstraint);
        //set the cameras local rotation based of the vertical rotation
            
        // Update the player's yaw rotation (left-right) locally on the client
        playerCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        

        transform.Rotate(Vector3.up * _mouseX);

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
        var transform1 = transform;
        _movement = transform1.right * _horizontalInput + transform1.forward * _verticalInput;
                
        transform.position += _movement * (speed * Time.deltaTime);
    }
    


    
    
}
