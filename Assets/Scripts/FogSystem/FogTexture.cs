using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTexture : MonoBehaviour
{
    public int gridSize;
    public int textureSize;
    public AnimationCurve sightFallout;
    
    public Dictionary<Vector3Int, Texture2D> textures;

    public delegate void TextureGridEvent(Texture2D texture, Vector3Int gridPos);
    public event TextureGridEvent TextureAssigned;

    public ComputeShader degradeShader;
    private RenderTexture rt;

    private void Start() {
        textures = new Dictionary<Vector3Int, Texture2D>();
        
        rt = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.RHalf);
        rt.enableRandomWrite = true;
        rt.Create();
    }

    public Vector3Int PosToGrid(Vector3 pos) {
        pos.z = 0;
        return Vector3Int.FloorToInt(pos / gridSize);
    }

    public Vector3 GridToPos(Vector3Int gridPos) {
        gridPos.z = 0;
        return (Vector3)gridPos * gridSize;
    }

    public Vector3 GridToPos(Vector3Int gridPos, Vector3 gridOffset) {
        gridOffset.z = 0;
        gridPos.z = 0;
        return (Vector3)(gridPos + gridOffset) * gridSize;
    }

    public Texture2D GetTexture(Vector3Int gridPos) {
        Texture2D result;
        if (textures.TryGetValue(gridPos, out result))
            return result;
        return null;
    }

    public void UpdateAt(Vector3 pos, float radius) {
        var gridMin = PosToGrid(pos - radius*Vector3.one);
        var gridMax = PosToGrid(pos + radius*Vector3.one);

        for (int gridX = gridMin.x; gridX <= gridMax.x; gridX++)
        for (int gridY = gridMin.y; gridY <= gridMax.y; gridY++) {
            var gridPos = new Vector3Int(gridX, gridY, 0);
            UpdateTexture(gridPos, pos, radius);
        }
    }

    public Texture2D GetInitializedTexture(Vector3Int gridPos) {
        var texture = GetTexture(gridPos);
        if (texture == null) {
            texture = new Texture2D(textureSize, textureSize, TextureFormat.RHalf, false, true);
            texture.anisoLevel = 0;
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Mirror;
            textures.Add(gridPos, texture);
            
            TextureAssigned?.Invoke(texture, gridPos);
        }
        return texture;
    }

    public void UpdateTexture(Vector3Int gridPos, Vector3 pos, float radius) {
        pos.z = 0;
        gridPos.z = 0;
        if (!IsGridOverlap(gridPos, pos, radius)) return;
        
        var texture = GetInitializedTexture(gridPos);

        var gridCenteredPos = pos - GridToPos(gridPos);
        var texCenter = gridCenteredPos / gridSize * textureSize;
        var texRadius = radius / gridSize * textureSize;
        var texRadiusSqr = texRadius * texRadius;
        for (int x = Mathf.CeilToInt(texCenter.x - texRadius); x < texCenter.x + texRadius; x++)
        for (int y = Mathf.CeilToInt(texCenter.y - texRadius); y < texCenter.y + texRadius; y++) {
            if (x < 0 || x >= textureSize || y < 0 || y >= textureSize) continue;
            var texPos = new Vector3(x + 0.5f, y + 0.5f, 0);
            var texDistSqr = (texPos - texCenter).sqrMagnitude;
            if (texDistSqr > texRadiusSqr) continue;

            var texDist = Mathf.Sqrt(texDistSqr);
            var original = texture.GetPixel(x, y).r;
            var newVal = sightFallout.Evaluate(texDist / texRadius);
            if (newVal > original) {
                texture.SetPixel(x, y, Color.red*newVal);
            }
        }
        texture.Apply(false);
    }

    public bool IsGridOverlap(Vector3Int gridPos, Vector3 pos, float radius) {
        var posMin = GridToPos(gridPos);
        var posMax = GridToPos(gridPos + Vector3Int.one);
        if (pos.x < posMin.x - radius
            || pos.x > posMax.x + radius
            || pos.y < posMin.y - radius
            || pos.y > posMax.y + radius) return false;
        var cornerTopLeft = new Vector3(posMin.x, posMax.y);
        var cornerBottomRight = new Vector3(posMax.x, posMin.y);
        var sqrRadius = radius * radius;
        if (pos.x < posMin.x && pos.y < posMin.y && (pos - posMin).sqrMagnitude > sqrRadius) return false;
        if (pos.x > posMax.x && pos.y > posMax.y && (pos - posMax).sqrMagnitude > sqrRadius) return false;
        if (pos.x < posMin.x && pos.y > posMax.y && (pos - cornerTopLeft).sqrMagnitude > sqrRadius) return false;
        if (pos.x > posMax.x && pos.y < posMin.y && (pos - cornerBottomRight).sqrMagnitude > sqrRadius) return false;
        return true;
    }

    public void Degrade(float multiplier) {
        foreach (var kv in textures) {
            var tex = kv.Value;
            DegradeTexture(tex, multiplier, 0);
        }
    }

    public void DegradeTexture(Texture2D texture, float multiplier, float eps) {
        int kernel = degradeShader.FindKernel("CSMain");
        degradeShader.SetTexture(kernel, "Input", texture);
        degradeShader.SetTexture(kernel, "Result", rt);
        degradeShader.SetFloat("multiplier", multiplier);
        degradeShader.SetFloat("eps", eps);
        degradeShader.Dispatch(kernel, textureSize/16, textureSize/16, 1);
        RenderTexture.active = rt;
        texture.ReadPixels(new Rect(0, 0, textureSize, textureSize), 0, 0, false);
        texture.Apply();
    }
}
