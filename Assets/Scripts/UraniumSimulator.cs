using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UraniumSimulator : MonoBehaviour
{

    public float lastSeenDecayDelay = 0.0f;
    [Range(1,31)]
    public int binaryLength = 31;
    [Range(0.00001f, 1.0f)]
    public float timeBeforeDecay = 0.024f;

    private int currentSeedingValue = 1;
    private string seedingString = "";
    private bool decayFlag = false;
    private float decayTimer = 0.0f;

    public int readUranium()
    {
        return currentSeedingValue;
    }

    // Update is called once per frame
    void Update()
    {
        decayTimer += Time.deltaTime;
        //Flip the sign
        decayFlag = !decayFlag;
        //Have we decayed long enough?
        if (decayTimer >= timeBeforeDecay) {
            //If we decay this frame
            int coin = UnityEngine.Random.Range(0, 10);
            if (coin == 6)
            {
                //Debug.Log("Decay Event");
                seedingString += Convert.ToInt32(decayFlag);

                //Check if our string is the binary length
                if (seedingString.Length == binaryLength)
                {
                    currentSeedingValue = Convert.ToInt32(seedingString, 2);
                    if (currentSeedingValue < 0.0f)
                    {
                        currentSeedingValue *= -1;
                    }
                    seedingString = "";
                }
                decayTimer = 0.0f;
            }
        }
    }
}
