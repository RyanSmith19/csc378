using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public int time_til_destruction;
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(time_til_destruction);
        Destroy(gameObject);
    }
}
