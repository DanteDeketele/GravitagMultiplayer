using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ManagementServerManager : MonoBehaviour
{
    private const string SERVER_URL = "http://localhost:3000";
    private const float PING_INTERVAL = 1f;

    private void Awake()
    {
        // Keep the object alive between scenes
        DontDestroyOnLoad(gameObject);

#if GAMESERVER
        Console.WriteLine("uwu");
        //lets print a lot of info
        string info;
        info = "Unity Version: " + Application.unityVersion + "\n";
        info += "Platform: " + Application.platform + "\n";
        info += "Language: " + Application.systemLanguage + "\n";
        info += "Data Path: " + Application.dataPath + "\n";
        info += "Persistent Data Path: " + Application.persistentDataPath + "\n";
        info += "Temporary Cache Path: " + Application.temporaryCachePath + "\n";
        info += "Cloud Project ID: " + Application.cloudProjectId + "\n";
        info += "Company Name: " + Application.companyName + "\n";
        info += "Product Name: " + Application.productName + "\n";
        info += "Target Frame Rate: " + Application.targetFrameRate + "\n";
        info += "Internet Reachability: " + Application.internetReachability + "\n";

        Console.WriteLine(info);
#else
        // Start pinging the server
        StartCoroutine(PingServerRepeatedly());
#endif
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
}
