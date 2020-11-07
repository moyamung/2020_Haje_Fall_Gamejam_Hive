using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
                }
            }
        }
    }
}
