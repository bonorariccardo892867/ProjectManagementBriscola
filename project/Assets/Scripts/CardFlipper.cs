using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    public Sprite CardFront;
    public Sprite CardBack;

    // Method to flip the card
    public void Flip(){
        // Get the current sprite of the card
        Sprite currentSprite = gameObject.transform.Find("Image").GetComponent<Image>().sprite;

        // Check if the current sprite is the front sprite, then change it to the back sprite, and vice versa
        if (currentSprite == CardFront){
            gameObject.transform.Find("Image").GetComponent<Image>().sprite = CardBack;
        }else{
            gameObject.transform.Find("Image").GetComponent<Image>().sprite = CardFront;
        }
    }
}
