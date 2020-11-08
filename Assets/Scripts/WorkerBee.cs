using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBee : Bee
{
    // Start is called before the first frame update
    float speed = 5f;
    SpriteRenderer spriteRenderer;
    IEnumerator action;

    public float honey;
    public float maxHoney;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Move(Vector3 arrivalPoint)
    {
        Vector3 direction = arrivalPoint - transform.position;
        direction = direction.normalized * speed;
        SpriteFlipping(direction);
        while ((transform.position - arrivalPoint).magnitude > speed * Time.fixedDeltaTime)
        {
            transform.Translate(direction * Time.fixedDeltaTime);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        transform.position = arrivalPoint;
    }

    void SpriteFlipping(Vector3 direction)
    {
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void SpriteDirection(Vector3 direction)
    {
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true;
            transform.rotation = Quaternion.AngleAxis(180 / Mathf.PI * Mathf.Atan2(direction.y, Mathf.Abs(direction.x)),Vector3.forward);
        }
        else
        {
            spriteRenderer.flipX = false;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, Mathf.Abs(direction.x)), Vector3.forward);
        }
    }

    public override void MoveTo(Vector3 arrival)
    {
        if (action != null) StopCoroutine(action);
        action = Move(arrival);
        StartCoroutine(action);
    }

    public void GatherFrom(Transform flower)
    {
        if (action != null) StopCoroutine(action);
        action = Gather(flower);
        StartCoroutine(action);
    }

    IEnumerator Gather(Transform flower)
    {
        Vector3 arrivalPoint = flower.position;
        Vector3 direction = arrivalPoint - transform.position;
        direction = direction.normalized * speed;
        SpriteFlipping(direction);
        while ((transform.position - arrivalPoint).magnitude > speed * Time.fixedDeltaTime)
        {
            transform.Translate(direction * Time.fixedDeltaTime);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        transform.position = arrivalPoint;
        honey = maxHoney;
    }
}
