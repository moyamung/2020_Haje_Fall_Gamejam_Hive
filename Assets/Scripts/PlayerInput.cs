using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update

    float scrollSpeed = 5f;
    Vector3 MoveStart;
    Vector3 Move;
    public GameObject selected;

    public GameObject popUpMenu;

    void Start()
    {
        popUpMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Click();
        Zoom();
        Panning();
        MoveCommand();
        KeyboardInput();
    }

    void Click()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //popUpMenu.SetActive(false);

            Vector3 mousePositon = Input.mousePosition;
            mousePositon = Camera.main.ScreenToWorldPoint(mousePositon);

            RaycastHit2D hit = Physics2D.Raycast(mousePositon, transform.forward, 100f);
            if (hit)
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.CompareTag("Button"))
                {
                    hit.transform.GetComponent<Button>().OnClick();
                    Deselect();
                }
                else if (hit.transform.CompareTag("HoneycombCell"))
                {
                    Select(hit.transform.gameObject);
                    PopUpMenu();
                    
                }
                else if (hit.transform.CompareTag("Controllable"))
                {
                    //Debug.Log("asdf");
                    Select(hit.transform.gameObject);
                }
            }
            else
            {
                Deselect();
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
            if (selected.CompareTag("Controllable"))
            {
                Vector3 mousePositon = Input.mousePosition;
                mousePositon = Camera.main.ScreenToWorldPoint(mousePositon);

                RaycastHit2D hit = Physics2D.Raycast(mousePositon, transform.forward, 100f);
                if (hit)
                {
                    if (hit.transform.CompareTag("Flower") && selected.GetComponent<WorkerBee>()!=null)
                    {
                        selected.GetComponent<WorkerBee>().GatherFrom(hit.transform);
                    }
                }

                selected.GetComponent<Bee>().MoveTo(mousePositon-Camera.main.transform.position);
            }
        }
    }

    void PopUpMenu()
    {
        popUpMenu.SetActive(true);
        //Vector3 mousePositon = Input.mousePosition;
        //mousePositon = Camera.main.ScreenToWorldPoint(mousePositon);
        //popUpMenu.transform.position = mousePositon - Camera.main.transform.position;
    }

    public void MakeLarva(HoneycombCell cell)
    {
        HoneycombCell chosenCell = cell;
        if (chosenCell == null) return;
        if (!chosenCell.containLarva)
        {
            chosenCell.AddLarva();
        }
    }

    void Select(GameObject _selected)
    {
        if (selected != null) Deselect();
        selected = _selected;
        if (selected.GetComponent<SpriteRenderer>() != null)
        {
            selected.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void Deselect()
    {
        if (selected == null) return;
        if (selected.GetComponent<SpriteRenderer>() != null)
        {
            selected.GetComponent<SpriteRenderer>().color = Color.white;
        }
        selected = null;
    }

    void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (selected.GetComponent<HoneycombCell>() != null) MakeLarva(selected.GetComponent<HoneycombCell>());
        }
    }
}
