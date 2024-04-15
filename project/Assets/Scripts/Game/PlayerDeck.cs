using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<ScopaCard> deck = new List<ScopaCard>();
    public ScopaCard container;
    public List<GameObject> cardDeck;
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
        HideDeckCards();
    }
    void HideDeckCards(){
        for (int i = 0; i < cardDeck.Count; i++){
            cardDeck[i].SetActive(deck.Count>i*(40/cardDeck.Count));
        }
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
