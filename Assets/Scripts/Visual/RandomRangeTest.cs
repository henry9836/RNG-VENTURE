﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRangeTest : MonoBehaviour
{

    public GameObject testObj;
    public int amountToSpawn = 10000;
    public float rangeToSpawn = 50.0f;


    public List<Vector3> spawns = new List<Vector3>();

    private float v1 = 0.0f;
    private float v2 = 0.0f;
    private float v3 = 0.0f;

    public enum method
    {
        LCG,
        URANIUM,
        MOUSE,
    }

    public method chosen;



    // Start is called before the first frame update
    void Start()
    {
        if (chosen == method.LCG)
        {
            LCG();
        }
        else if (chosen == method.URANIUM)
        {
            uranium();
        }
        else if (chosen == method.MOUSE)
        {
            mouse();
        }

    }


    private void LCG()
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


        StartCoroutine(SpawnObjects());

    }

    private void uranium()
    {
        //TODO add code
    }

    private void mouse()
    {
        StartCoroutine(mousewig());
        StartCoroutine(SpawnObjects());

    }

    void Number(float tmp)
    {
        if (v1 == 0.0f)
        {
            v1 = tmp;
        }
        else if (v2 == 0.0f)
        {
            v2 = tmp;
        }
        else if (v3 == 0.0f)
        {
            v3 = tmp;
        }
    }
 

    public IEnumerator mousewig()
    {
        bool play = true;

        int count = 0;


        while (play == true)
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            float tmp = 0.0f;


            if (x != 0.0f && y != 0.0f)
            {
                tmp = x * y;
                tmp = tmp % 10.0f;
                count++;
                Number(tmp);

            }
            else if (x != 0.0f)
            {
                tmp = x;
                tmp = tmp % 10.0f;
                count++;
                Number(tmp);

            }
            else if (y != 0.0f)
            {
                tmp = y;
                tmp = tmp % 10.0f;
                count++;
                Number(tmp);
            }


            if (count == 3)
            {
                count = 0;
                Instantiate(testObj, new Vector3(v1, v2, v3), Quaternion.identity);
                v1 = 0.0f;
                v2 = 0.0f;
                v3 = 0.0f;
            }


            yield return null;
        }

        yield return null;
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
