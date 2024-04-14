using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayScopaCard : MonoBehaviour
{
    public static ScopaCard displayCard = new ScopaCard();
    public int displayId;
    public Sprite spriteImg;
    //
    public Image artImg; 


    // Start is called before the first frame update
    void Start()
    {
        displayCard = ScopaCardDatabase.cardList[displayId];

        spriteImg = displayCard.spriteImg;

        //artImg = GetComponent<Image>();
        artImg.sprite = spriteImg;
    }

    // Update is called once per frame
    void Update()
{
    

}
}