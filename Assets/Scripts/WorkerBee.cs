using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBee : Bee
{
    // Start is called before the first frame update
    float speed = 5f;
    Vector3 arrivalPoint;
    bool isMoving;
    SpriteRenderer renderer;

    void Start()
    {
        isMoving = false;
        renderer = GetComponent<SpriteRenderer>();
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
            SpriteFlipping(direction);
            //SpriteDirection(direction);
        }
    }

    void SpriteFlipping(Vector3 direction)
    {
        if (direction.x > 0)
        {
            renderer.flipX = true;
        }
        else
        {
            renderer.flipX = false;
        }
    }

    void SpriteDirection(Vector3 direction)
    {
        if (direction.x > 0)
        {
            renderer.flipX = true;
            renderer.
            transform.rotation = Quaternion.AngleAxis(180 / Mathf.PI * Mathf.Atan2(direction.y, Mathf.Abs(direction.x)),Vector3.forward);
        }
        else
        {
            renderer.flipX = false;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, Mathf.Abs(direction.x)), Vector3.forward);
        }
    }

    public override void MoveTo(Vector3 arrival)
    {
        arrivalPoint = arrival;
        isMoving = true;
    }
}
