using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Collections;

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
    public TMP_Text LightRayText;

    public GameObject explosion;
    public GameObject backgroundFlash;

    private PlayerMove pmScript;
    private SecondPlayerMove secondpmScript;
    private float LastTimePlayerTookDamage = 0;
    private float LastTimeOpponentTookDamage = 0;

    private float lastTimeRaySpawned = 0;
    private bool isSpawning = false;
    private GameObject RayObject;
    private Vector3 lightrayspawnpos;

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
        if (shake > 0)
        {
            Vector3 shakeCalc = Random.insideUnitSphere * shakeAmount / 10;

            cameraObject.transform.localPosition = new Vector3(shakeCalc.x, shakeCalc.y, -10);

            shake -= Time.deltaTime * decreaseFactor;

        }
        else
        {
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

        if (!isSpawning && Time.time - lastTimeRaySpawned >= 20)
        {
            StartCoroutine(SpawnRayWithWarning());
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

    private IEnumerator SpawnRayWithWarning()
    {
        isSpawning = true;

        // Pick a random position for the upcoming ray
        Vector3 spawnPos = new Vector3(Random.Range(-9f, 9f), 0f, 0f);

        // Convert to screen space for the overlay text
        Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnPos);
        LightRayText.transform.position = screenPos;

        // Show the warning text
        LightRayText.gameObject.SetActive(true);

        // Wait 3 seconds before spawning the actual ray
        yield return new WaitForSeconds(3f);

        // Hide the text
        LightRayText.gameObject.SetActive(false);

        float t = 0;

        // Spawn the light ray in world space
        GameObject lightRayObj = Instantiate(LightRay, spawnPos, Quaternion.identity);


        while (t < 1.0f)
        {
            t += Time.deltaTime / 2;
            lightRayObj.transform.localScale = new Vector3(Mathf.Lerp(0.1f, 1f, t), 1, 1);

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        Destroy(lightRayObj);

        // Reset state
        lastTimeRaySpawned = Time.time;
        isSpawning = false;
    }


    public void SpawnHitParticle(Collision2D collision)
    {
        foreach (ContactPoint2D unitHit in collision.contacts)
        {
            Vector2 hitPoint = unitHit.point;
            GameObject spawnedExplosion = Instantiate(explosion, new Vector3(hitPoint.x, hitPoint.y, 0), Quaternion.identity);
            Destroy(spawnedExplosion, 2f);
        }
    }

    public void BackgroundLightFlash(Collision2D collision)
    {
        foreach (ContactPoint2D unitHit in collision.contacts)
        {
            Vector2 hitPoint = unitHit.point;
            GameObject spawnedbgFlash = Instantiate(
                backgroundFlash, 
                new Vector3(hitPoint.x, hitPoint.y, 0f), 
                Quaternion.identity
            );

            Light2D flashLight = spawnedbgFlash.GetComponent<Light2D>();
            Light2D flashLightchild = spawnedbgFlash.transform.GetChild(0).GetComponent<Light2D>();
            if (flashLight != null)
            {
                StartCoroutine(DimLightAndDestroy(flashLight, 1f));
                StartCoroutine(DimLightAndDestroy(flashLightchild, 1f));
            }
            else
            {
                Destroy(spawnedbgFlash, 1f); // fallback if no Light2D found
            }
        }
    }

    private System.Collections.IEnumerator DimLightAndDestroy(Light2D light, float duration)
    {
        float startIntensity = light.intensity;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            light.intensity = Mathf.Lerp(startIntensity, 0f, elapsed / duration);
            yield return null;
        }

        light.intensity = 0f;
        Destroy(light.gameObject);
    }
}
