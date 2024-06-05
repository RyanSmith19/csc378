using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropper : MonoBehaviour
{
    public GameObject ballPrefab;
    public float spawnFrequency = 5.0f;
    private float timer;

    public int time_til_destruction;

    // Start is called before the first frame update
    void Start()
    {
        timer = spawnFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f) 
        {
            GameObject go = Instantiate(ballPrefab, transform.position, transform.rotation);

            timer = spawnFrequency;

            StartCoroutine(SelfDestruct(go));
        }
    }

    IEnumerator SelfDestruct(GameObject go)
    {
        yield return new WaitForSeconds(time_til_destruction);
        Destroy(go);
    }
}