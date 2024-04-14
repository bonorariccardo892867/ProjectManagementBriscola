using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ScopaCard 
{
    public int status;
    public Sprite spriteImg;
    

    public ScopaCard(){

    }
    public ScopaCard(int status, Sprite spriteImg){
        this.status = status;
        this.spriteImg = spriteImg;
    }
}
