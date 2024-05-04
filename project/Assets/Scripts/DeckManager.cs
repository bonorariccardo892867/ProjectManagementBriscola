using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DeckManager : NetworkBehaviour
{
    // List to store the cards in the deck
    public List<GameObject> cards = new List<GameObject>();

    // Index of the card at the top of the deck
    private int deckIndex;

    // The suit of the "briscola" card
    private int briscolaSuit;

    private void Start() {
        // Initialize the deck index to the last card in the deck
        deckIndex = cards.Count-1;
    }

    // Method to get the index
    public int GetIndex(){
        return deckIndex;
    }

    // Method to get the card at the top of the deck decrementing the index
    public GameObject GetCard(){
        return cards[deckIndex--];
    }

    // Method to get the card at the top of the deck without decrementing the index
    public GameObject GetCardWithoutDecrement(){
        return cards[deckIndex];
    }

    // Method to remove a card from the deck at a specific index
    public void RemoveAt(int index){
        cards.RemoveAt(index);
    }

    // Method to insert a card into the deck at a specific index
    public void Insert(int index, GameObject card){
        cards.Insert(index, card);
    }

    // Method to set the suit of the "briscola" card
    public void SetBriscolaSuit(int suit){
        briscolaSuit = suit;
    }

    // Method to get the suit of the "briscola" card
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
