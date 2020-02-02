using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public float indicatorLifetime = 0.1f;
    [SerializeField] private PlayerMovement _playerMovement;

    private IEnumerator indicatorRoutine;
    
    public void TryHit(SongProfiler.PlayerNumber playerNumber)
    {
        Debug.Log("trying to hit something");
        // Show the indicator for a bit
        if (indicatorRoutine != null)
        {
            StopCoroutine(indicatorRoutine);
        }

        indicatorRoutine = HideIndicator();
        StartCoroutine(indicatorRoutine);
        GetComponent<SpriteRenderer>().enabled = true;
        
        // Try and hit overlapping Notes
        List<Collider2D> overlappingColliders = new List<Collider2D>();
        GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(), overlappingColliders);

        bool found = false;
        foreach (Collider2D collider in overlappingColliders)
        {
            SockNote note = collider.gameObject.GetComponent<SockNote>();
            if (note != null)
            {
                note.Hit(playerNumber);
                found = true;
            }
        }
        if (!found) _playerMovement.UpdateScore(-1);
    }

    IEnumerator HideIndicator()
    {
        yield return new WaitForSeconds(indicatorLifetime);
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
