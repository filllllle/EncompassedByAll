using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class EncompassedByAll : MonoBehaviourPunCallbacks
{
    public static EncompassedByAll Instance = null;

    [SerializeField]
    string minorGameVersion = "1";

    [SerializeField]
    string majorGameVersion = "0";


    string playerName;
    public string PlayerName
    {
        get => playerName;
        set
        {
            playerName = value;
            PhotonNetwork.NickName = playerName;
        }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            PhotonNetwork.GameVersion = string.Format("{0}.{1}", majorGameVersion, minorGameVersion);
            PhotonNetwork.AutomaticallySyncScene = true;

            Debug.LogFormat("Game Version: {0}", PhotonNetwork.GameVersion);

            PlayerName = "Player";
        }
        else
        { 
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PhotonNetwork.ConnectUsingSettings())
        {
            Debug.Log("Connected?");
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster()");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected() - reason {0}", cause);
    }

    public override void OnCreatedRoom()
    {
        SceneManager.LoadScene(1);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarningFormat("Failed to create room. Code:{0}. Msg:{1}", returnCode, message);
        MenuUI menuUI = GameObject.FindObjectOfType<MenuUI>();

        if(menuUI!=null)
        {
            menuUI.OnCreateButton();
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        
    }

    
}
