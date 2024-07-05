using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class OverlayEffect : MonoBehaviour
{
    public Shader overlayShader;
    public Texture2D overlayTexture;
    [Range(0, 1)] public float overlayStrength = 1.0f;
    [Range(0, 1)] public float darkenStrength = 1.0f;

    private Material overlayMaterial;

    void Start()
    {
        if (overlayShader == null)
        {
            Debug.LogError("No overlay shader assigned!");
            enabled = false;
            return;
        }

        overlayMaterial = new Material(overlayShader);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (overlayMaterial != null)
        {
            if (overlayTexture != null)
            {
                overlayMaterial.SetTexture("_OverlayTex", overlayTexture);
                overlayMaterial.SetFloat("_OverlayStrength", overlayStrength);
                overlayMaterial.SetFloat("_DarkenStrength", darkenStrength);
                Graphics.Blit(src, dest, overlayMaterial);
            }
            else
            {
                Debug.LogError("Overlay texture is not assigned!");
                Graphics.Blit(src, dest);
            }
        }
        else
        {
            Debug.LogError("Overlay material is not created!");
            Graphics.Blit(src, dest);
        }
    }
}
