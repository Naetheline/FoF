using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  This class manage which visual model of hat is display.
 * 
 *  Must be subscribe to the hat change of the hat slot (Equipement)
 * 
 * 
 */
public class EquipHat : MonoBehaviour
{
    public GameObject topHat;
    public GameObject fedora;
    public GameObject witchHat;



    public void OnHatChanged(Hat newHat, Hat oldHat)
    {

        if (oldHat != null)
        {
            switch (oldHat.TypeHat)
            {
                case Hat.hatType.TOP: topHat.SetActive(false); break;
                case Hat.hatType.FEDORA: fedora.SetActive(false); break;
                case Hat.hatType.WITCH: witchHat.SetActive(false); break;
                default: break;
            }

        }

        if (newHat != null)
        {
            // equip new weapon
            switch (newHat.TypeHat)
            {
                case Hat.hatType.TOP: topHat.SetActive(true); break;
                case Hat.hatType.FEDORA: fedora.SetActive(true); break;
                case Hat.hatType.WITCH: witchHat.SetActive(true); break;
                default: break;
            }
        }

    }
}
