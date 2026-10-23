
using UnityEngine;

public class CrateOpener : MonoBehaviour
{
    public GameObject cratePanel;

    void OnMouseDown()
    {
        if (cratePanel != null)
        {
            cratePanel.SetActive(true);
            Debug.Log("Crate opened!");
        }
        else
        {
            Debug.LogError("CratePanel is not assigned!");
        }
    }
}
