using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks 
{
    public static GameManager Instance;

    [SerializeField]
    bool debugMode;

    [SerializeField]
    SpawnPoints levelSpawnPoints;

    [Tooltip("This is the prefab used for spawning in players")]
    public GameObject playerPrefab;

    public static bool DEBUG_MODE { get => Instance.debugMode; }

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

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    float playerInteractRadius;

    public Camera MainCamera { get => mainCamera; }

    public float PlayerInteractRadius { get => playerInteractRadius; set => playerInteractRadius = value; }

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

        PhotonNetwork.Instantiate(playerPrefab.name, levelSpawnPoints.SpawnPositions[0].position, Quaternion.identity, 0);
    }

    void Update()
    {
        
    }


#if DEBUG || UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN

    void DrawPlayerInformation(Player player)
    {

    }

#endif
}
