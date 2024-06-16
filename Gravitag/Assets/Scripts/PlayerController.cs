using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;

    private NetworkVariable<Vector3> networkedPosition = new NetworkVariable<Vector3>();

    private void Start()
    {
        // Only the server should update the position
        if (IsServer)
        {
            networkedPosition.Value = transform.position;
        }
    }

    private void Update()
    {
        // Only clients send input
        if (IsClient && IsOwner)
        {
            HandleInput();
        }

        // Both clients and server update the position
        UpdatePosition();
    }

    private void HandleInput()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move *= moveSpeed * Time.deltaTime;

        if (move != Vector3.zero)
        {
            SubmitPositionRequestServerRpc(move);
        }
    }

    [ServerRpc]
    private void SubmitPositionRequestServerRpc(Vector3 move, ServerRpcParams rpcParams = default)
    {
        networkedPosition.Value += move;
    }

    private void UpdatePosition()
    {
        if (IsServer)
        {
            // The server directly updates the transform
            transform.position = networkedPosition.Value;
        }
        else
        {
            // Clients interpolate or directly set the position based on the server's state
            transform.position = Vector3.Lerp(transform.position, networkedPosition.Value, Time.deltaTime * 10f);
        }
    }
}
