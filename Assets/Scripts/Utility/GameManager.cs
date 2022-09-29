using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks 
{
    public static GameManager Instance;

    [Tooltip("This is the prefab used for spawning in players")]
    public GameObject playerPrefab;

    private List<Player> allPlayers;
    public Player[] Players { get => allPlayers.ToArray(); }

    public static void AddPlayer(Player player)
    {
        Instance.allPlayers.Add(player);
    }

    public static void RemovePlayer(Player player)
    {
        Instance.allPlayers.Remove(player);
    }

    //public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    //{
    //    foreach(Player p in allPlayers)
    //    {
    //        if(p.PunPlayer == otherPlayer)
    //        {
    //            allPlayers.Remove(p);
    //        }
    //    }
    //}

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    void Start()
    {
        Instance = this;
        allPlayers = new List<Player>();

        if(playerPrefab == null)
        {
            Debug.LogError("Missing playerPrefab reference.");
        }

        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    void Update()
    {
        
    }
}
