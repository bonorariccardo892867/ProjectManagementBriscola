using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BriscolaScript : MonoBehaviour
{
    public void BriscolaCard(Sprite card){
        gameObject.transform.Find("Image").GetComponent<Image>().sprite = card;
    }
}
