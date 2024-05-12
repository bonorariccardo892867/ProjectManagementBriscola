using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PCounterScript : NetworkBehaviour
{
    private string pcounter1 = "Players in game\n 1/2";
    private string pcounter2 = "Players in game\n 2/2";
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf){
            if(NetworkServer.connections.Count == 1 && text.ToString() != pcounter1)
                text.text = pcounter1;
            else if(NetworkServer.connections.Count == 2 && text.ToString() != pcounter2)
                text.text = pcounter2;
        }
    }
}
