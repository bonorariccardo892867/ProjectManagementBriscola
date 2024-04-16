using System;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game{
    public class MainMenu : MonoBehaviour
    {
        // Declaration of variables representing various UI elements
        [SerializeField] private GameObject mainScreen;
        [SerializeField] private GameObject joinScreen;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI codeText;

        private void OnEnable(){
            hostButton.onClick.AddListener(OnHostClicked);
            joinButton.onClick.AddListener(OnJoinClicked);
            submitButton.onClick.AddListener(OnSubmitClicked);
            backButton.onClick.AddListener(OnBackClicked);

        }

        private void OnDisable(){
            hostButton.onClick.RemoveListener(OnHostClicked);
            joinButton.onClick.RemoveListener(OnJoinClicked);
            submitButton.onClick.RemoveListener(OnSubmitClicked);
            backButton.onClick.RemoveListener(OnBackClicked);

        }

        // Method called when the join button is clicked
        private void OnJoinClicked(){
            mainScreen.SetActive(false);
            joinScreen.SetActive(true);
        }

        // Method called when the create button is clicked
        private async void OnHostClicked(){
            bool succeeded = await GameLobbyManager.instance.CreateLobby();
            if(succeeded)
                SceneManager.LoadSceneAsync("Lobby");
        }

        // Method called when the submit button is clicked
        private async void OnSubmitClicked(){
            string code = codeText.text;
            code = code.Substring(0, code.Length - 1);

            bool succeeded = await GameLobbyManager.instance.JoinLobby(code);
            if(succeeded)
                SceneManager.LoadSceneAsync("Lobby");
        }

        private void OnBackClicked()
        {
            mainScreen.SetActive(true);
            joinScreen.SetActive(false);
        }
    }
}