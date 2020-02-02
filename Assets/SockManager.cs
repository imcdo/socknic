using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockManager : Singleton<SockManager>
{
    public List<Sock> sockCatalog;

    public Sock emptySock;
    
    [Header("Donut touch")]
    // Socks that have not been recently used
    public List<Sock> availableSocks;

    public List<Sock> playerOneSockQueue;
    public List<Sock> playerTwoSockQueue;

    void Start()
    {
        // Init unusedSocks
        availableSocks.AddRange(sockCatalog);
        
        // Init player's queue
        for (int i = 0; i < 2; i++)
        {
            QueuePlayerSock(SongProfiler.PlayerNumber.Player1);
            QueuePlayerSock(SongProfiler.PlayerNumber.Player2);
        }
        
        RhythmManager.Instance.playerOne.UpdateSock();
        RhythmManager.Instance.playerTwo.UpdateSock();
    }

    private Sock GetRandomAvailableSock()
    {
        return availableSocks[Random.Range(0, availableSocks.Count - 1)];
    }

    private void QueuePlayerSock(SongProfiler.PlayerNumber playerNumber)
    {
        Sock sock = GetRandomAvailableSock();
        if (playerNumber == SongProfiler.PlayerNumber.Player1)
        {
            playerOneSockQueue.Add(sock);
        }

        if (playerNumber == SongProfiler.PlayerNumber.Player2)
        {
            playerTwoSockQueue.Add(sock);
        }
        availableSocks.Remove(sock);
    }

    // Used to see the first Sock in a player's Queue
    public Sock PeekPlayerSock(SongProfiler.PlayerNumber playerNumber)
    {
        Sock sock = null;
        
        if (playerNumber == SongProfiler.PlayerNumber.Player1)
        {
            sock = playerOneSockQueue[0];
        }

        if (playerNumber == SongProfiler.PlayerNumber.Player2)
        {
            sock = playerTwoSockQueue[0];
        }

        return sock;
    }

    // Get the first sock of a Player, and maintain its size by adding a new one
    public Sock PopPlayerSock(SongProfiler.PlayerNumber playerNumber)
    {
        Sock sock = PeekPlayerSock(playerNumber);
        if (playerNumber == SongProfiler.PlayerNumber.Player1)
        {
            playerOneSockQueue.Remove(sock);
        }

        if (playerNumber == SongProfiler.PlayerNumber.Player2)
        {
            playerTwoSockQueue.Remove(sock);
        }
        
        availableSocks.Add(sock);
        
        QueuePlayerSock(playerNumber);

        return sock;
    }
}