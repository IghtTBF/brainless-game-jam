using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject fireflies;
    public Vector3 spawnAreaSize = new Vector3(10f,10f,10f);
    public float spawnInterval = 2f;
    public int maxFireflies = 20;

    private float nextSpawnTime;
    private int currentFireflyCount = 0;
    

    void Update()
    {
        if (Time.time >= nextSpawnTime && currentFireflyCount < maxFireflies)
        {
            SpawnObject();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnObject()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x /2), 
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y /2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z /2)
        );

        Vector3 spawnPosition = transform.TransformPoint(randomPosition);

        GameObject newFirefly = Instantiate(fireflies, spawnPosition, fireflies.transform.rotation);
        currentFireflyCount++;

        FireflyTracker tracker = newFirefly.AddComponent<FireflyTracker>();
        tracker.spawner = this;
    }

    public void OnFireflyDestroyed()
    {
        currentFireflyCount--;
    }

    void OGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }

    public class FireflyTracker : MonoBehaviour
    {
        public SpawnerScript spawner;

        void OnDestroy()
        {
            if (spawner != null)
            {
                spawner.OnFireflyDestroyed();
            }
        }
    }
}
