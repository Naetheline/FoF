using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Keep track of all the characteristic of a character.
 * 
 *  Can be interact with. 
 *  Has two sub-class PlayerStats for the player specific stat and MonsterStats for the mob.
 * 
 *  Healing and damage are done via this class.
 * 
 */


public class CharacterStats : Interactable
{
    public enum DamageType {PHYSICAL, MAGICAL, IGNOREDEF };

    public Stat strength;
    public Stat constitution;
    public Stat dexterity;
    public Stat intelligence;
    public Stat willpower;

    public Stat range;

    public CompStat HPMax, defPhy;
    public CompStat damagePhy;
    public CompStat hitChance, crit, attackSpeed;
    public CompStat damageMagic;
    public CompStat defMagic;

    public float currentHP;
    

    public DamageType typeDamage;


    public void Awake()
    {
       strength = new Stat();
        constitution = new Stat();
        dexterity = new Stat();
        intelligence = new Stat();
        willpower = new Stat();
        range = new Stat(1f);


    HPMax = new CompStat( constitution);
        defPhy = new CompStat( constitution);

        damagePhy = new CompStat( strength);

        hitChance = new CompStat( dexterity);
        crit = new CompStat( dexterity);
        attackSpeed = new CompStat( dexterity);

        damageMagic = new CompStat( intelligence);

        defMagic = new CompStat( willpower);

        currentHP = HPMax.GetValue();
    }

    public virtual void TakeDamage(DamageType type, float value)
    {
        float damage = 0;
        switch (type)
        {
            case DamageType.PHYSICAL: damage = value - defPhy.GetValue(); break;
            case DamageType.MAGICAL: damage = value - defMagic.GetValue(); break;
            default: damage = value; break;
        }
        Mathf.Clamp(damage, 0, int.MaxValue);

        Debug.Log(this.name + " takes " + damage  +" " + type + " damage(s).");

        currentHP -= damage;


        if(currentHP <= 0)
        {
            Die();
        }
    }

    public virtual void Heal(float amount)
    {
        currentHP += amount;
        Mathf.Clamp(currentHP, 0, HPMax.GetValue() * 10);
    }

    public virtual  void Die()
    {
        Debug.Log(this.name + " has died...");
    }

}
