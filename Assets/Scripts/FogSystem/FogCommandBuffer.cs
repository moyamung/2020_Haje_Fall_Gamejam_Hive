using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class FogCommandBuffer : MonoBehaviour
{
    public enum CommandType {
        Terrain,
        Enemy,
    };
    public CommandType type;
    public Shader shader;

    private Material mat;
    private Camera cam;

    private void OnEnable() {
        mat = new Material(shader);
        cam = GetComponent<Camera>();

        if (type == CommandType.Terrain) {
            CommandBuffer cb = new CommandBuffer();
            cb.name = "Fog of war: Terrain";
            
            var terrainID = Shader.PropertyToID("_Terrain");
            cb.GetTemporaryRT(terrainID, -1, -1, 0, FilterMode.Bilinear);
            cb.SetRenderTarget(terrainID);
            cb.ClearRenderTarget(true, true, Color.black);
            cb.SetGlobalTexture("_Terrain", terrainID);
            cam.AddCommandBuffer(CameraEvent.BeforeDepthTexture, cb);
        } else if (type == CommandType.Enemy) {
            CommandBuffer cb = new CommandBuffer();
            cb.name = "Fog of war: Enemy";
            
            var enemyID = Shader.PropertyToID("_Enemy");
            cb.GetTemporaryRT(enemyID, -1, -1, 0, FilterMode.Bilinear);
            cb.SetRenderTarget(enemyID);
            cb.ClearRenderTarget(true, true, Color.clear);
            cb.SetGlobalTexture("_Enemy", enemyID);
            cam.AddCommandBuffer(CameraEvent.BeforeDepthTexture, cb);
        }
    }
}
