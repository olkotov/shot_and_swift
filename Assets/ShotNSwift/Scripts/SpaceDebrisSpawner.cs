// Oleg Kotov

using UnityEngine;

public class SpaceDebrisSpawner : MonoBehaviour
{
    public Vector3 spawnPosition = new Vector3( 0.0f, -12.5f, 0.0f );
    public float debrisMovementSpeed = 4.0f;

    public GameObject coinPrefab;
    public GameObject obstaclePrefab;

    private float spawnInterval = 0.0f;
    private float spawnTimer = 0.0f;

    private bool shouldSpawnObstacle = false;
    private float coinSpawnChance = 1.0f / 3.0f;

    void Start()
    {
        // time = distance / speed
        spawnInterval = 2.5f / debrisMovementSpeed;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if ( spawnTimer <= 0.0f )
        {
            spawnTimer = spawnInterval;
            shouldSpawnObstacle = !shouldSpawnObstacle;

            if ( shouldSpawnObstacle )
            {
                Quaternion rotation = Quaternion.Euler( 0.0f, 0.0f, Random.Range( 0.0f, 360.0f ) );
                GameObject debris = Instantiate( obstaclePrefab, spawnPosition, rotation, transform );
                debris.GetComponent<Movier>().speed = debrisMovementSpeed;
            }
            else
            {
                if ( Random.value > coinSpawnChance ) return;

                GameObject debris = Instantiate( coinPrefab, spawnPosition, Quaternion.identity, transform );
                debris.GetComponent<Movier>().speed = debrisMovementSpeed;
            }
        }
    }
}

