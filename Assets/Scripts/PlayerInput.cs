using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update

    float scrollSpeed = 5f;
    Vector3 MoveStart;
    Vector3 Move;
    GameObject chosen;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Click();
        Zoom();
        Panning();
        MoveCommand();
    }

    void Click()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePositon = Input.mousePosition;
            mousePositon = Camera.main.ScreenToWorldPoint(mousePositon);

            RaycastHit2D hit = Physics2D.Raycast(mousePositon, transform.forward, 100f);
            if (hit)
            {
                if (hit.transform.CompareTag("Button"))
                {
                    //Debug.Log(hit.transform.name);
                    hit.transform.GetComponent<Button>().OnClick();
                    chosen = null;
                }
                else if (hit.transform.CompareTag("Controllable"))
                {
                    //Debug.Log("asdf");
                    chosen = hit.transform.gameObject;
                }
            } else
            {
                chosen = null;
            }
        }
    }

    void Zoom()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        //Camera.main.transform.Translate(new Vector3(0f, 0f, -wheelInput * scrollSpeed));
        Camera.main.orthographicSize -= wheelInput * scrollSpeed;
    }

    void Panning()
    {
        if (Input.GetMouseButtonDown(2))
        {
            MoveStart = Input.mousePosition;
            MoveStart = Camera.main.ScreenToWorldPoint(MoveStart);
        }
        else if (Input.GetMouseButton(2))
        {
            Move = Input.mousePosition;
            Move = Camera.main.ScreenToWorldPoint(Move);
            Camera.main.transform.position -= Move - MoveStart;
        }
    }

    void MoveCommand()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (chosen != null)
            {
                Vector3 mousePositon = Input.mousePosition;
                mousePositon = Camera.main.ScreenToWorldPoint(mousePositon);
                chosen.GetComponent<Bee>().MoveTo(mousePositon-Camera.main.transform.position);
            }
        }
    }
}
