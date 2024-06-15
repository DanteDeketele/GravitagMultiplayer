using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServerPanelUI : MonoBehaviour
{
    [SerializeField] TMP_Text serverNameText;
    [SerializeField] TMP_Text serverPlayersText;

    public int serverPort;

    public void UpdateVisuals(string serverName, int serverPlayers, int port)
    {
        serverNameText.text = serverName;
        serverPlayersText.text = serverPlayers.ToString();
        serverPort = port;
    }

    public void JoinServer()
    {
        ManagementServerManager.Instance.JoinGame(serverPort);
    }
}
