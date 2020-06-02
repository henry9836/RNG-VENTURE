using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRangeTest : MonoBehaviour
{
    public GameObject testObj;
    public UraniumSimulator uraniumSim;
    public float rangeToSpawn = 50.0f;

    public List<Vector3> spawns = new List<Vector3>();

    private float v1 = 0.0f;
    private float v2 = 0.0f;
    private float v3 = 0.0f;

    public int count = 0;
    public float timetaken = 0.0f;

    long nanosecondspertick;


    public enum method
    {
        LCG,
        MOUSE,
        MOUSEANDTIME,
        MERSENNETWISTER,
        MERSENNETWISTERANDURANIUM,
        URANIUMLCG,
    }

    public method chosen;
    private void Awake()
    {
        nanosecondspertick = (1000L * 1000L * 1000L) / System.Diagnostics.Stopwatch.Frequency;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ScreenCapture.CaptureScreenshot("Doc/"+ chosen.ToString() + ".png");
            Debug.Log("Saved Screenshot");
        }
    }

    #region init
    void Start()
    {
        switch (chosen)
        {
            case method.LCG:
                {
                    LCG();
                    break;
                }
            case method.URANIUMLCG:
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
            case method.MERSENNETWISTER:
                {
                    MersenneTwistercall();
                    break;
                }
            case method.MERSENNETWISTERANDURANIUM:
                {
                    twisteranduranium();
                    break;
                }
            default:
                break;
        }
    }

    private void twisteranduranium()
    {
        StartCoroutine(uraniumandtwist());
    }
    private void LCG()
    {
        StartCoroutine(LCGtimer());
    }
    private void uranium()
    {
        StartCoroutine(readDatUranium());
    }
    private void mouse()
    {
        StartCoroutine(mousewig());
    }
    private void mouseTime()
    {
        StartCoroutine(mousewigTime());
    }
    private void MersenneTwistercall()
    {
        StartCoroutine(twist());
    }

    #endregion

    public float nfmod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }
    public void Number(float tmp)
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
        int thecount = 0;

        while (true)
        {
            timetaken += Time.deltaTime;

            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            float tmp = 0.0f;

            if (x != 0.0f || y != 0.0f)
            {
                tmp = x + y;

                if (tmp > 10.0f || tmp < -10.0f)
                {
                    tmp = nfmod(tmp, 10.0f);
                    tmp /= 10.0f;
                    tmp *= rangeToSpawn;
                    tmp -= rangeToSpawn / 2.0f;
                    thecount++;
                    Number(tmp);
                }
            }

            if (thecount == 3)
            {
                count++;
                thecount = 0;
                spawns.Add(new Vector3(v1, v2, v3 ));
                v1 = 0.0f;
                v2 = 0.0f;
                v3 = 0.0f;
            }


            yield return null;
        }

    }

    public IEnumerator mousewigTime()
    {
        int thecount = 0;

        while (true)
        {
            timetaken += Time.deltaTime;

            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            float tmp = 0.0f;

            if (x != 0.0f || y != 0.0f)
            {
                tmp = x + y;

                if (tmp > 10.0f || tmp < -10.0f)
                {
                    tmp *= System.DateTime.Now.Millisecond;
                    tmp = nfmod(tmp, 10.0f);
                    tmp /= 10.0f;
                    tmp *= rangeToSpawn;
                    tmp -= rangeToSpawn / 2.0f;
                    thecount++;
                    Number(tmp);
                }
            }


            if (thecount == 3)
            {
                count++;
                thecount = 0; 
                spawns.Add(new Vector3(v1, v2, v3));
                v1 = 0.0f;
                v2 = 0.0f;
                v3 = 0.0f;
            }

            yield return null;
        }
    }

    public IEnumerator readDatUranium()
    {
        float timeToRando = uraniumSim.timeBeforeDecay / 3.0f;
        float timer = 0.0f;

        while (true)
        {
            timetaken += Time.deltaTime;
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
                v1 -= rangeToSpawn/2.0f;

                seed = (a * seed * c) % m;

                v2 = seed / m;
                v2 *= rangeToSpawn;
                v2 -= rangeToSpawn/2.0f;


                seed = (a * seed * c) % m;

                v3 = seed / m;
                v3 *= rangeToSpawn;
                v3 -= rangeToSpawn/2.0f;

                spawns.Add(new Vector3((float)v1, (float)v2, (float)v3));
                count++;


                timer = 0.0f;
            }

            yield return null;
        }

    }

    public IEnumerator twist()
    {
        MersenneTwister MT = this.GetComponent<MersenneTwister>();
        long ticktotal = 0;


        while (true)
        {
            var tmptimer = System.Diagnostics.Stopwatch.StartNew();

            double v1 = MT.genrand_res53();
            v1 *= rangeToSpawn;
            v1 -= rangeToSpawn / 2.0f;
            double v2 = MT.genrand_res53();
            v2 *= rangeToSpawn;
            v2 -= rangeToSpawn / 2.0f;
            double v3 = MT.genrand_res53();
            v3 *= rangeToSpawn;
            v3 -= rangeToSpawn / 2.0f;
            spawns.Add(new Vector3((float)v1, (float)v2, (float)v3));
            count++;

            tmptimer.Stop();
            if (count > 10 && count < 5000)
            {
                ticktotal += tmptimer.ElapsedTicks;
                Debug.Log((ticktotal * nanosecondspertick) / count);
            }
            tmptimer.Reset();

            yield return null;
        }
    }

    public IEnumerator LCGtimer()
    {
        double seed = 1234;
        long ticktotal = 0;


        double a = 65539;
        double c = 1;
        double m = 2147483648;

        double v1 = 0.0;
        double v2 = 0.0;
        double v3 = 0.0;

        while (true)
        {
            var tmptimer = System.Diagnostics.Stopwatch.StartNew();


            seed = (a * seed * c) % m;

            v1 = seed / m;
            v1 *= rangeToSpawn;
            v1 -= rangeToSpawn / 2.0f;

            seed = (a * seed * c) % m;

            v2 = seed / m;
            v2 *= rangeToSpawn;
            v2 -= rangeToSpawn / 2.0f;


            seed = (a * seed * c) % m;

            v3 = seed / m;
            v3 *= rangeToSpawn;
            v3 -= rangeToSpawn / 2.0f;

            spawns.Add(new Vector3((float)v1, (float)v2, (float)v3));
            count++;

            tmptimer.Stop();
            if (count > 10 && count < 5000)
            {
                ticktotal += tmptimer.ElapsedTicks;

               // long avgnanno = (ticktotal * nanosecondspertick) / count;
                Debug.Log((ticktotal * nanosecondspertick) / count);
            }
            tmptimer.Reset();

            yield return null;
        }
    }

    public IEnumerator uraniumandtwist()
    {
        float timeToRando = uraniumSim.timeBeforeDecay / 3.0f;
        float timer = 0.0f;
        long ticktotal = 0;

        MersenneTwister MT = this.GetComponent<MersenneTwister>();


        while (true)
        {
            timer += Time.deltaTime;

            if ((timer >= timeToRando) && (uraniumSim.readUranium() != 1))
            {

                var tmptimer = System.Diagnostics.Stopwatch.StartNew();

                float seed = (float)uraniumSim.readUranium() * (float)System.DateTime.Now.Millisecond;
                MT.reseed(Mathf.Abs(seed));
                timer = 0.0f;

                double v1 = MT.genrand_res53();
                v1 *= rangeToSpawn;
                v1 -= rangeToSpawn / 2.0f;
                double v2 = MT.genrand_res53();
                v2 *= rangeToSpawn;
                v2 -= rangeToSpawn / 2.0f;
                double v3 = MT.genrand_res53();
                v3 *= rangeToSpawn;
                v3 -= rangeToSpawn / 2.0f;
                spawns.Add(new Vector3((float)v1, (float)v2, (float)v3));
                count++;

                tmptimer.Stop();
                if (count > 10 && count < 5000)
                {
                    ticktotal += tmptimer.ElapsedTicks;
                    Debug.Log((ticktotal * nanosecondspertick) / count);
                }
                tmptimer.Reset();
            }

            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawCube(spawns[i], new Vector3(0.5f, 0.5f, 0.5f));
        }
    }
}
