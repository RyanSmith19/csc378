using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform respawnPoint; 
    private Vector3 currentCheckpoint;

    void Start()
    {
        // Initialize the current checkpoint to the initial respawn point
        currentCheckpoint = respawnPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -300)
        {
            transform.position = currentCheckpoint;
        }
    }

    // Method to set a new checkpoint
    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    // Method to get the current checkpoint
    public Vector3 GetCurrentCheckpoint()
    {
        return currentCheckpoint;
    }
}
