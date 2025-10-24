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

    private PlayerMove pmScript;
    private SecondPlayerMove secondpmScript;
    private float LastTimePlayerTookDamage = 0;
    private float LastTimeOpponentTookDamage = 0;

    void Start()
    {
        pmScript = player.GetComponent<PlayerMove>();
        secondpmScript = opponent.GetComponent<SecondPlayerMove>();
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
}
