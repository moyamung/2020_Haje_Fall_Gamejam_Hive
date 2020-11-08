using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityGridTest : MonoBehaviour
{
    public Grid grid;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log((string.Format("Coord of mouse is [X: {0}, Y: {1}]", pos.x, pos.y)));

            Vector3Int gridPos = grid.WorldToCell(pos);
            Debug.Log((string.Format("Coord of grid is [X: {0}, Y: {1}, Z: {2}]", gridPos.x, gridPos.y, gridPos.z)));
        }
    }
}
