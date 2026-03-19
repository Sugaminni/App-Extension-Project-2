using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    [System.Serializable]
    public class ItemEntry
    {
        public string itemName;
        public GameObject prefab;
    }

    [SerializeField] private List<ItemEntry> items = new List<ItemEntry>();

    private Dictionary<string, GameObject> itemLookup;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        itemLookup = new Dictionary<string, GameObject>();

        foreach (var item in items)
        {
            if (!string.IsNullOrWhiteSpace(item.itemName) && item.prefab != null)
            {
                if (!itemLookup.ContainsKey(item.itemName))
                    itemLookup.Add(item.itemName, item.prefab);
            }
        }
    }

    // Retrieves the prefab associated with the given item name. Returns null if not found.
    public GameObject GetPrefabByName(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
            return null;

        if (itemLookup.TryGetValue(itemName, out GameObject prefab))
            return prefab;

        Debug.LogWarning("ItemDatabase: No prefab found for item name: " + itemName);
        return null;
    }

    // Spawns an item by name at the given position and rotation. Returns the spawned GameObject or null if not found.
    public GameObject SpawnItemByName(string itemName, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = GetPrefabByName(itemName);

        if (prefab == null)
            return null;

        return Instantiate(prefab, position, rotation);
    }
}