using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

public class RandomRangeTest : MonoBehaviour
{

    public GameObject testObj;
    public UraniumSimulator uraniumSim;
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
        MOUSEANDTIME,

    }

    public method chosen;



    // Start is called before the first frame update
    void Start()
    {

        switch (chosen)
        {
            case method.LCG:
                {
                    LCG();
                    break;
                }
            case method.URANIUM:
                {
                    uranium();
                    break;
                }
            case method.MOUSE:
                {
                    mouse();
                    break;
                }
            case method.MOUSEANDTIME:
                {
                    mouseTime();
                    break;
                }
            default:
                break;
        }

    }


    private void LCG()
    {
        double seed = 1234;
        double a = 22695477;
        double c = 1;
        double m = 100000;

        double v1 = 0.0;
        double v2 = 0.0;
        double v3 = 0.0;

        for (int i = 0; i < amountToSpawn; i++)
        {
            seed = (a * seed * c) % m;

            v1 = seed / m;
            v1 *= rangeToSpawn;
            v1 -= rangeToSpawn/2;

            seed = (a * seed * c) % m;

            v2 = seed / m;
            v2 *= rangeToSpawn;
            v2 -= rangeToSpawn/2;


            seed = (a * seed * c) % m;

            v3 = seed / m;
            v3 *= rangeToSpawn;
            v3 -= rangeToSpawn/2;

            spawns.Add(new Vector3((float)v1, (float)v2, (float)v3));
        }
    }


    private void uranium()
    {
        StartCoroutine(readDatUranium());
    }

    private void mouse()
    {
        StartCoroutine(mousewig());
        //StartCoroutine(SpawnObjects());
    }

    private void mouseTime()
    {
        StartCoroutine(mousewigTime());
        //StartCoroutine(SpawnObjects());
    }

    float nfmod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
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

        int count = 0;

        while (true)
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            float tmp = 0.0f;



            if (x != 0.0f && y != 0.0f)
            {
                tmp = x + y;
                if (tmp > 10.0f)
                {
                    tmp = nfmod(tmp, 10.0f);
                    count++;
                    Number(tmp);
                }

            }
            else if (x != 0.0f)
            {
                tmp = x;
                if (tmp > 10.0f)
                {
                    tmp = nfmod(tmp, 10.0f);
                    count++;
                    Number(tmp);
                }

            }
            else if (y != 0.0f)
            {
                tmp = y;
                if (tmp > 10.0f)
                {
                    tmp = nfmod(tmp, 10.0f);
                    count++;
                    Number(tmp);
                }
            }


            if (count == 3)
            {
                count = 0;
                //Instantiate(testObj, new Vector3(Mathf.Abs(v1), Mathf.Abs(v2), Mathf.Abs(v3)), Quaternion.identity);
                spawns.Add(new Vector3(Mathf.Abs(v1), Mathf.Abs(v2), Mathf.Abs(v3)));
                v1 = 0.0f;
                v2 = 0.0f;
                v3 = 0.0f;
            }


            yield return null;
        }

        yield return null;
    }

    public IEnumerator mousewigTime()
    {

        int count = 0;

        while (true)
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            float tmp = 0.0f;



            if (x != 0.0f && y != 0.0f)
            {
                tmp = x + y;
                if (tmp > 10.0f)
                {
                    tmp *= System.DateTime.Now.Millisecond;
                    tmp = nfmod(tmp, 10.0f);
                    count++;
                    Number(tmp);
                }

            }
            else if (x != 0.0f)
            {
                tmp = x;
                if (tmp > 10.0f)
                {
                    tmp *= System.DateTime.Now.Millisecond;
                    tmp = nfmod(tmp, 10.0f);
                    count++;
                    Number(tmp);
                }

            }
            else if (y != 0.0f)
            {
                tmp = y;
                if (tmp > 10.0f)
                {
                    tmp *= System.DateTime.Now.Millisecond;
                    tmp = nfmod(tmp, 10.0f);
                    count++;
                    Number(tmp);
                }
            }


            if (count == 3)
            {
                count = 0; 
                spawns.Add(new Vector3(Mathf.Abs(v1), Mathf.Abs(v2), Mathf.Abs(v3)));
                //Instantiate(testObj, new Vector3(Mathf.Abs(v1), Mathf.Abs(v2), Mathf.Abs(v3)), Quaternion.identity);
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
        //StartCoroutine(SpawnObjects());
        yield return null;
    }

    IEnumerator readDatUranium()
    {
        float timeToRando = uraniumSim.timeBeforeDecay / 3.0f;
        float timer = 0.0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer >= timeToRando && uraniumSim.readUranium() != 1)
            {

                //Get Uranium Value With Other Values
                double seed = uraniumSim.readUranium() * System.DateTime.Now.Millisecond;

                if (seed < 0.0)
                {
                    seed *= -1.0;
                }

                //LCG
                double a = 22695477;
                double c = 1;
                double m = 100000;

                double v1 = 0.0;
                double v2 = 0.0;
                double v3 = 0.0;

                seed = (a * seed * c) % m;

                v1 = seed / m;
                v1 *= rangeToSpawn;
                v1 -= rangeToSpawn/2;

                seed = (a * seed * c) % m;

                v2 = seed / m;
                v2 *= rangeToSpawn;
                v2 -= rangeToSpawn/2;


                seed = (a * seed * c) % m;

                v3 = seed / m;
                v3 *= rangeToSpawn;
                v3 -= rangeToSpawn/2;

                //spawns.Add(new Vector3(Mathf.Abs((float)v1), Mathf.Abs((float)v2), Mathf.Abs((float)v3)));
                spawns.Add(new Vector3((float)v1, (float)v2, (float)v3));

                timer = 0.0f;
            }

            yield return null;
        }

        yield return null;
    }

    //IEnumerator SpawnObjects()
    //{
    //    for (int i = 0; i < spawns.Count; i++)
    //    {
    //        Instantiate(testObj, spawns[i], Quaternion.identity);

    //        yield return null;
    //    }

    //    yield return null;
    //}


    void OnDrawGizmos()
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawCube(spawns[i], new Vector3(1.0f, 1.0f, 1.0f));
        }

    }
}
