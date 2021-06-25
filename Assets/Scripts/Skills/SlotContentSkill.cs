using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


/*
 *  UI that contains a skill. Can be drag around.
 *  
 *  Print the cooldown of the skill on useage.
 * 
 */

public class SlotContentSkill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, ITooltip
{
 
    private Sprite empty;

    private Skill content;
    public Skill Content { get { return content; } set { content = value; icon.sprite = content.icon; } }
 
    private Image icon;

    private TextMeshProUGUI coolDownText;
   
    public Transform parentToReturnTo;

    private float nextUse = 0f;


    private void Awake()
    {
        icon = GetComponentInChildren<Image>();
        coolDownText = GetComponentInChildren<TextMeshProUGUI>();
        if (icon == null)
        {
            Debug.LogError("No icon found for slot in inventory !");
        }

    }
    private void Start()
    {
        empty = GameManager.itemSpriteDict["TRANSPARENCY"];
    }

    private void Update()
    {
        nextUse -= Time.deltaTime;

        if(nextUse > 0f)
        {
            coolDownText.text = nextUse.ToString("0.0");
            icon.color = Color.gray;
        }
        else
        {
            coolDownText.text = "";
            icon.color = Color.white;
        }
    }


    public void Use()
    {
        if (content != null && nextUse <= 0f)
        {
            content.Use();
            nextUse = content.CoolDown;
        }

       
    }
 

    public void EmptyContent()
    {
        content = null;
        icon.sprite = empty;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       // Instantiate a copy of the skill, if from Book
        parentToReturnTo = null;

        if (GetComponentInParent<SkillSlot>().Type == SkillSlot.SlotType.BOOK)
        {
           GameObject newSpell = Instantiate(gameObject, this.transform.parent);
            newSpell.GetComponent<SlotContentSkill>().Content = this.content;
        }
        this.transform.SetParent(transform.root);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(parentToReturnTo == null)
        {
            // Destruction of the object and all of its children
            int totalChildren = this.transform.childCount;
            for (int i = 0 ; i < totalChildren; i++)
            {
               
                Destroy(this.transform.GetChild(0).gameObject);
            }
            Destroy(this);
           
        }
        else
        {
           
                int childrenschildren = parentToReturnTo.transform.GetChild(0).childCount;
                for (int j = 0; j < childrenschildren; j++)
                {
                    Destroy(parentToReturnTo.transform.GetChild(0).GetChild(j).gameObject);
                }
                Destroy(parentToReturnTo.transform.GetChild(0).gameObject);
            
            

                transform.SetParent(parentToReturnTo);
            transform.localPosition = Vector3.zero;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Use();
        }
    }

    public string GetTooltip()
    {

        if (content != null)
        {
                return content.GetTooltip();
        }
        return null;
    }
}
