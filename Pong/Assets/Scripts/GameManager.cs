using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEditor.PackageManager;
public class GameManager : NetworkBehaviour
{
    [SerializeField] private NetworkObject LPaddle;
    [SerializeField] private NetworkObject RPaddle;

    void Start()
    {
        if (IsServer)
        {
            StartCoroutine(AssignOwnershipAfterSpawn());
        }
    }

    private IEnumerator AssignOwnershipAfterSpawn()
    {
        yield return new WaitForSeconds(1f);

        if (LPaddle != null && LPaddle.IsSpawned)
        {
            LPaddle.ChangeOwnership(NetworkManager.ServerClientId);
            Debug.Log("Assigned LPaddle to host");
        }
        else
        {
            Debug.Log("Error, LPaddle not spawned");    
        }
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }

        private void OnClientConnected(ulong clientId)
    {
        if (!IsServer) return;

        if (clientId == NetworkManager.ServerClientId) return;
        Debug.Log($"Client {clientId} connected, assigning RPaddle");

        StartCoroutine(AssignRPaddleToClient(clientId));
    }

    private IEnumerator AssignRPaddleToClient(ulong clientId)
    {
        yield return new WaitForSeconds(0.5f);

        if (RPaddle != null && RPaddle.IsSpawned)
        {
            RPaddle.ChangeOwnership(clientId);
            Debug.Log("Assigned RPaddle to client"); 
            }
            else
            {
                Debug.Log("Error, RPaddle not assigned.");
        }
                
    }
}
