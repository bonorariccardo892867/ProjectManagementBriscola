using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    public GameObject Card1;
    public GameObject PlayerArea;
    // Start is called before the first frame update
    public void OnCLick(){
        for (int i=0;i<5;i++){
            GameObject card = Instantiate(Card1,new Vector2(0,0),Quaternion.identity);
            card.transform.SetParent(PlayerArea.transform,false);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
