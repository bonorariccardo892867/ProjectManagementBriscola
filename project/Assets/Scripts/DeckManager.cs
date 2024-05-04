using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DeckManager : NetworkBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    private int deckIndex;
    private int briscolaSuit;

    private void Start() {
        deckIndex = cards.Count-1;
    }

    public int GetIndex(){
        return deckIndex;
    }

    public GameObject GetCard(){
        return cards[deckIndex--];
    }

    public GameObject GetCardWithoutDecrement(){
        return cards[deckIndex];
    }

    public void RemoveAt(int index){
        cards.RemoveAt(index);
    }

    public void Insert(int index, GameObject card){
        cards.Insert(index, card);
    }

    public void SetBriscolaSuit(int suit){
        briscolaSuit = suit;
    }

    public int GetBriscolaSuit(){
        return briscolaSuit;
    }
    // Method to shuffle the cards in the deck
    public void Shuffle(){
        int n = cards.Count;
        while(n > 1){
            int k = (int)Mathf.Floor(Random.value * (n--));
            GameObject temp = cards[n];
            cards[n] = cards[k];
            cards[k] = temp;
        }
    }
}
