using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 *  Fireball is a Skill that, when used, will create a projectile 
 *  from the launcher in the direction of the mouse cursor.
 * 
 *  TODO : pass it a target, so we can change it easily if a monster is to cast this spell.
 */ 

public class Fireball : Skill
{

    private float speed;

    public Fireball(string name, int attack, float cooldown, float spd, CharacterStats launcher) 
        : base(name, GameManager.skillSpriteDict["fireSpell"], CharacterStats.DamageType.MAGICAL, attack, cooldown, launcher)
    {
        speed = spd;
    }

    public override void Use()
    {
        base.Use();

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        Vector3 mousePos = hit.point;

        Vector3 dir = mousePos - new Vector3(launcher.transform.position.x, 0, launcher.transform.position.z);
        dir.Normalize();
        


        Vector3 position = launcher.transform.position + dir / 2;
        position.y = 1;
        launcher.transform.LookAt(mousePos);
        GameObject fireBall = GameObject.Instantiate(projectilePrefab, position + dir , launcher.transform.rotation);
        Projectile projectile = fireBall.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetTypeAndStrength(Type, AttackPwr);
        }
        Rigidbody rb = fireBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            
            rb.velocity = dir * speed;
        }
        
    }
}
