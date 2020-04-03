using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRangeTest : MonoBehaviour
{

    public GameObject testObj;
    public int amountToSpawn = 10000;
    public float rangeToSpawn = 50.0f;


    public List<Vector3> spawns = new List<Vector3>();



    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log($"B: {Random.state}");
        //Random.InitState(1234);
        double seed = 1234;
        double a = 22695477;
        double c = 1;
        //double m = Mathf.Pow(2, 32);
        double m = 100000;
        double bigboi = 0.0;

        double v1 = 0.0;
        double v2 = 0.0;
        double v3 = 0.0;
        //Debug.Log($"A: {Random.seed}");ssed
        for (int i = 0; i < amountToSpawn; i++)
        {
            seed = (a * seed * c) % m;
            Debug.Log(seed);

            v1 = seed / m;
            v1 *= 100;
            v1 -= 50;

            seed = (a * seed * c) % m;
            Debug.Log(seed);

            v2 = seed / m;
            v2 *= 100;
            v2 -= 50;


            seed = (a * seed * c) % m;
            Debug.Log(seed);

            v3 = seed / m;
            v3 *= 100;
            v3 -= 50;

            spawns.Add(new Vector3((float)v1, (float)v2, (float)v3));
        }
        Debug.Log($"The biggest boi is: {bigboi}");
        StartCoroutine(SpawnObjects());
    }

    IEnumerator GenerateRandomVectors()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            spawns.Add(new Vector3(Random.Range(-rangeToSpawn, rangeToSpawn), Random.Range(-rangeToSpawn, rangeToSpawn), Random.Range(-rangeToSpawn, rangeToSpawn)));
            yield return null;
        }
        StartCoroutine(SpawnObjects());
        yield return null;
    }

    IEnumerator SpawnObjects()
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            Instantiate(testObj, spawns[i], Quaternion.identity);

            yield return null;
        }
        yield return null;
    }

}
