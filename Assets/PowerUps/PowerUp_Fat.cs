using UnityEngine;

public class PowerUp_Fat : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.localScale += new Vector3(1, 1, 1);
            collision.gameObject.GetComponent<PlayerMove>().fatStarted = Time.time;
        }
        if (collision.gameObject.tag == "Opponent")
        {
            collision.gameObject.transform.localScale += new Vector3(1, 1, 1);
            collision.gameObject.GetComponent<SecondPlayerMove>().fatStarted = Time.time;
        }
        Destroy(gameObject);
    }
}
