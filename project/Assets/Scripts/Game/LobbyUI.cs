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
            if(!GameLobbyManager.instance.IsHost)
                startButton.gameObject.SetActive(false);

            // Add the lobby code
            lobbyCodeText.text = GameLobbyManager.instance.GetLobbyCode();
            
        }

        // Method called when the start button is clicked
        private void OnStartClicked(){
            Debug.Log("START!!!");
        }

    }
}
