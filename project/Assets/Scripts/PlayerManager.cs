using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class PlayerManager : NetworkBehaviour {
    public List <GameObject> cards = new List<GameObject>();
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;

    public override void OnStartClient(){
        base.OnStartClient();
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("OtherArea");
        DropZone = GameObject.Find("DropZone");
    }
    
    [Command]
    public void CmdDealCards(){
        for (int i=0;i<3;i++){
            GameObject card = Instantiate(cards[Random.Range(0,cards.Count)],new Vector2(0,0),Quaternion.identity);
            NetworkServer.Spawn(card,connectionToClient);
            RpcShowCard(card,"dealt");
        }
    }

    public void PlayCard(GameObject card){
        CmdPlayCard(card);
    }

    [Command]
    void CmdPlayCard(GameObject card){
        RpcShowCard(card,"played");
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
    void RpcIncrementClick(GameObject card)
    {
        card.GetComponent<IncrementClick>().NumberOfClicks++;
        Debug.Log("This card has been clicked" + card.GetComponent<IncrementClick>().NumberOfClicks + " times!");
    }

}
