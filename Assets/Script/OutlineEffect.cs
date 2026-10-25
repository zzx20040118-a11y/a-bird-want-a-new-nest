
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

        MeshFilter originalMesh = GetComponent<MeshFilter>();

        if (originalMesh == null)
        {
            Debug.LogWarning("没有找到 MeshFilter：" + gameObject.name);
            return;
        }

        if (outlineMaterial == null)
        {
            Debug.LogWarning("没有指定 Outline Material：" + gameObject.name);
            return;
        }

        outlineObject = new GameObject("Outline_Visual");
        outlineObject.transform.SetParent(transform, false);

        MeshFilter outlineMesh = outlineObject.AddComponent<MeshFilter>();
        outlineMesh.sharedMesh = originalMesh.sharedMesh;

        MeshRenderer outlineRenderer = outlineObject.AddComponent<MeshRenderer>();
        outlineRenderer.sharedMaterial = outlineMaterial;

        outlineObject.SetActive(false);

        Debug.Log("轮廓对象创建成功：" + gameObject.name);
    }

    public void SetOutline(bool visible)
    {
        if (outlineObject != null)
        {
            outlineObject.SetActive(visible);
            Debug.Log(
                gameObject.name + " 的轮廓设置为：" + visible
            );
        }
        else
        {
            Debug.LogWarning(
                gameObject.name + " 的 outlineObject 不存在！"
            );
        }
    }
}
