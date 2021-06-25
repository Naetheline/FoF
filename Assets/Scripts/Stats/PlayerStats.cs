using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  Represent the caracteristcs of the player.
 * 
 *  Manage the equipment changed via the OnWeaponChaneged and OnHatChanged (must register to the specific equipment slot)
 *  Manage the display of HP
 *  TODO : Manage the xp and the display of xp
 *  TODO : Manage level and the display of level
 */

public class PlayerStats : CharacterStats
{

    [SerializeField]
    private Image HP;

    private void Start()
    {
        this.strength.SetValue(11);
        this.constitution.SetValue(12);
        this.intelligence.SetValue(13);
        this.dexterity.SetValue(14);
        this.willpower.SetValue(15);
        this.range.SetValue(1f);

        this.HPMax.SetValue(20);
        this.defPhy.SetValue(3);
        this.attackSpeed.SetValue(1);

        currentHP = Mathf.RoundToInt( HPMax.GetValue() * 10);

    }


    public override void TakeDamage(DamageType type, float value)
    {
        base.TakeDamage(type, value);

        HP.fillAmount = currentHP / (HPMax.GetValue() * 10);
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        HP.fillAmount = currentHP / (HPMax.GetValue() * 10);
    }


    public void OnWeaponChanged(Weapon newOne, Weapon oldOne)
    {
        if (oldOne != null)
        {
            // unequip old one
            range.RemoveModificator(oldOne.RangeModifier);
            if(oldOne.TypeDamage == DamageType.PHYSICAL)
            {
                damagePhy.RemoveModificator(oldOne.AttackModifier);
            }
            else if(oldOne.TypeDamage == DamageType.MAGICAL)
            {
                damageMagic.RemoveModificator(oldOne.AttackModifier);
            }
        }

        if (newOne != null)
        {
            // equip new weapon
            range.AddModificator(newOne.RangeModifier);
            typeDamage = newOne.TypeDamage;
            if (newOne.TypeDamage == DamageType.PHYSICAL)
            {
                damagePhy.AddModificator(newOne.AttackModifier);
            }
            else if (newOne.TypeDamage == DamageType.MAGICAL)
            {
                damageMagic.AddModificator(newOne.AttackModifier);
            }
        }
        else
        {
            typeDamage = DamageType.PHYSICAL;
        }
    }


    public void OnHatChanged(Hat newOne, Hat oldOne)
    {
        if (oldOne != null)
        {
            defPhy.RemoveModificator(oldOne.PhysArmorModifier);
            defMagic.RemoveModificator(oldOne.MagicArmorModifier);
        }
        if (newOne != null)
        {
            defPhy.AddModificator(newOne.PhysArmorModifier);
            defMagic.AddModificator(newOne.MagicArmorModifier);
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            this.TakeDamage(DamageType.PHYSICAL, 50);
        }
    }
}
