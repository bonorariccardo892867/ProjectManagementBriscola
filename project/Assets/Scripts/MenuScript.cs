using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MenuScript : MonoBehaviour
{
    // NetworkManager
    public NetworkManager networkManager;

    // UI panels
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject drawPanel;
    public GameObject joinPanel;
    public GameObject profilePanel;
    public GameObject rulesPanel;

    // Function to start hosting a game
    public void Host()
    {
        networkManager.StartHost();
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(false);
        drawPanel.SetActive(false);
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
        drawPanel.SetActive(false);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(false);
    }

    // Function to join a game
    public void Join()
    {
        networkManager.StartClient();
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(false);
        drawPanel.SetActive(false);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(false);
    }

    // Function to switch to the Profile section
    public void Profile()
    {
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(false);
        drawPanel.SetActive(false);
        profilePanel.SetActive(true);
        rulesPanel.SetActive(false);
    }

    // Function to switch to the Rules section
    public void Rules(){
        menuPanel.SetActive(false);
        joinPanel.SetActive(false);
        gamePanel.SetActive(false);
        drawPanel.SetActive(false);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(true);
    }

    // Function to go back to the main menu
    public void Back(){
        gamePanel.SetActive(false);
        drawPanel.SetActive(false);
        joinPanel.SetActive(false);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(false);
        menuPanel.SetActive(true);
    }


    // Function to stop hosting or disconnect from the server
    public void Stop()
    {
        if (networkManager.mode == NetworkManagerMode.Host)
        {
            // If hosting, stop hosting
            networkManager.StopHost();
            gamePanel.SetActive(false);
            drawPanel.SetActive(false);
        }
        if (networkManager.mode == NetworkManagerMode.ClientOnly)
        {
            // If client, stop client
            networkManager.StopClient();
            gamePanel.SetActive(false);
            drawPanel.SetActive(false);
        }
    }

    public void FinalResults(string winner, string score){
        joinPanel.SetActive(false);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(false);
        menuPanel.SetActive(false);

        if(winner == ""){
            gamePanel.SetActive(false);
            drawPanel.SetActive(true);
            PlayerPrefs.SetInt("draw", PlayerPrefs.GetInt("draw")+1);
            GameObject.Find("ScoreTextD").GetComponent<TextMeshProUGUI>().text = score;
        }else{
            drawPanel.SetActive(false);
            gamePanel.SetActive(true);
            if(winner == "YOU")
                PlayerPrefs.SetInt("win", PlayerPrefs.GetInt("win")+1);
            else
                PlayerPrefs.SetInt("lose", PlayerPrefs.GetInt("lose")+1);
            GameObject.Find("WinnerText").GetComponent<TextMeshProUGUI>().text = winner;
            GameObject.Find("ScoreTextW").GetComponent<TextMeshProUGUI>().text = score;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        drawPanel.SetActive(false);
        joinPanel.SetActive(false);
        profilePanel.SetActive(false);
        rulesPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(joinPanel.activeSelf) 
        {
            gamePanel.SetActive(false);
            menuPanel.SetActive(false);
            drawPanel.SetActive(false);
            profilePanel.SetActive(false);
            rulesPanel.SetActive(false);
        }
        else if (profilePanel.activeSelf)
        {
            gamePanel.SetActive(false);
            menuPanel.SetActive(false);
            drawPanel.SetActive(false);
            joinPanel.SetActive(false);
            rulesPanel.SetActive(false);
        }
        else if(rulesPanel.activeSelf){
            gamePanel.SetActive(false);
            menuPanel.SetActive(false);
            drawPanel.SetActive(false);
            joinPanel.SetActive(false);
            profilePanel.SetActive(false);
        }
        else if(gamePanel.activeSelf)
        {
            rulesPanel.SetActive(false);
            menuPanel.SetActive(false);
            joinPanel.SetActive(false);
            profilePanel.SetActive(false);
            drawPanel.SetActive(false);
        }
        else if(drawPanel.activeSelf)
        {
            gamePanel.SetActive(false);
            joinPanel.SetActive(false);
            profilePanel.SetActive(false);
            rulesPanel.SetActive(false);
            menuPanel.SetActive(false);
        } 
        else if(menuPanel.activeSelf)
        {
            gamePanel.SetActive(false);
            joinPanel.SetActive(false);
            profilePanel.SetActive(false);
            rulesPanel.SetActive(false);
            drawPanel.SetActive(false);
        }
    }
}
