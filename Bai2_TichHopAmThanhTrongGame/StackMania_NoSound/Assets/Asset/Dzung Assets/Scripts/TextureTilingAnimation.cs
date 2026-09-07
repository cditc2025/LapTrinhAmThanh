using UnityEngine;

public class TextureOffsetAnimation : MonoBehaviour
{
    Renderer targetRenderer;
    public Vector2 offsetSpeed = new Vector2(0.1f, 0.1f); // Speed of offset change

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (targetRenderer != null)
        {
            // Get the current offset
            Vector2 offset = targetRenderer.material.mainTextureOffset;

            // Update offset over time
            offset += offsetSpeed * Time.deltaTime;

            // Apply new offset
            targetRenderer.material.mainTextureOffset = offset;
        }
    }
}
