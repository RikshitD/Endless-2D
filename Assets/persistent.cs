using UnityEngine;

public class persistent : MonoBehaviour
{
    void Start()
    {
        // Find GameObject with name "ServerManager"
        GameObject serverManager = GameObject.Find("ServerManager");
        
        if (serverManager != null)
        {
            // Found the ServerManager GameObject
            Debug.Log("Found ServerManager GameObject");
            
            // Disable the ServerManager GameObject (for example)
            serverManager.SetActive(false);
            
            // Log a message to confirm the ServerManager GameObject is disabled
            Debug.Log("ServerManager GameObject has been disabled");
        }
        else
        {
            // ServerManager GameObject not found
            Debug.LogWarning("ServerManager GameObject not found");
        }
    }
}
