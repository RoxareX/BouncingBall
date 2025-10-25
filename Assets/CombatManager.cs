using UnityEngine;
using TMPro;

public class CombatManager : MonoBehaviour
{

    public GameObject player;
    public GameObject opponent;

    public TMP_Text playerHealthText;
    public TMP_Text opponentHealthText;
    public GameObject endScreen;

    // EndGame Canvas
    public bool gameHasEnded;
    public TMP_Text WinnerText;

    public GameObject LightRay;

    public GameObject explosion;

    private PlayerMove pmScript;
    private SecondPlayerMove secondpmScript;
    private float LastTimePlayerTookDamage = 0;
    private float LastTimeOpponentTookDamage = 0;

    private float LastTimeRaySpawned = 0;
    private bool raySpawned;
    private GameObject RayObject;

    public Camera cameraObject; // set this via inspector
    public float shake;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    void Start()
    {
        pmScript = player.GetComponent<PlayerMove>();
        secondpmScript = opponent.GetComponent<SecondPlayerMove>();
    }

    void FixedUpdate() 
    {
        if (shake > 0) {
            Vector3 shakeCalc = Random.insideUnitSphere * shakeAmount / 10;

            cameraObject.transform.localPosition = new Vector3(shakeCalc.x, shakeCalc.y, -10);

            shake -= Time.deltaTime * decreaseFactor;

        } else {
            shake = 0.0f;
            cameraObject.transform.localPosition = new Vector3(0, 0, -10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerHealthText.text = pmScript.Health.ToString();
        opponentHealthText.text = secondpmScript.Health.ToString();

        if (pmScript.Health <= 0)
        {
            pmScript.Health = 0;
            WinnerText.text = "Player 2 Has Won!";
            endScreen.SetActive(true);
            gameHasEnded = true;
        }
        if (secondpmScript.Health <= 0)
        {
            secondpmScript.Health = 0;
            WinnerText.text = "Player 1 Has Won!";
            endScreen.SetActive(true);
            gameHasEnded = true;
        }

        if (raySpawned)
        {
            DestroyLightrayIn(22);
        }
        else
        {
            SpawnLightrayIn(20);
        }
    }

    public void DamagePlayer(float damage)
    {
        if (pmScript.invulnerability)
        {
            return;
        }
        if ((Time.time - LastTimePlayerTookDamage) >= 1)
        {
            pmScript.Health -= damage;
            LastTimePlayerTookDamage = Time.time;
        }
    }
    public void DamageOpponent(float damage)
    {
        if (secondpmScript.invulnerability)
        {
            return;
        }
        if ((Time.time - LastTimeOpponentTookDamage) >= 1)
        {
            secondpmScript.Health -= damage;
            LastTimeOpponentTookDamage = Time.time;
        }
    }

    public void SpawnLightrayIn(int timeTillNextSpawn)
    {
        if ((Time.time - LastTimeRaySpawned) >= timeTillNextSpawn)
        {
            RayObject = Instantiate(LightRay, new Vector3(Random.Range(-10, 10), 0, 0), transform.rotation);
            raySpawned = true;
        }
    }
    public void DestroyLightrayIn(int timeTillNextSpawn)
    {
        if ((Time.time - LastTimeRaySpawned) >= timeTillNextSpawn)
        {
            try
            {
                Destroy(RayObject);
            }
            catch (System.Exception)
            {
                Debug.Log("Ray Already destroyed");
            }

            raySpawned = false;
            LastTimeRaySpawned = Time.time;
        }
    }

    public void SpawnHitParticle(Collision2D collision)
    {
        foreach (ContactPoint2D missileHit in collision.contacts)
        {
            Vector2 hitPoint = missileHit.point;
            GameObject spawnedExplosion = Instantiate(explosion, new Vector3(hitPoint.x, hitPoint.y, 0), Quaternion.identity);
            Destroy(spawnedExplosion, 2f);
        }
    }
}
