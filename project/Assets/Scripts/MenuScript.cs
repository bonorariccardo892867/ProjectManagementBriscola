﻿using System.Collections;
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

    // Function to start hosting a game
    public void Host()
    {
        networkManager.StartHost();
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(true);
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
    }

    // Function to join a game
    public void Join()
    {
        networkManager.StartClient();
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    // Function to go back to the main menu
    public void Back(){
        joinPanel.SetActive(false);
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
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
    }

    // Update is called once per frame
    void Update()
    {
        // Check if server or client is active
        if (NetworkServer.active || NetworkClient.active)
        {
            gamePanel.SetActive(true);
        } // If join panel is active
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