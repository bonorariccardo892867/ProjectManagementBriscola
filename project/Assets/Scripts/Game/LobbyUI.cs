using System;
using GameFramework.Core.GameFramework.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button backButton;

        [SerializeField] private TextMeshProUGUI lobbyCodeText;

        private void OnEnable(){
            if(GameLobbyManager.instance.IsHost)
                startButton.onClick.AddListener(OnStartClicked);
            backButton.onClick.AddListener(OnBackClicked);
        }

        private void OnDisable(){
            startButton.onClick.RemoveListener(OnStartClicked);
            backButton.onClick.RemoveListener(OnBackClicked);

        }

        void Start()
        {
            // Add the lobby code
            lobbyCodeText.text = GameLobbyManager.instance.GetLobbyCode();
            
        }

        void Update(){
            if(!GameLobbyManager.instance.IsHost || LobbyManager.instance.NumberOfPlayers != 2)
                startButton.gameObject.SetActive(false);
            else{
                startButton.gameObject.SetActive(true);
            }
        }

        // Method called when the start button is clicked
        private async void OnStartClicked(){
            await GameLobbyManager.instance.StartGame();
        }

        // Method called when the back button is clicked
        private void OnBackClicked(){
            LobbyManager.instance.Disconnection();
            SceneManager.LoadSceneAsync("MainMenu");
        }

    }
}
