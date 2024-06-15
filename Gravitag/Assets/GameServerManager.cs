using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Assertions;

public class GameServerManager : MonoBehaviour
{
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
        throw new NotImplementedException();
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

        string startupMessage = "";
        startupMessage += "Server started:";
        startupMessage += " - scene " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "\n";
        startupMessage += " - status " + NetworkManager.Singleton.IsServer + "\n";
        startupMessage += " - transport " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name + "\n";
        startupMessage += " - port " + portNumber + "\n";
        startupMessage += " - startup args " + string.Join(" ", args) + "\n";
        Debug.Log(startupMessage);
    }
}