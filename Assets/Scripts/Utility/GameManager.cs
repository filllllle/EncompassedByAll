using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon.StructWrapping;
using System.Security.Cryptography;
using ImGuiNET;

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

    public GameObject localPlayer;

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

        if (playerPrefab == null)
        {
            Debug.LogError("Missing playerPrefab reference.");
        }

        int actornr = PhotonNetwork.LocalPlayer.ActorNumber;
        localPlayer = PhotonNetwork.Instantiate(playerPrefab.name, levelSpawnPoints.SpawnPositions[actornr].position, Quaternion.identity, 0);

        if (PhotonNetwork.IsMasterClient)
        {
            this.Invoke("DelayedStart", 0.5f);
        }
    }

    void DelayedStart()
    {

        Photon.Realtime.Room currentRoom = PhotonNetwork.CurrentRoom;
        int amountOfPlayers = currentRoom.Players.Count;

        int imposterCount = 0;
        if (!currentRoom.CustomProperties.TryGetValue<int>("imposters", out imposterCount))
        {
            if (amountOfPlayers > 6) imposterCount = 2;
            else imposterCount = 1;
        }

        int[] imposters = new int[imposterCount];
        for (int i = 0; i < imposterCount; i++)
        {
            imposters[i] = RandomNumberGenerator.GetInt32(0, amountOfPlayers);
        }

        var enumerator = currentRoom.Players.Values.GetEnumerator();
        enumerator.MoveNext();

        Debug.Log("Assigning roles now");
        for (int i = 0; i < amountOfPlayers; i++)
        {
            Debug.LogFormat("Sent to {0}", enumerator.Current.NickName);

            if (!imposters.Contains(i)) photonView.RPC("AssignRole", RpcTarget.All, enumerator.Current, PlayerRole.PlayerRoles.Crewmate);
            else photonView.RPC("AssignRole", RpcTarget.All, enumerator.Current, PlayerRole.PlayerRoles.Imposter);

            enumerator.MoveNext();
        }

    }

    void Update()
    {

    }


#if DEBUG || UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN

    void DrawPlayerInformation()
    {
        var players = PhotonNetwork.CurrentRoom.Players.Values;

        ImGui.Begin("Debug Info");

        foreach (var p in players)
        {
            ImGui.Columns(3, "player_info", true);
            ImGui.SetColumnWidth(0, 100f);
            ImGui.SetColumnWidth(1, 200f);
            ImGui.SetColumnWidth(2, 100f);

            ImGui.TextUnformatted(p.NickName);
            ImGui.NextColumn();

            GameObject playerObj = p.TagObject as GameObject;

            PlayerRole role = null;
            if (playerObj != null)
            {
                role = playerObj.GetComponent<PlayerRole>();
            }
            if (role != null)
            {
                ImGui.TextUnformatted(string.Format("Role: {0}", role.GetPlayerRole()));
            }
            else
            {
                ImGui.TextUnformatted("Role: Unknown");
            }

            ImGui.NextColumn();
            ImGui.TextUnformatted("Test");

            ImGui.NextColumn();
        }
        ImGui.End();
    }

#endif

    public override void OnEnable()
    {
#if DEBUG || UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
        ImGuiUn.Layout += DrawPlayerInformation;
#endif
        base.OnEnable();
    }

    public override void OnDisable()
    {
#if DEBUG || UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
        ImGuiUn.Layout -= DrawPlayerInformation;
#endif        
        base.OnEnable();
    }

    [PunRPC]
    void AssignRole(Photon.Realtime.Player player, PlayerRole.PlayerRoles playerRole, PhotonMessageInfo messageInfo)
    {
        GameObject p = player.TagObject as GameObject;
        p.SendMessage("RecieveRole", playerRole);
    }
}
