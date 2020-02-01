using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengeManager : MonoBehaviour
{
    public static ScavengeManager Instance;

    public List<GameObject> SpawnableItems;
    public Transform SpawnPoint;
    public float SpawnDelay = 5;

    private List<ItemData> scavengedItems = new List<ItemData>();
    private float time = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= SpawnDelay)
        {
            SpawnItem();
            time = 0;
        }
    }

    public void CollectItem(ItemData item)
    {
        scavengedItems.Add(item);
    }

    public void SpawnItem()
    {
        int i = Random.Range(0, SpawnableItems.Count);

        Instantiate(SpawnableItems[i], SpawnPoint.position, Quaternion.identity);
    }
}
