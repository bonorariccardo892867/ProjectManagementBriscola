using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Unity.VisualScripting;


public class PlayerManager : NetworkBehaviour {
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;
    public GameObject Briscola;
    public GameObject TurnDisplay;
    public GameManager gm;
    public DeckManager deck;

    // Player identifier and whether the player is the winner
    public string player;
    private bool win;

    public override void OnStartClient(){
        base.OnStartClient();
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("OtherArea");
        DropZone = GameObject.Find("DropZone");

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (isServer){
            win = true;
            player = "P1";
        }else{
            win = false;
            player = "P2";
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        deck = GameObject.Find("DeckManager").GetComponent<DeckManager>();
    }


    [Server]
    // Method to shuffle the cards in the deck
    public void Shuffle(){
        deck.Shuffle();
    }

    // Method called by the server to deal cards to players
    [Server]
    public void DealCards(int index){
        if(index%2 == 0 && deck.GetIndex()-index >= -1){
            for(int i=0; i<index; i++){
                GameObject card = Instantiate(deck.GetCard(), new Vector2(0, 0), Quaternion.identity);
                card.GetComponent<CardValues>().player = (win && i < index/2) || (!win && i >= index/2) ? "P1" : "P2";
                NetworkServer.Spawn(card, connectionToClient);
                RpcShowCard(card, "dealt", card.GetComponent<CardValues>().player);
            }
        }
    }

    [ClientRpc]
    private void UpdateCounter(GameObject card, int index){
        card.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = index.ToString();
    }

    [Server]
    public void SetBriscola(Vector2 position){
        GameObject briscola = deck.GetCardWithoutDecrement();
        deck.RemoveAt(deck.GetIndex());
        deck.Insert(0, briscola);
        deck.SetBriscolaSuit(briscola.GetComponent<CardValues>().suit);

        Briscola = Instantiate(Briscola, position, Quaternion.identity);
        TurnDisplay = Instantiate(TurnDisplay);
        NetworkServer.Spawn(Briscola, connectionToClient);
        NetworkServer.Spawn(TurnDisplay, connectionToClient);

        RpcSetBriscola(Briscola, briscola.GetComponent<CardFlipper>().CardFront.name, deck.GetIndex()+1);
        
    }

    [ClientRpc]
    private void RpcSetBriscola(GameObject card, string spriteName, int index){
        Transform canvas = GameObject.Find("Main Canvas").transform;
        card.GetComponent<BriscolaScript>().BriscolaCard(Resources.Load<Sprite>("Sprite/" + spriteName));
        card.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = index.ToString();
        card.transform.SetParent(canvas);
    }

    // Method to show the card on the game scene
    [ClientRpc]
    private void RpcShowCard(GameObject card, string type, string player_){
        if (type == "dealt"){
            card.GetComponent<CardValues>().player = player_;
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
        UpdateTurnsPlayed();
    }

    [Server]
    private void UpdateTurnsPlayed()
    {
        RpcUpdateTurn();
        gm.UpdateTurnsPlayed("P1");
        if(gm.turnsPlayed == "P"){
            Invoke("Take", 1.5f);
        }
    }

    [Server]
    private void Take(){
        ChooseRoundWinner();
        DeleteCards();
        UpdateCounter(GameObject.Find("Main Canvas").transform.Find("Briscola(Clone)").gameObject, deck.GetIndex()+1);
    }

    [Server]
    private void ChooseRoundWinner(){
        CardValues winner = null;
        CardValues loser = null;
        foreach (Transform child in DropZone.GetComponent<GridLayoutGroup>().transform)
        {
            if(winner == null)
                winner = child.gameObject.GetComponent<CardValues>();
            else
                loser = child.gameObject.GetComponent<CardValues>();
        }

        if(winner.suit == deck.GetBriscolaSuit() && loser.suit == deck.GetBriscolaSuit()){
            if(winner.number < loser.number)
                winner = loser;
        }else if(winner.suit != deck.GetBriscolaSuit() && loser.suit != deck.GetBriscolaSuit()){
            if(winner.suit == loser.suit && winner.number < loser.number)
                winner = loser;
        }else if(loser.suit == deck.GetBriscolaSuit()){
            winner = loser;
        }

        win = winner.player == player;
        RpcUpdateRoundWinnner(!win);
    }

    [Server]
    private void DeleteCards(){
        foreach (Transform child in DropZone.GetComponent<GridLayoutGroup>().transform)
        {
            NetworkServer.Destroy(child.gameObject);
        }
        RpcUpdateTurn();
        gm.UpdateTurnsPlayed("P1", win);
        DealCards(2);
    }

    [ClientRpc]
    private void RpcUpdateTurn()
    {
        if(!isServer){
            gm.UpdateTurnsPlayed("P2", win);
        }
    }

    [ClientRpc]
    private void RpcUpdateRoundWinnner(bool isWinner){
        if(!isServer)
            win = isWinner;
    }
}
