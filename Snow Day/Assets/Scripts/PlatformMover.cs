using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlatformMover : MonoBehaviour
{
    public Transform[] wayPoints;
    public float speed;
    int currentWayPoint = 0;
 
    void Update()
    {
        if (Vector2.Distance(wayPoints[currentWayPoint].position, transform.position) < 0.1f)
        {
            currentWayPoint++;
 
            if(currentWayPoint >= wayPoints.Length)
            {
                currentWayPoint = 0;
            }
        }
    }
 
    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position, wayPoints[currentWayPoint].position, speed * Time.deltaTime));
    }
}
