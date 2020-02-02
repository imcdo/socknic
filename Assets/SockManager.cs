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

    public PlayerSocks playerOneSocks;
    public PlayerSocks playerTwoSocks;
    
    public void InitSockManager()
    {
        // Init unusedSocks
        availableSocks.AddRange(sockCatalog);

        RhythmManager.Instance.playerOne.UpdateSock();
        RhythmManager.Instance.playerTwo.UpdateSock();
    }

    private Sock GetRandomAvailableSock()
    {
        return availableSocks[Random.Range(0, availableSocks.Count - 1)];
    }

    // Add more socks to both queues
    private void QueueSock(SongProfiler.PlayerNumber playerNumber)
    {
        Sock sock = GetRandomAvailableSock();
        if (playerNumber == SongProfiler.PlayerNumber.Player1)
        {
            playerOneSocks.playerQueue.Add(sock);
            playerOneSocks.rhythmQueue.Add(sock);
        }

        if (playerNumber == SongProfiler.PlayerNumber.Player2)
        {
            playerTwoSocks.playerQueue.Add(sock);
            playerTwoSocks.rhythmQueue.Add(sock);
        }
        availableSocks.Remove(sock);
    }

    // Used to get the next Sock to spawn
    public Sock PopRhythmQueue(SongProfiler.PlayerNumber playerNumber)
    {
        
        Sock sock = null;
        
        if (playerNumber == SongProfiler.PlayerNumber.Player1)
        {
            if (playerOneSocks.rhythmQueue.Count == 0)
            {
                QueueSock(SongProfiler.PlayerNumber.Player1);
            }
            sock = playerOneSocks.rhythmQueue[0];
            playerOneSocks.rhythmQueue.Remove(sock);
        }

        if (playerNumber == SongProfiler.PlayerNumber.Player2)
        {
            if (playerTwoSocks.rhythmQueue.Count == 0)
            {
                QueueSock(SongProfiler.PlayerNumber.Player2);
            }
            sock = playerTwoSocks.rhythmQueue[0];
            playerTwoSocks.rhythmQueue.Remove(sock);
        }

        return sock;
    }

    // Get the first sock of a Player
    public Sock PopPlayerSock(SongProfiler.PlayerNumber playerNumber)
    {
        Sock sock = null;
        if (playerNumber == SongProfiler.PlayerNumber.Player1)
        {
            if (playerOneSocks.playerQueue.Count == 0)
            {
                QueueSock(SongProfiler.PlayerNumber.Player1);
            }
            sock = playerOneSocks.playerQueue[0];
            playerOneSocks.playerQueue.Remove(sock);
        }

        if (playerNumber == SongProfiler.PlayerNumber.Player2)
        {
            if (playerTwoSocks.playerQueue.Count == 0)
            {
                QueueSock(SongProfiler.PlayerNumber.Player2);
                
            }
            sock = playerTwoSocks.playerQueue[0];
            playerTwoSocks.playerQueue.Remove(sock);
        }
        
        availableSocks.Add(sock);
        
        return sock;
    }
}

// NOTE: The Player Queue holds all the notes that the player is ready to display. The Rhythm Queue holds all the notes ready to spawn.
// When a note is popped from the PlayerQueue, that means it has been hit and can be spawned again.
[System.Serializable]
public class PlayerSocks
{
    // What's shown to the Player
    public List<Sock> playerQueue = new List<Sock>();
    // What's given to the RhythmManager
    public List<Sock> rhythmQueue = new List<Sock>();
}