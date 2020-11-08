using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneycombCell : MonoBehaviour
{
    // Start is called before the first frame update
    public float honeyMax;
    public float honeySaved;
    public bool containLarva;

    public SpriteRenderer larva;
    public Transform honeyMask;

    void Start()
    {
        larva = transform.GetChild(0).GetComponent<SpriteRenderer>();
        honeyMask = transform.GetChild(1).GetChild(0);
        LarvaVisualUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        HoneyVisualUpdate();
        //LarvaVisualUpdate();
    }

    void LarvaVisualUpdate()
    {
        larva.enabled = containLarva;
    }

    void HoneyVisualUpdate()
    {
        honeyMask.localScale = new Vector3(1f, honeySaved / honeyMax * 1.1f, 1f);
        honeyMask.localPosition = new Vector3(0f, (honeySaved / honeyMax - 1.0f) * 0.55f);
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

    public void AddLarva()
    {
        StartCoroutine("LarvaMaking");
    }

    IEnumerator LarvaMaking()
    {
        containLarva = true;
        LarvaVisualUpdate();
        yield return new WaitForSeconds(10f);
        containLarva = false;
        LarvaVisualUpdate();
        GameManager.gameManager.AddBee();
    }
}
