using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MenuScript : MonoBehaviour
{
    // NetworkManager
    public NetworkManager networkManager;

    // UI panels
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject joinPanel;
    public GameObject profilePanel;
    public GameObject rulesPanel;

    // Function to start hosting a game
    public void Host()
    {
        networkManager.StartHost();
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(true);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(false);
    }

    // Function to set IP address for joining a game
    public void SetIP(string ip)
    {
        networkManager.networkAddress = ip;
    }

    // Function to switch to the join menu
    public void JoinMenu(){
        menuPanel.SetActive(false);
        joinPanel.SetActive(true);
        gamePanel.SetActive(false);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(false);
    }

    // Function to join a game
    public void Join()
    {
        networkManager.StartClient();
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(true);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(false);
    }

    // Function to switch to the Profile section
    public void Profile()
    {
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(false);
        profilePanel.SetActive(true);
        rulesPanel.SetActive(false);
    }

    // Function to switch to the Rules section
    public void Rules(){
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(false);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(true);
    }

    // Function to go back to the main menu
    public void Back(){
        joinPanel.SetActive(false);
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(false);
    }

    // Function to stop hosting or disconnect from the server
    public void Stop()
    {
        if (networkManager.mode == NetworkManagerMode.Host)
        {
            // If hosting, stop hosting
            networkManager.StopHost();
        }
        if (networkManager.mode == NetworkManagerMode.ClientOnly)
        {
            // If client, stop client
            networkManager.StopClient();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        joinPanel.SetActive(false);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if server or client is active
        if (NetworkServer.active || NetworkClient.active)
        {
            gamePanel.SetActive(true);
        }
        else if(joinPanel.activeSelf) 
        {
            gamePanel.SetActive(false);
            menuPanel.SetActive(false);
            profilePanel.SetActive(false);
            rulesPanel.SetActive(false);
        }
        else if (profilePanel.activeSelf)
        {
            gamePanel.SetActive(false);
            menuPanel.SetActive(false);
            joinPanel.SetActive(false);
            rulesPanel.SetActive(false);
        }
        else if(rulesPanel.activeSelf){
            gamePanel.SetActive(false);
            menuPanel.SetActive(false);
            joinPanel.SetActive(false);
            profilePanel.SetActive(false);
        }
        else
        {
            gamePanel.SetActive(false);
            joinPanel.SetActive(false);
            menuPanel.SetActive(true);
            profilePanel.SetActive(false);
            rulesPanel.SetActive(false);
        }

    }
}
