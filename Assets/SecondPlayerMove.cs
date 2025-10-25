using UnityEngine;

public class SecondPlayerMove : MonoBehaviour
{
    public CombatManager combatManager;
    private Rigidbody2D rb2D;

    public float Health = 100;
    public bool invulnerability;
    public float invulnerabilityStarted;
    public float fatStarted;
    public float PowerUp_ultimateControl = 1f;
    public float ultimateControlStarted;

    void Start()
    {
        rb2D = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (combatManager.gameHasEnded)
        {
            gameObject.SetActive(false);
            return;
        }

        // Resets
        if ((Time.time - invulnerabilityStarted) >= 10)
        {
            invulnerability = false;
        }
        if ((Time.time - fatStarted) >= 10)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if ((Time.time - ultimateControlStarted) >= 10)
        {
            PowerUp_ultimateControl = 1f;
        }


        // Controls 
        transform.parent.transform.position = transform.position;
        transform.localPosition = new Vector3(0, 0, 0);

        rb2D.linearVelocity = Vector2.ClampMagnitude(rb2D.linearVelocity, 20);

        if (Input.GetKey("up"))
        {
            rb2D.AddForce(transform.parent.transform.up * 0.5f * PowerUp_ultimateControl);
        }
        if (Input.GetKey("down"))
        {
            rb2D.AddForce(-transform.parent.transform.up * 0.5f * PowerUp_ultimateControl);
        }
        if (Input.GetKey("right"))
        {
            rb2D.AddForce(transform.parent.transform.right * 0.5f * PowerUp_ultimateControl);
        }
        if (Input.GetKey("left"))
        {
            rb2D.AddForce(-transform.parent.transform.right * 0.5f * PowerUp_ultimateControl);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "RoofAndFloor")
        {
            rb2D.linearVelocityX = -collision.relativeVelocity.x * 1;
            rb2D.linearVelocityY = collision.relativeVelocity.y * 1;
        }
        else if (collision.gameObject.tag == "SideWall")
        {
            rb2D.linearVelocityX = collision.relativeVelocity.x * 1;
            rb2D.linearVelocityY = -collision.relativeVelocity.y * 1;
        }

        if (collision.gameObject.tag == "Player")
        {
            var damagetodo = Mathf.Round(rb2D.linearVelocity.magnitude);
            combatManager.DamagePlayer(damagetodo);
        }
    }
}
