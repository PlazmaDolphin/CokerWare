using UnityEngine;

public class OutlineOnHover : MonoBehaviour
{
    public Material outlineMaterial;  // The material with the outline shader
    private Material originalMaterial;  // Store the original material
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    void OnMouseEnter()
    {
        // When the mouse hovers over the object, change the material to the outline
        objectRenderer.material = outlineMaterial;
    }

    void OnMouseExit()
    {
        // When the mouse exits the object, revert to the original material
        objectRenderer.material = originalMaterial;
    }
}