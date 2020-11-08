using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public FogTexture fogTexture;
    public Shader shader;

    public bool degrade;
    public float degradeSpeed;

    private Material mat;
    private void Start() {
        mat = new Material(shader);
    }

    public Bounds GetOrthographicBounds()
    {
        var screenAspect = (float)Screen.width / (float)Screen.height;
        var cameraHeight = Camera.main.orthographicSize * 2;
        var bounds = new Bounds(
            Camera.main.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    private void Update() {
        if (degrade) {
            // var mult = Mathf.Pow(1 - degradeSpeed, Time.deltaTime);
            fogTexture.Degrade(degradeSpeed);
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        // ONLY WORKS if camera bounds never span out of 4 grids
        var camPos = Camera.main.transform.position;

        // Assuming camera locates upper-right part of the grid where it is located
        var gridUR = fogTexture.PosToGrid(camPos);
        var gridURCenter = fogTexture.GridToPos(gridUR, new Vector3(0.5f, 0.5f));

        // Correct otherwise
        if (camPos.x > gridURCenter.x) gridUR.x += 1;
        if (camPos.y > gridURCenter.y) gridUR.y += 1;
        
        var gridDiagonal = new Vector3Int(1, 1, 0);
        mat.SetTexture("_FogUR", fogTexture.GetTexture(gridUR));
        mat.SetTexture("_FogUL", fogTexture.GetTexture(gridUR - Vector3Int.right));
        mat.SetTexture("_FogLR", fogTexture.GetTexture(gridUR - Vector3Int.up));
        mat.SetTexture("_FogLL", fogTexture.GetTexture(gridUR - gridDiagonal));
        mat.SetVector("_FogPos", fogTexture.GridToPos(gridUR));
        mat.SetFloat("_FogSize", fogTexture.gridSize);

        var bounds = GetOrthographicBounds();
        var boundsVector = new Vector4(bounds.min.x, bounds.min.y, bounds.max.x, bounds.max.y);
        mat.SetVector("_CameraBounds", boundsVector);
        Graphics.Blit(src, dest, mat);
    }
}
