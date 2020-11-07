using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTextureTest : MonoBehaviour
{
    public FogTexture fogTexture;

    private void Start() {
        // fogTexture.TextureAssigned += CreateSprite;
    }

    private void CreateSprite(Texture2D tex, Vector3Int gridPos) {
        var obj = new GameObject("TestTexture");
        obj.layer = LayerMask.NameToLayer("TerrainFog");
        var sr = obj.AddComponent<SpriteRenderer>();
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, fogTexture.textureSize / fogTexture.gridSize);
        sr.sprite = sprite;

        obj.transform.position = fogTexture.GridToPos(gridPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            fogTexture.UpdateAt(pos, 4);
        }
    }
}
