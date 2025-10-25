using UnityEngine;

public class PowerUp_UltimateControl : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMove>().ultimateControlStarted = Time.time;
            collision.gameObject.GetComponent<PlayerMove>().PowerUp_ultimateControl = 2;
        }
        if (collision.gameObject.tag == "Opponent")
        {
            collision.gameObject.GetComponent<SecondPlayerMove>().ultimateControlStarted = Time.time;
            collision.gameObject.GetComponent<SecondPlayerMove>().PowerUp_ultimateControl = 2;
        }
        Destroy(gameObject);
    }
}
