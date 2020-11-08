using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncCamera : MonoBehaviour
{
    public Camera target;
    private Camera cam;
    
    private void Start() {
        cam = GetComponent<Camera>();
    }

    private void OnPreRender() {
        cam.orthographicSize = target.orthographicSize;
    }
}
