using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class PlayerManager : NetworkBehaviour {
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;
    public GameObject Briscola;

    // List of cards in the deck and deck index
    public List<GameObject> cards_ = new List<GameObject>();
    private int deckIndex = 39;

    // Player identifier and whether the player is the winner
    public string player;
    private bool win;
    private int briscolaSuit;

    public override void OnStartClient(){
        base.OnStartClient();
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("OtherArea");
        DropZone = GameObject.Find("DropZone");

        if(isServer){
            win = true;
            player = "P1";
        }else{
            win = false;
            player = "P2";
        }
    }

    public override void OnStartServer(){
        base.OnStartServer();

        // Shuffle the deck twice when the server starts
        Shuffle();
        Shuffle();
    }

    // Method to shuffle the cards in the deck
    private void Shuffle(){
        int n = cards_.Count;
        while(n > 1){
            int k = (int)Mathf.Floor(Random.value * (n--));
            GameObject temp = cards_[n];
            cards_[n] = cards_[k];
            cards_[k] = temp;
        }
    }

    // Method called by the server to deal cards to players
    [Server]
    public void DealCards(){
        for(int i=0; i<6; i++){
            GameObject card = Instantiate(cards_[deckIndex--], new Vector2(0, 0), Quaternion.identity);
            if(win && i<3){
                card.GetComponent<CardValues>().player = "P1";
            }else{
                card.GetComponent<CardValues>().player = "P2";
            }
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "dealt", card.GetComponent<CardValues>().player);
        }
    }

    [Server]
    public void SetBriscola(Vector2 position){
        GameObject briscola = cards_[deckIndex];
        cards_.RemoveAt(deckIndex);
        cards_.Insert(0, briscola);
        briscolaSuit = briscola.GetComponent<CardValues>().suit;

        Briscola = Instantiate(Briscola, position, Quaternion.identity);
        NetworkServer.Spawn(Briscola, connectionToClient);

        RpcSetBriscola(Briscola, briscola.GetComponent<CardFlipper>().CardFront.name);
    }

    [ClientRpc]
    private void RpcSetBriscola(GameObject card, string spriteName){
        card.GetComponent<BriscolaScript>().BriscolaCard(Resources.Load<Sprite>("Sprite/" + spriteName));
        card.transform.SetParent(GameObject.Find("Main Canvas").transform);
    }

    // Method to show the card on the game scene
    [ClientRpc]
    private void RpcShowCard(GameObject card, string type, string player_){
        if (type == "dealt"){
            if (player_ == player){
                card.transform.SetParent(PlayerArea.transform,false);
            }else{
                card.transform.SetParent(EnemyArea.transform,false);
                card.GetComponent<CardFlipper>().Flip();
            }
        }else if (type == "played"){
            card.transform.SetParent(DropZone.transform,false);
            if(player_ != player)
                card.GetComponent<CardFlipper>().Flip();
        }
    }

    // Method called when a player plays a card
    public void PlayCard(GameObject card){
        CmdPlayCard(card);
    }

    [Command]
    private void CmdPlayCard(GameObject card){
        RpcShowCard(card, "played", card.GetComponent<CardValues>().player);
    }
}
