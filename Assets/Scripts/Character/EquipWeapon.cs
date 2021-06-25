using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  This class manage which visual model of weapon is display.
 * 
 *  Must be subscribe to the weapon change of the weapon slot (Equipement)
 * 
 * 
 */
public class EquipWeapon : MonoBehaviour
{

    public GameObject sword;
    public GameObject axe;
    public GameObject bow;
  

    public void OnWeaponChanged(Weapon newWeapon, Weapon oldWeapon)
    {

        if( oldWeapon != null)
        {
            switch (oldWeapon.TypeWeapon)
            {
                case Weapon.weaponType.SWORD: sword.SetActive(false); break;
                case Weapon.weaponType.AXE: axe.SetActive(false); break;
                case Weapon.weaponType.BOW: bow.SetActive(false); break;
                default: break;
            }
    
        }

        if( newWeapon != null)
        {
            // equip new weapon
            switch (newWeapon.TypeWeapon)
            {
                case Weapon.weaponType.SWORD: sword.SetActive(true); break;
                case Weapon.weaponType.AXE: axe.SetActive(true); break;
                case Weapon.weaponType.BOW: bow.SetActive(true); break;
                default: break;
            }
        }

    }

}
