using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ManagementServerManager : MonoBehaviour
{
    private const string SERVER_URL = "http://localhost:7000";
    private const float PING_INTERVAL = 1f;

    public int LatestPort { get; private set; }

    private static ManagementServerManager instance;
    public static ManagementServerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ManagementServerManager>();
            }
            return instance;
        }
    }

    public List<GameServerObject> gameServers = new List<GameServerObject>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Keep the object alive between scenes
        DontDestroyOnLoad(gameObject);
        // Start pinging the server
        StartCoroutine(PingServerRepeatedly());

        StartCoroutine(GetServersCoroutine());
    }

    private IEnumerator PingServerRepeatedly()
    {
        while (true)
        {
            // Send a ping to the server
            StartCoroutine(PingServerCoroutine());

            yield return new WaitForSeconds(PING_INTERVAL);
        }
    }

    private IEnumerator PingServerCoroutine()
    {
        string url = SERVER_URL + "/ping/";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // no error
        }
    }

    private IEnumerator GetServersCoroutine()
    {
        string url = SERVER_URL + "/servers/";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {

            string json = request.downloadHandler.text;

            Debug.Log(json);
            GameServerObjectWrapper wrapper = JsonUtility.FromJson<GameServerObjectWrapper>("{\"servers\":" + json + "}");
            GameServerObject[] servers = wrapper.servers;
            gameServers.Clear();
            gameServers.AddRange(servers);
        }
    }

    public void JoinGame(int port)
    {
        Debug.Log("Joining game on port " + port);
        LatestPort = port;
        SceneManager.LoadScene(1);
    }
}

[System.Serializable]
public struct GameServerObject
{
    public string Name;
    public string Adress;
    public int Port;
    public int PlayerCount;
}

[System.Serializable]
public class GameServerObjectWrapper
{
    public GameServerObject[] servers;
}