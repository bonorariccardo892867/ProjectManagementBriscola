using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopaCardDatabase : MonoBehaviour
{
   public static List<ScopaCard> cardList = new List<ScopaCard>();

   void Awake(){

      //research load all
    cardList.Add(new ScopaCard(0,Resources.Load<Sprite>("Sprite/denari3")));
    cardList.Add(new ScopaCard(1,Resources.Load<Sprite>("Sprite/denari4")));
    cardList.Add(new ScopaCard(2,Resources.Load<Sprite>("Sprite/denari5")));
    cardList.Add(new ScopaCard(3,Resources.Load<Sprite>("Sprite/denari6")));
    cardList.Add(new ScopaCard(4,Resources.Load<Sprite>("Sprite/denari7")));
    cardList.Add(new ScopaCard(5,Resources.Load<Sprite>("Sprite/denari8")));
    cardList.Add(new ScopaCard(6,Resources.Load<Sprite>("Sprite/denari9")));
   }
}
