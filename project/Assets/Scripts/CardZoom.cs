using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class CardZoom : NetworkBehaviour
{
    public GameObject canvas;
    public GameObject ZoomCard;

    private GameObject zoomCard;
    private Sprite zoomSprite;
    // Start is called before the first frame update
    public void Awake()
    {
        canvas = GameObject.Find("Main Canvas");
        zoomSprite = gameObject.GetComponent<Image>().sprite;

    }
    public void OnHoverPointer(){
        if (!isOwned) return;
        zoomCard = Instantiate(ZoomCard,new Vector2(Input.mousePosition.x,Input.mousePosition.y+250),Quaternion.identity);
        zoomCard.GetComponent<Image>().sprite = zoomSprite;
        zoomCard.transform.SetParent(canvas.transform,true);
        RectTransform rect = zoomCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(240,354);
    }
    public void OnHoverExit(){
        Destroy(zoomCard);
    }
}
