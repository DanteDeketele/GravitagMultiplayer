/*#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Renci.SshNet;
using System.IO;

public class WebserverManagementWindow : EditorWindow
{
    private string serverAddress = "your-server-address";
    private string username = "your-username";
    private string password = "your-password";
    private string remoteDirectory = "/path/to/deploy";
    private string localBuildPath = "Path/To/Your/Local/Build";

    [MenuItem("Tools/Server Control")]
    public static void ShowWindow()
    {
        GetWindow<WebserverManagementWindow>("Server Control");
    }

    private void OnGUI()
    {
        GUILayout.Label("Server Connection Settings", EditorStyles.boldLabel);

        serverAddress = EditorGUILayout.TextField("Server Address", serverAddress);
        username = EditorGUILayout.TextField("Username", username);
        password = EditorGUILayout.PasswordField("Password", password);
        remoteDirectory = EditorGUILayout.TextField("Remote Directory", remoteDirectory);
        localBuildPath = EditorGUILayout.TextField("Local Build Path", localBuildPath);

        if (GUILayout.Button("Upload Project"))
        {
            UploadProject();
        }

        if (GUILayout.Button("Reboot Server"))
        {
            RebootServer();
        }
    }

    private void UploadProject()
    {
        using (var client = new SftpClient(serverAddress, username, password))
        {
            try
            {
                client.Connect();
                Debug.Log("Connected to server.");

                var files = Directory.GetFiles(localBuildPath, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var relativePath = file.Substring(localBuildPath.Length + 1).Replace("\\", "/");
                    var remoteFilePath = Path.Combine(remoteDirectory, relativePath).Replace("\\", "/");

                    using (var fileStream = new FileStream(file, FileMode.Open))
                    {
                        client.UploadFile(fileStream, remoteFilePath, true);
                    }
                }

                Debug.Log("Project uploaded successfully.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error uploading project: {ex.Message}");
            }
            finally
            {
                client.Disconnect();
            }
        }
    }

    private void RebootServer()
    {
        using (var client = new SshClient(serverAddress, username, password))
        {
            try
            {
                client.Connect();
                Debug.Log("Connected to server.");

                var command = client.CreateCommand("sudo reboot");
                command.Execute();

                Debug.Log("Server rebooted successfully.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error rebooting server: {ex.Message}");
            }
            finally
            {
                client.Disconnect();
            }
        }
    }
}
#endif*/