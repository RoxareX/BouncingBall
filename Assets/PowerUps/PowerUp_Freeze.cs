using UnityEngine;

public class PowerUp_Freeze : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
        }
        if (collision.gameObject.tag == "Opponent")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
        }
        Destroy(gameObject);
    }
}
