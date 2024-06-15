using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject ServerPanelPrefab;
    [SerializeField] TMP_Text ServerAmount;
    [SerializeField] GameObject Viewport;

    public void Start()
    {
        UpdateServerlist();
    }

    public void UpdateServerlist()
    {
        List<GameServerObject> gameServerObjects = ManagementServerManager.Instance.gameServers;

        for (int i = 0; i < gameServerObjects.Count; i++)
        {
            GameObject serverPanel = Instantiate(ServerPanelPrefab, Viewport.transform);
            ServerPanelUI serverPanelUI = serverPanel.GetComponent<ServerPanelUI>();
            serverPanelUI.UpdateVisuals(gameServerObjects[i].Name, gameServerObjects[i].PlayerCount, gameServerObjects[i].Port);
        }

        ServerAmount.text = "Servers: " + gameServerObjects.Count.ToString();
    }
}
