using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneycombAddButton : Button
{
    int x;
    int y;

    public void set(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public override void OnClick()
    {
        Add();
    }

    public void Add()
    {
        this.transform.parent.GetComponent<Honeycomb>().AddCell(x, y);
    }
}
