using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MenuScript : MonoBehaviour
{

    public NetworkManager networkManager;
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject joinPanel;

    public void Host()
    {
        networkManager.StartHost();
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void SetIP(string ip)
    {
        networkManager.networkAddress = ip;
    }

    public void JoinMenu(){
        menuPanel.SetActive(false);
        joinPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void Join()
    {
        networkManager.StartClient();
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void Back(){
        joinPanel.SetActive(false);
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void Stop()
    {
        if (networkManager.mode == NetworkManagerMode.Host)
        {
            networkManager.StopHost();
        }
        if (networkManager.mode == NetworkManagerMode.ClientOnly)
        {
            networkManager.StopClient();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        joinPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkServer.active || NetworkClient.active)
        {
            gamePanel.SetActive(true);
        }
        else if(joinPanel.activeSelf)
        {
            gamePanel.SetActive(false);
            menuPanel.SetActive(false);
        }
        else
        {
            gamePanel.SetActive(false);
            joinPanel.SetActive(false);
            menuPanel.SetActive(true);
        }

    }
}
