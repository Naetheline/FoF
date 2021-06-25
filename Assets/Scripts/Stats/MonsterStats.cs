using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Represent the caracteristic of a monster.
 *  Takes damage when the player interact with it.
 */

public class MonsterStats : CharacterStats
{
    // Start is called before the first frame update
    void Start()
    {
        // TODO : Find a way to set this by monster type
        this.strength.SetValue(10);
        this.constitution.SetValue(8);
        this.intelligence.SetValue(10);
        this.dexterity.SetValue(10);
        this.willpower.SetValue(10);

        this.HPMax.SetValue(20);
        this.defPhy.SetValue(1);

        currentHP = HPMax.GetValue() * 10;
    }

    public override void Die()
    {
        Debug.Log(this.name + "died...");

        this.gameObject.SetActive(false);

        // TODO : Give xp to the player

    }


    public override void MoveToInteraction(CharacterControl cc, PlayerStats player)
    {
        base.MoveToInteraction(cc, player);
        cc.agent.stoppingDistance = range.GetValue();
    }

    public override void Interact(CharacterControl cc, PlayerStats player)
    {
        base.Interact(cc, player);
        float valueOfDamage = (player.typeDamage == DamageType.PHYSICAL) ? player.damagePhy.GetValue() : player.damageMagic.GetValue();
        this.TakeDamage(player.typeDamage, valueOfDamage);
    }
}
