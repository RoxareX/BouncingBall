using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    // -10 -> 10, -4 -> 4

    public GameObject[] Powerup;

    private float LastPowerUpSpawn;
    private GameObject PowerUpObject;
    private bool spawned;

    // Update is called once per frame
    void Update()
    {
        if (spawned)
        {
            DestroyPowerUp();
        }
        else
        {
            SpawnEvery10Seconds();
        }
    }

    private void SpawnEvery10Seconds()
    {
        if ((Time.time - LastPowerUpSpawn) >= 2)
        {
            PowerUpObject = Instantiate(Powerup[Random.Range(0, 3)], new Vector3(Random.Range(-10, 10), Random.Range(-4, 4), 0), transform.rotation);
            spawned = true;
        }
    }
    
    private void DestroyPowerUp()
    {
        if ((Time.time - LastPowerUpSpawn) >= 4)
        {
            try
            {
                Destroy(PowerUpObject);
            }
            catch (System.Exception)
            {
                Debug.Log("Already destroyed");                
            }
            
            spawned = false;
            LastPowerUpSpawn = Time.time;
        }
    }
}
