
using UnityEngine;
using TMPro;

public class InventoryDisplay : MonoBehaviour
{
    public TMP_Text inventoryText;

    void Update()
    {
        if (GameInventory.Instance != null &&
            inventoryText != null)
        {
            inventoryText.text =
                GameInventory.Instance.GetInventoryText();
        }
    }
}
