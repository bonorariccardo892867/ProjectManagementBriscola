using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject  DropZone;
    private bool isDragging = false;
    private GameObject startParent;
    private Vector2 startposition;
    private GameObject dropZone;
    private bool isOverDropZone;
    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Main Canvas");
        DropZone = GameObject.Find("DropZone");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colliding!");
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("uncolliding!");
        isOverDropZone = false;
        dropZone = null;
    }
    public void StartDrag(){
        isDragging=true;
        startParent = transform.parent.gameObject;
        startposition = transform.position;

    }
    public void EndDrag(){
        isDragging=false;
        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
        }else
        {
            transform.position = startposition;
            transform.SetParent(startParent.transform,false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isDragging){
            transform.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
            transform.SetParent(Canvas.transform,true);
        }
    }
}
