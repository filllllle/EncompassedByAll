using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class MenuUI : MonoBehaviour
{

    [SerializeField]
    Button createButton;

    [SerializeField]
    Button joinButton;

    [SerializeField]
    Button optionsButton;

    [SerializeField]
    Button exitButton;

    [Space(20)]
    [Header("UI Field Scripts")]
    [SerializeField]
    OptionPanelUI optionPanel;

    [SerializeField]
    JoinRoomPanel joinRoomPanel;

    void Start()
    {
        createButton.onClick.AddListener(OnCreateButton);
        joinButton.onClick.AddListener(OnJoinButton);
        optionsButton.onClick.AddListener(OnOptionsButton);
        exitButton.onClick.AddListener(OnExitButton);

        optionPanel.MenuUI = this;
        joinRoomPanel.MenuUI = this;
    }

    public void OnCreateButton()
    {
        string roomCode = RoomCodeGenerator.GetRoomCode();
        bool requestSent = PhotonNetwork.CreateRoom(roomCode);

       
        if(!requestSent)
        {
            Debug.LogError("Request failed to send!");
        }
    }

    public void OnJoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    void OnJoinButton()
    {
        gameObject.SetActive(false);
        joinRoomPanel.gameObject.SetActive(true);
    }

    void OnOptionsButton()
    {
        gameObject.SetActive(false);
        optionPanel.gameObject.SetActive(true);
    }

    void OnExitButton()
    {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();   
#endif
    }
}
