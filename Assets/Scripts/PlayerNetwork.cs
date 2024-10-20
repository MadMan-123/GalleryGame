using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{

    public bool serverAuthority = false;
    private NetworkVariable<PlayerData> _state;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();    
        var perm = serverAuthority ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        _state = new NetworkVariable<PlayerData>(writePerm: perm);
    }

    private void Update()
    {
        if (IsOwner) TransmitState();
        else ConsumeState();
    }

    [ServerRpc]
    private void TransmitStateServerRpc(PlayerData state)
    {
        _state.Value = state;
    }
    void TransmitState()
    {
        var state = new PlayerData 
        {
            Position = _rb.position,
            Rotation = transform.rotation
        };

        if (IsServer || !serverAuthority)
            _state.Value = state;
        else
            TransmitStateServerRpc(state); 
    }

    void ConsumeState()
    {
       _rb.MovePosition(_state.Value.Position); 
       
       transform.rotation = _state.Value.Rotation;
    }
    struct PlayerData : INetworkSerializable{
        public Vector3 Position;
        public Quaternion Rotation;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Position);
            serializer.SerializeValue(ref Rotation);
        }
    }
}
