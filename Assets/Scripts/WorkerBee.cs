using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBee : Bee
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Move(Vector3 arrival)
    {
        this.transform.position = arrival;
    }
}
