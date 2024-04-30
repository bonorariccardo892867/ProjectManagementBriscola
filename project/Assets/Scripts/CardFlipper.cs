using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    public Sprite CardFront;
    public Sprite CardBack;
    // Start is called before the first frame update
    public void Flip(){
        Sprite currentSprite = gameObject.transform.Find("Image").GetComponent<Image>().sprite;

        if (currentSprite == CardFront){
            gameObject.transform.Find("Image").GetComponent<Image>().sprite = CardBack;
        }else{
            gameObject.transform.Find("Image").GetComponent<Image>().sprite = CardFront;
        }
    }
}
