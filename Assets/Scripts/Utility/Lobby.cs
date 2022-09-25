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
    }

    void OnStartClicked()
    {
        SceneManager.LoadScene(2);
    }
}
