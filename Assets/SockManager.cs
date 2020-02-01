using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockManager : Singleton<SockManager>
{
    public List<Sock> sockCatalog;
    
    [Header("Donut touch")]
    // Socks that have not been recently used
    public List<Sock> availableSocks;
    

    // Refill availableSocks with all the possible socks
    private void ReadyAllSocks()
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
        
        int randomSockIdx = Random.Range(0, availableSocks.Count - 1);
        Sock sock = availableSocks[randomSockIdx];
        availableSocks.Remove(sock);

        return sock;
    }
}