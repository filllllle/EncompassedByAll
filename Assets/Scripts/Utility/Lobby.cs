using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class Lobby : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI connectLabel;

    [SerializeField]
    Button startButton;


    private void Start()
    {
        if(PhotonNetwork.InRoom)
        {
            connectLabel.text = PhotonNetwork.CurrentRoom.Name;
            startButton.onClick.AddListener(OnStartClicked);
            SceneManager.sceneLoaded += EncompassedByAll.Instance.OnSceneLoaded;
        }
        else
        {
            connectLabel.text = "Error";
        }

        if(!PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(false);
        }
    }

    void OnStartClicked()
    {
        PhotonNetwork.LoadLevel(2);
        //SceneManager.LoadScene(2);
    }
}
