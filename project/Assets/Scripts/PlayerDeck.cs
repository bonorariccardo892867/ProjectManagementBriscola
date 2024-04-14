using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<ScopaCard> deck = new List<ScopaCard>();

    public ScopaCard container;
    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<deck.Count;i++){
            deck[i] = ScopaCardDatabase.cardList[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shuffle(){
        for (int i=0;i<deck.Count;i++){
            container = deck[i];
            int randomIndex = Random.Range(i,deck.Count);
            deck[i]=deck[randomIndex];
            deck[randomIndex] = container;
        }
    }
}
