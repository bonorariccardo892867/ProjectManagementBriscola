using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardToHand : MonoBehaviour
{
    public GameObject Hand;
    public GameObject HandCard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Hand = GameObject.Find("PlayerHand");
        HandCard.transform.SetParent(Hand.transform);
        HandCard.transform.localScale = Vector3.one;
        HandCard.transform.position = new Vector3(transform.position.x,transform.position.y,-48);
        HandCard.transform.eulerAngles = new Vector3(25,0,0);
    }
}
