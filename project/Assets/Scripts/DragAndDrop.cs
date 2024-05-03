using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragAndDrop : NetworkBehaviour
{
    public GameObject Canvas;
    public PlayerManager playerManager;
    private bool isDragging = false;
    private bool isDraggable = true;
    private GameObject startParent;
    private Vector2 startposition;
    private GameObject dropZone;
    private bool isOverDropZone;
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Main Canvas");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Called when the card collides with another object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }

    // Called when the card stops colliding with another object
    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;
    }

    // Method called when the player starts dragging the card
    public void StartDrag(){
        if (!isDraggable) return;
        isDragging=true;
        startParent = transform.parent.gameObject;
        startposition = transform.position;

    }

    // Method called when the player stops dragging the card
    public void EndDrag(){
        if (!isDraggable) return;
        isDragging=false;
        if (isOverDropZone)
        {
            // Move the card to the drop zone and play the card using the PlayerManager
            transform.SetParent(dropZone.transform, false);
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerManager = networkIdentity.GetComponent<PlayerManager>();
            playerManager.PlayCard(gameObject);
        }else
        {
            // Move the card back to its starting position and parent
            transform.position = startposition;
            transform.SetParent(startParent.transform,false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject parent = transform.parent.gameObject;
        if ((!isDragging && parent != null && parent.name != "PlayerArea") || (gameObject.GetComponent<CardValues>().player != gm.turnsPlayed))
            isDraggable = false;
        else
            isDraggable = true;

        if (isDragging){
            transform.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
            transform.SetParent(Canvas.transform,true);
        }
    }
}
