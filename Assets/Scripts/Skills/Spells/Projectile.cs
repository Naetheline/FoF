using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  Projectile that does damage when it collides with a character (monster or player).
 * 
 * 
 */
public class Projectile : MonoBehaviour
{
    public float lifetime = 10f;
    private CharacterStats.DamageType type;
    private float strength;

    private void OnTriggerEnter(Collider other)
    {
        CharacterStats character = other.GetComponent<CharacterStats>();

        if(character != null)
        {
            character.TakeDamage(type, strength);
            Destroy(this.gameObject);
        }
        
        
    }

    public void SetTypeAndStrength(CharacterStats.DamageType t, float s, float lt = 10f)
    {
        type = t;
        strength = s;
        lifetime = lt;
    }

    public void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
