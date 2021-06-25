using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.EventSystems;


/*
 * 
 * All input from the users are handle here
 * 
 */

[RequireComponent(typeof(CharacterStats))]
public class InputManager : MonoBehaviour { 

    private Camera mainCam;
    private PlayerStats player;
    private CharacterControl playerController;

    private GameObject map;


    private LayerMask interactableMask;


    private QuickBar quickbar;
    private SkillsBar skillsbar;

    private bool inventoryOpen = false;
    private bool equipmentOpen = false;
    private bool skillsOpen = false;
    private bool mapOpen = false;

    private float attackCooldown = 0f;

    void Start()
    {
        mainCam = Camera.main;
        player = GetComponent<PlayerStats>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControl>();
        interactableMask = ~LayerMask.GetMask("Player"); // All masks that are not the player mask (bitwise operation)
        

        quickbar = GameObject.Find("QuickBar").GetComponent<QuickBar>();
        skillsbar = GameObject.Find("SkillsBar").GetComponent<SkillsBar>();
        map = GameObject.Find("MapPanel");
        map.SetActive(false);

    }


    void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0)) // left click to move and interact
        {
            if ( ! EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit,  100, interactableMask))
                {

                    Interactable interactableObject = hit.collider.gameObject.GetComponent<Interactable>();
                    if(interactableObject != null && attackCooldown <= 0)
                    {
                        interactableObject.MoveToInteraction(playerController, player);
                        attackCooldown = 10f / player.attackSpeed.GetValue();
                    }
                    else
                    {
                        playerController.SetTarget(hit.point);
                        playerController.agent.stoppingDistance = 0.2f;
                    } 
                }
            }
        }

        

        // touch m open the map
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapOpen = !mapOpen;
            if (mapOpen)
            {
                map.SetActive(true);
            }
            else
            {
                map.SetActive(false);
            }
        }

        // touch I open the inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryOpen = !inventoryOpen;
            if(inventoryOpen)
            {
                player.GetComponent<Inventory>().OpenInventory();
            }
            else
            {
                player.GetComponent<Inventory>().CloseInventory();
            }
        }

        // touch c open the equipement panel (character panel)
        if (Input.GetKeyDown(KeyCode.C))
        {
            equipmentOpen = !equipmentOpen;
            if (equipmentOpen)
            {
                player.GetComponent<Equipment>().OpenEquipment();
            }
            else
            {
                player.GetComponent<Equipment>().CloseEquipment();
            }
        }
        // touch S open the spellsbook
        if (Input.GetKeyDown(KeyCode.S))
        {
            skillsOpen = !skillsOpen;
            if (skillsOpen)
            {
                player.GetComponent<SkillBook>().OpenBook();
            }
            else
            {
                player.GetComponent<SkillBook>().CloseBook();
            }
        }

        // Quickbar slot use
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            quickbar.UseSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            quickbar.UseSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            quickbar.UseSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            quickbar.UseSlot(3);
        }

        // Quickskill
        if(Input.GetKeyDown(KeyCode.Q))
        {
            skillsbar.UseSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            skillsbar.UseSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            skillsbar.UseSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            skillsbar.UseSlot(3);
        }
    }
}
