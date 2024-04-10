using GameFramework.Core.GameFramework.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private TextMeshProUGUI lobbyCodeText;

        private void OnEnable(){
            if(GameLobbyManager.instance.IsHost)
                startButton.onClick.AddListener(OnStartClicked);
        }

        private void OnDisable(){
            startButton.onClick.RemoveListener(OnStartClicked);
        }

        void Start()
        {
            // Add the lobby code
            lobbyCodeText.text = GameLobbyManager.instance.GetLobbyCode();
            
        }

        void Update(){
            if(!GameLobbyManager.instance.IsHost || LobbyManager.instance.NumberOfPlayers != 2)
                startButton.gameObject.SetActive(false);
        }

        // Method called when the start button is clicked
        private async void OnStartClicked(){
            await GameLobbyManager.instance.StartGame();
        }

    }
}
