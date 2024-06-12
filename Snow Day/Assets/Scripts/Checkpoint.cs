using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Respawn respawn = FindObjectOfType<Respawn>();
            if (respawn != null)
            {
                respawn.SetCheckpoint(transform.position);
            }
        }
    }
}