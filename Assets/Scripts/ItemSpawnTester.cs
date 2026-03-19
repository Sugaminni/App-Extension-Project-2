using UnityEngine;

public class ItemSpawnTester : MonoBehaviour
{
    public string itemName = "Health30";
    public Transform spawnPoint;

    // Test item spawning with P key
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ItemDatabase.Instance.SpawnItemByName(itemName, spawnPoint.position, Quaternion.identity);
        }
    }
}