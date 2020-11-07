using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBee : Bee
{
    // Start is called before the first frame update
    float speed = 5f;
    Vector3 arrivalPoint;
    bool isMoving;
    void Start()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) Move();
    }

    void Move()
    {
        if ((transform.position - arrivalPoint).magnitude < speed * Time.deltaTime)
        {
            transform.position = arrivalPoint;
            isMoving = false;
        }
        else
        {
            Vector3 direction = arrivalPoint - transform.position;
            direction = direction.normalized * speed;
            transform.Translate(direction * Time.deltaTime);
        }
    }

    public override void MoveTo(Vector3 arrival)
    {
        arrivalPoint = arrival;
        isMoving = true;
    }
}
