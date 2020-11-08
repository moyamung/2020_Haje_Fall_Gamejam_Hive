using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogUpdater : MonoBehaviour
{
    public FogTexture[] fogs;

    // Start is called before the first frame update
    void Start()
    {
        fogs = FindObjectsOfType<FogTexture>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var fog in fogs) {
            fog.UpdateAt(transform.position, 4);
        }
    }
}
