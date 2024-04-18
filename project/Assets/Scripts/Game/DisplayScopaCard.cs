using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayScopaCard : MonoBehaviour
{
    public static ScopaCard displayCard;
    public static int displayId;
    public Sprite spriteImg;
    
    public Image artImg; 
    public bool cardBack;
    public static bool staticCardBack;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
{

    displayCard = ScopaCardDatabase.cardList[displayId];

    spriteImg = displayCard.spriteImg;

    //artImg = GetComponent<Image>();
    artImg.sprite = spriteImg;

    staticCardBack = cardBack;   

}
}