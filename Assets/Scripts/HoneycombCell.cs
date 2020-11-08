using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneycombCell : MonoBehaviour
{
    // Start is called before the first frame update
    public float honeyMax;
    public float honeySaved;
    public bool containLarva;

    public SpriteRenderer Larva;
    public Transform honeyMask;

    void Start()
    {
        Larva = transform.GetChild(0).GetComponent<SpriteRenderer>();
        honeyMask = transform.GetChild(1).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        HoneyVisualUpdate();
        LarvaVisualUpdate();
    }

    void LarvaVisualUpdate()
    {
        Larva.enabled = containLarva;
    }

    void HoneyVisualUpdate()
    {
        honeyMask.localPosition = new Vector3(0f, (honeySaved / honeyMax - 1.0f) * 1.1f);
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
        if (honeySaved + honey < 0)
        {
            float temp = honeySaved;
            honeySaved = 0;
            return -temp;
        }
        honeySaved += honey;
        return honey;
    }
}
