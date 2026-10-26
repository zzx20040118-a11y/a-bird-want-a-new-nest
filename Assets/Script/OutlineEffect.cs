using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    [Header("轮廓设置")]
    public Material outlineMaterial;

    private class OutlinePart
    {
        public GameObject outlineObject;
        public Renderer originalRenderer;
    }

    private readonly List<OutlinePart> outlineParts =
        new List<OutlinePart>();

    private bool initialized = false;

    private void Awake()
    {
        InitializeOutline();
    }

    private void InitializeOutline()
    {
        if (initialized)
            return;

        initialized = true;

        if (outlineMaterial == null)
        {
            Debug.LogWarning(
                "没有指定 Outline Material：" + gameObject.name,
                this
            );
            return;
        }

        // 获取当前物体及其所有子物体上的 Renderer
        Renderer[] renderers =
            GetComponentsInChildren<Renderer>(true);

        foreach (Renderer originalRenderer in renderers)
        {
            // 跳过已经生成的轮廓对象，避免重复生成
            if (originalRenderer.gameObject.name == "Outline_Visual")
                continue;

            MeshFilter meshFilter =
                originalRenderer.GetComponent<MeshFilter>();

            // 当前版本针对普通 MeshRenderer
            if (!(originalRenderer is MeshRenderer) ||
                meshFilter == null ||
                meshFilter.sharedMesh == null)
            {
                continue;
            }

            GameObject outlineObject =
                new GameObject("Outline_Visual");

            outlineObject.transform.SetParent(
                originalRenderer.transform,
                false
            );

            MeshFilter outlineMesh =
                outlineObject.AddComponent<MeshFilter>();

            outlineMesh.sharedMesh = meshFilter.sharedMesh;

            MeshRenderer outlineRenderer =
                outlineObject.AddComponent<MeshRenderer>();

            outlineRenderer.sharedMaterial = outlineMaterial;

            // 保持与原模型相同的渲染层级
            outlineRenderer.sortingLayerID =
                originalRenderer.sortingLayerID;

            outlineRenderer.sortingOrder =
                originalRenderer.sortingOrder;

            outlineObject.SetActive(false);

            outlineParts.Add(new OutlinePart
            {
                outlineObject = outlineObject,
                originalRenderer = originalRenderer
            });
        }

        if (outlineParts.Count == 0)
        {
            Debug.LogWarning(
                "没有找到可生成轮廓的 MeshRenderer：" +
                gameObject.name,
                this
            );
        }
        else
        {
            Debug.Log(
                "轮廓初始化完成：" + gameObject.name +
                "，共 " + outlineParts.Count + " 个部分。",
                this
            );
        }
    }

    public void SetOutline(bool visible)
    {
        foreach (OutlinePart part in outlineParts)
        {
            if (part.outlineObject != null)
            {
                part.outlineObject.SetActive(visible);
            }
        }
    }

    private void OnDisable()
    {
        SetOutline(false);
    }
}
