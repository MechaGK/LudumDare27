using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    float health;

    void ApplyDamage()
    {
        health--;
        

        if (health <= 0)
        {
            GameController.enemies = GameController.enemies + 1;
            Destroy(gameObject);
        }
    }
}
