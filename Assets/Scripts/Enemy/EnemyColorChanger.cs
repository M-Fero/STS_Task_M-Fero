using UnityEngine;

public class EnemyColorChanger : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer;
    [Header("Material Color Settings")] public Color materialColors;

    void Awake()
    {
        if (objectRenderer == null) return;
        Material[] materials = objectRenderer.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = materialColors;
        }
    }
}