using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This class manage the equipement panel.
 * 
 *  It is also in charge of regestering the listeners (PlayerStat, for stats change and EquipWeapon for model change) of the different equippement slots
 *  to notify then in case of a change of equipement.
 */ 

public class Equipment : MonoBehaviour
{
    private GameObject equipmentPanel;
    [SerializeField]
    private Slot weaponSlot;
    [SerializeField]
    private Slot hatSlot;

    private void Start()
    {
        equipmentPanel = GameObject.Find("EquipmentPanel");
        equipmentPanel.SetActive(false);

        weaponSlot.RegisterWeaponChanged(this.GetComponent<PlayerStats>().OnWeaponChanged);
        weaponSlot.RegisterWeaponChanged(this.GetComponentInChildren<EquipWeapon>().OnWeaponChanged);

        hatSlot.RegisterHatChanged(this.GetComponent<PlayerStats>().OnHatChanged);
        hatSlot.RegisterHatChanged(this.GetComponentInChildren<EquipHat>().OnHatChanged);
    }


   

    public void OpenEquipment()
    {
        equipmentPanel.SetActive(true);
    }

    public void CloseEquipment()
    {
        equipmentPanel.SetActive(false);
    }
}
