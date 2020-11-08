using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallFuncButton : Button
{
    // Start is called before the first frame update

    public string script;
    public string funcName;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnClick()
    {
        Debug.Log("onclick");
        //GameObject.Find(script).SendMessage(funcName);
    }
}
