using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneycombCell : MonoBehaviour
{
    // Start is called before the first frame update
    public float honeyMax;
    public float honeySaved;
    public bool containLarva;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float AddHoney(float honey) // add honey, and return the amount of honey added. it can use for subtracting.
    {
        if (containLarva) return 0;
        if (honeySaved + honey > honeyMax)
        {
            float temp = honeySaved;
            honeySaved = honeyMax;
            return honeyMax - temp;
        }
        honeySaved += honey;
        return honey;
    }
}
