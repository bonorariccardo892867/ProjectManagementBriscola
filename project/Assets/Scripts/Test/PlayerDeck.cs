using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<ScopaCard> deck = new List<ScopaCard>();
    public ScopaCard container;
    public int deckSize;
    public List<GameObject> cardDeck;

    public List<GameObject> playerHand;
    public List<ScopaCard> playerCard;
    public int playerCardCount;
    //create new pile
    //public List<GameObject> playerHand;
    public GameObject Hand;
    // Start is called before the first frame update
    void Start()
    {
        deckSize = deck.Count;
        for (int i=0; i<deck.Count;i++){
            deck[i] = ScopaCardDatabase.cardList[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        HideDeckCards();
        HideHandCards();
    }
    public void HideDeckCards(){
        if (deck.Count==0){cardDeck[0].SetActive(false);}
        else{
            for (int i = 0; i < cardDeck.Count; i++){
            cardDeck[i].SetActive(deck.Count>i*(40/deck.Count));
            //cardDeck[i].SetActive(false);
            }
        }
        
    }
    public void drawCard(){
        //pop last element and save it as a ScopaCard
        if (deck.Count>0 && playerCardCount<3){
        ScopaCard lastCard = deck[deck.Count-1];
        deck.RemoveAt(deck.Count-1);
        //playerCard.Add(lastCard);
        playerCardCount++;
        }
    }

    public void HideHandCards(){
        playerHand[0].SetActive(playerCardCount>0);
        playerHand[1].SetActive(playerCardCount>1);
        playerHand[2].SetActive(playerCardCount>2);

        /*for (int i = 0; i < playerHand.Count; i++){
            playerHand[i].SetActive(playerCard.Count>i);
        }*/
    }
    public void setHandCardId(){
        //playerHand[0].DisplayScopaCard.displayId = playerCard[0].status;
        //playerHand[0].getDisplayScopaCard().setDisplayId(playerCard[0].getStatus());
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
