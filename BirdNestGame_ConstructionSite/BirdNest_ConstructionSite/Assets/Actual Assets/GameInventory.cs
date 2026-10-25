
using UnityEngine;
using System.Collections.Generic;

public class GameInventory : MonoBehaviour
{
    public static GameInventory Instance;

    private HashSet<string> collectedItems =
        new HashSet<string>();

    public List<string> Items
    {
        get { return new List<string>(collectedItems); }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Collect(string itemName)
    {
        if (collectedItems.Add(itemName))
        {
            Debug.Log("Collected: " + itemName);
        }
        else
        {
            Debug.Log(itemName + " is already collected.");
        }
    }

    public bool HasItem(string itemName)
    {
        return collectedItems.Contains(itemName);
    }

    public string GetInventoryText()
    {
        if (collectedItems.Count == 0)
            return "Materials: None";

        return "Materials: " +
            string.Join(", ", collectedItems);
    }
}
