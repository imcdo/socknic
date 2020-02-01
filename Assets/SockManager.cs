using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockManager : MonoBehaviour
{
    public List<Sock> sockCatalog;

    private List<Sock> availableSocks;

    // Refill availableSocks with all the possible socks
    public void ReadyAllSocks()
    {
        availableSocks.Clear();
        availableSocks.AddRange(sockCatalog);
    }

    public Sock GetRandomReadySock()
    {
        // First make sure there's availableSocks
        if (availableSocks.Count == 0)
        {
            ReadyAllSocks();
        }
        
        int randomSockIdx = Random.Range(0, availableSocks.Count);
        Sock sock = availableSocks[randomSockIdx];
        availableSocks.Remove(sock);

        return sock;
    }
}

[System.Serializable]

[CreateAssetMenu(fileName = "song config", menuName = "socknic/Sock")]
public class Sock : ScriptableObject
{
    public string id;
    public Sprite sprite;
}