using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class PlayerManager : NetworkBehaviour {
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;
    public List<GameObject> cards_ = new List<GameObject>();

    public int DeckIndex{
        get {return cards_.Count-1;}
    }

    public override void OnStartClient(){
        base.OnStartClient();
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("OtherArea");
        DropZone = GameObject.Find("DropZone");
    }

    public override void OnStartServer(){
        base.OnStartServer();
    }

    private void Shuffle(){
        int n = cards_.Count;
        while(n > 1){
            int k = (int)Mathf.Floor(Random.value * (n--));
            GameObject temp = cards_[n];
            cards_[n] = cards_[k];
            cards_[k] = temp;
        }
    }

    [Command]
    public void CmdDealCards(int count){
        if(DeckIndex >= 0){
            for (int i=0;i<count;i++){
                GameObject card = Instantiate(cards_[Random.Range(0, DeckIndex)], new Vector2(0, 0), Quaternion.identity);
                NetworkServer.Spawn(card, connectionToClient);
                RpcShowCard(card,"dealt");
            }
        }
    }

    public void PlayCard(GameObject card){
        CmdPlayCard(card);
    }

    [Command]
    private void CmdPlayCard(GameObject card){
        RpcShowCard(card,"played");
        if (isServer){
            updateTurnsPlayed();
        }
    }
    [Server]
    void updateTurnsPlayed(){
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.updateTurnsPlayed();
        RpcLogToClients("TurnsPlayed: "+gm.turnsPlayed);
    }
    [ClientRpc]
    void RpcLogToClients(string message){
        Debug.Log(message);
    }


    [ClientRpc]
    private void RpcShowCard(GameObject card, string type){
        if (type == "dealt"){
            if (isOwned){
                card.transform.SetParent(PlayerArea.transform,false);
            }else{
                card.transform.SetParent(EnemyArea.transform,false);
                card.GetComponent<CardFlipper>().Flip();
            }
        }else if (type == "played"){
            card.transform.SetParent(DropZone.transform,false);
            if(!isOwned)
                card.GetComponent<CardFlipper>().Flip();
        }
    }


    [Command]
    public void CmdTargetSelfCard()
    {
        TargetSelfCard();
    }

    [Command]
    public void CmdTargetOtherCard(GameObject target) {
        NetworkIdentity opponentIdentity = target.GetComponent<NetworkIdentity>();
        TargetOtherCard(opponentIdentity.connectionToClient);
    }

    [TargetRpc]
    private void TargetSelfCard()
    {
        Debug.Log("target self Card");
    } 

    [TargetRpc]

    private void TargetOtherCard(NetworkConnection target)
    {
        Debug.Log("targeted other card");
    }

    [Command]
    public void CmdIncrementClick(GameObject card)
    {
        RpcIncrementClick(card);
    }

    [ClientRpc]
    private void RpcIncrementClick(GameObject card)
    {
        card.GetComponent<IncrementClick>().NumberOfClicks++;
        Debug.Log("This card has been clicked" + card.GetComponent<IncrementClick>().NumberOfClicks + " times!");
    }

}
