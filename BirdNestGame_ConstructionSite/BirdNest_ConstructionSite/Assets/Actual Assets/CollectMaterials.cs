
using UnityEngine;

public class CollectMaterial : MonoBehaviour
{
    public string itemName;

    public void Collect()
    {
        Debug.Log("Collected material: " + itemName);

        gameObject.SetActive(false);
    }
}
