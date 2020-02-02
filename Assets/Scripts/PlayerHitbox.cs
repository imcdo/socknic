using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public float indicatorLifetime = 0.1f;

    private IEnumerator indicatorRoutine;
    
    public void TryHit()
    {
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

        foreach (Collider2D collider in overlappingColliders)
        {
            SockNote note = collider.gameObject.GetComponent<SockNote>();
            if (note != null)
            {
                note.Hit();
            }
        }
    }

    IEnumerator HideIndicator()
    {
        yield return new WaitForSeconds(indicatorLifetime);
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
