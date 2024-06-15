using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            Respawn respawn = FindObjectOfType<Respawn>();
            if (respawn != null)
            {
                respawn.SetCheckpoint(transform.position);
                isActivated = true;
            }
        }
    }
}