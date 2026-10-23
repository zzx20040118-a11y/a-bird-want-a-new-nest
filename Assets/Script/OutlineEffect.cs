
using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    public Material outlineMaterial;

    private Renderer targetRenderer;
    private GameObject outlineObject;

    void Awake()
    {
        targetRenderer = GetComponent<Renderer>();

        if (targetRenderer == null)
        {
            Debug.LogWarning("没有找到 Renderer：" + gameObject.name);
            return;
        }

        outlineObject = new GameObject("Outline_Visual");
        outlineObject.transform.SetParent(transform, false);

        MeshFilter originalMesh = GetComponent<MeshFilter>();

        if (originalMesh != null)
        {
            MeshFilter outlineMesh =
                outlineObject.AddComponent<MeshFilter>();

            outlineMesh.sharedMesh = originalMesh.sharedMesh;

            MeshRenderer outlineRenderer =
                outlineObject.AddComponent<MeshRenderer>();

            outlineRenderer.sharedMaterial = outlineMaterial;
            outlineObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("没有找到 MeshFilter：" + gameObject.name);
            Destroy(outlineObject);
        }
    }

    public void SetOutline(bool visible)
    {
        if (outlineObject != null)
        {
            outlineObject.SetActive(visible);
        }
    }
}
