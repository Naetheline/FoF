using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

/*
 * Base classe for the interactables objects
 * 
 * Let the player move in range for the interaction, then launch the interaction.
 * Sub-classes must override at least Interact.
 */

public class Interactable : MonoBehaviour
{

    public const float DEFAULT_STOPPING_DISTANCE = 0.5f;

    private bool hasInteracted = true;
    private CharacterControl character;
    private PlayerStats player;

    public virtual void MoveToInteraction(CharacterControl cc, PlayerStats player)
    {
        this.character = cc;
        this.player = player;
        hasInteracted = false;
        cc.SetTarget(this.transform.position);
        cc.agent.stoppingDistance = DEFAULT_STOPPING_DISTANCE;
        
    }

    private void Update()
    {
        if (character != null && !character.agent.pathPending)
        {
            if (!hasInteracted &&  Vector3.Distance(character.transform.position, this.transform.position) <= character.agent.stoppingDistance)
            {
                //character.transform.position = this.transform.position;
                Interact(character, player);
            }
        }
    }



    public virtual void  Interact(CharacterControl cc, PlayerStats player)
    {
        hasInteracted = true;
    }

}
