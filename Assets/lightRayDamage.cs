using UnityEngine;
using System.Collections.Generic;

public class lightRayDamage : MonoBehaviour
{
    public List<GameObject> units = new List<GameObject>();
    public bool isAbleToDamage = false;
    public int damageAmount = 1;          // integer damage
    public float damageInterval = 0.1f;     // apply damage every 1 second

    private float damageTimer = 0f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Opponent")
        {
            units.Add(collider.gameObject);
            isAbleToDamage = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Opponent")
        {
            units.Remove(collider.gameObject);
            isAbleToDamage = false;
        }
    }

    void Update()
    {
        if (!isAbleToDamage) return;

        damageTimer += Time.deltaTime;

        if (damageTimer >= damageInterval)
        {
            damageTimer = 0f; // reset timer

            foreach (GameObject obj in units)
            {
                if (obj.tag == "Player")
                {
                    obj.GetComponent<PlayerMove>().Health -= damageAmount;
                }
                else if (obj.tag == "Opponent")
                {
                    obj.GetComponent<SecondPlayerMove>().Health -= damageAmount;
                }
            }
        }
    }
}
