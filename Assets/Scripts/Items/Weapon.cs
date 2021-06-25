using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


/*
 *  A Weapon is an item that cannot be used, but can be equiped in weapon slots.
 *  An equiped weapon changes the character stats.
 * 
 */

public class Weapon : Item
{
    public enum weaponType {SWORD, AXE, BOW }

    private weaponType typeWeapon;
    public weaponType TypeWeapon
    {
        get
        {
            return typeWeapon;
        }
    }

    private CharacterStats.DamageType typeDamage;
    public CharacterStats.DamageType TypeDamage
    {
        get
        {
            return typeDamage;
        }
    }

    private float rangeModifier;
    public float RangeModifier
    {
        get
        {
            return rangeModifier;
        }
    }
    private float attackModifier;
    public float AttackModifier
    {
        get
        {
            return attackModifier;
        }
    }
    
    public Weapon(string name, Sprite icon, float range, float attack, weaponType type = weaponType.SWORD) 
        : base(ItemType.WEAPON, name, icon )
    {
        rangeModifier = range;
        attackModifier = attack;
        typeWeapon = type;
    }

    public override string GetTooltip()
    {

        StringBuilder sb = new StringBuilder();
        sb.Append(base.GetTooltip());
        sb.Append("\nDoes" + TypeDamage +" damages." );
        sb.Append("\n+" + attackModifier);
        sb.Append("\n" + rangeModifier + "m");

        return sb.ToString();
    }
}
