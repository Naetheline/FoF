using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * 
 *  UI slot that contains a skillContent.
 *  
 *  Is a drop zone for the skillContent that are draggable.
 * 
 */ 

public class SkillSlot : MonoBehaviour, IDropHandler
{
    public enum SlotType { BOOK, QUICKBAR}

    private SlotType type;
    public SlotType Type
    {
        get { return type; }
    }

    public void SetType(SlotType type)
    {
        this.type = type;
    }


    public void OnDrop(PointerEventData eventData)
    {
        
        SlotContentSkill draggedContent = eventData.pointerDrag.GetComponent<SlotContentSkill>();

        if (draggedContent == null)
        {
            return;
        }

        if(type == SlotType.QUICKBAR)
        {
            draggedContent.parentToReturnTo = this.transform;
        }

    }
}
