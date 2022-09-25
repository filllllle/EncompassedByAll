using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinRoomPanel : MonoBehaviour
{
    public MenuUI MenuUI { get; set; }

    [SerializeField]
    TMP_InputField roomInputField;

    [SerializeField]
    Button joinRoomButton;

    private void OnEnable()
    {
        joinRoomButton.onClick.AddListener(JoinRoom);
    }

    private void OnDisable()
    {
        joinRoomButton.onClick.RemoveListener(JoinRoom);
    }

    void JoinRoom()
    {
        gameObject.SetActive(false);
        MenuUI.gameObject.SetActive(true);

        MenuUI.OnJoinRoom(roomInputField.text);
    }
}
