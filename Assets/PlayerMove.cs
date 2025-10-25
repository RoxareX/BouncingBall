using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour
{
    public CombatManager combatManager;
    private Rigidbody2D rb2D;

    public float Health = 100;
    public bool invulnerability;
    public float invulnerabilityStarted;
    public float fatStarted;

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
        if ((Time.time - invulnerabilityStarted) >= 10)
        {
            invulnerability = false;
        }
        if ((Time.time - fatStarted) >= 10)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        transform.parent.transform.position = transform.position;
        transform.localPosition = new Vector3(0, 0, 0);

        rb2D.linearVelocity = Vector2.ClampMagnitude(rb2D.linearVelocity, 20);

        if (Input.GetKey("w"))
        {
            rb2D.AddForce(transform.parent.transform.up * 0.4f);
        }
        if (Input.GetKey("s"))
        {
            rb2D.AddForce(-transform.parent.transform.up * 0.4f);
        }
        if (Input.GetKey("d"))
        {
            rb2D.AddForce(transform.parent.transform.right * 0.4f);
        }
        if (Input.GetKey("a"))
        {
            rb2D.AddForce(-transform.parent.transform.right * 0.4f);
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

        if (collision.gameObject.tag == "Opponent")
        {
            var damagetodo = Mathf.Round(rb2D.linearVelocity.magnitude);
            combatManager.DamageOpponent(damagetodo);
            combatManager.SpawnHitParticle(collision);
            combatManager.shake = 0.2f;
        }
        
    }
}
