using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Assertions;

public class GameServerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    void Start()
    {
#if GAMESERVER
        StartServer();
#else
        StartClient();
#endif
    }

    private void StartClient()
    {
        int portNumber = ManagementServerManager.Instance.LatestPort;

        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = (ushort)portNumber;
        NetworkManager.Singleton.StartClient();

    }

    private void StartServer()
    {
        int portNumber = 7777;
        // Check if port was provided as an argument
        string[] args = System.Environment.GetCommandLineArgs();
        if (args.Length > 3 && args[0] != null)
        {
            int.TryParse(args[3], out portNumber); // Parse the port number
        }

        Assert.IsTrue(portNumber > 0, "Invalid port number provided!");
        Assert.IsTrue(portNumber < 65536, "Invalid port number provided!");

        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = (ushort)portNumber;

        NetworkManager.Singleton.StartServer();

        NetworkManager.Singleton.OnServerStarted += () =>
        {
            Debug.Log("Server started");
        };

        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            Debug.Log("Client connected");
            var playerInstance = Instantiate(playerPrefab);
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) =>
        {
            Debug.Log("Client disconnected");
        };
    }
}