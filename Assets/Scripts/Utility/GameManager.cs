using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon.StructWrapping;
using System.Security.Cryptography;
using ImGuiNET;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [SerializeField]
    bool debugMode;
    public static bool DEBUG_MODE { get => Instance.debugMode; }

    #region Common Variables (Master Client and Clients)

    [SerializeField]
    SpawnPoints levelSpawnPoints;

    [SerializeField]
    Task[] availableTasks;

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

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Canvas worldUI;

    [SerializeField]
    Canvas playerUI;

    [SerializeField]
    float playerInteractRadius;


    public float PlayerInteractRadius { get => playerInteractRadius; set => playerInteractRadius = value; }


    private float killCooldown = 10;

    private int desiredImposters = 1;

    private int taskAmount = 1;

    public float KillCooldown { get => killCooldown; }
    public int DesiredImposters { get => desiredImposters; }
    public int TaskAmount { get => taskAmount; }

    public Canvas PlayerUI { get => playerUI; }

    #endregion

    #region Local Variables

    [SerializeField]
    EventSystem eventSystem;

    public GameObject localPlayer;

    public EventSystem EventSystem { get => eventSystem; }
    public Camera MainCamera { get => mainCamera; }
    public Canvas WorldUI { get => worldUI; }

    private int[] assignedTasks;

    public List<Task> LocalPlayerTasks
    {
        get
        {
            List<Task> tasks = new List<Task>();

            if (assignedTasks == null)
                return tasks;

            for (int i = 0; i < assignedTasks.Length; i++)
            {
                tasks.Add(availableTasks[i]);
            }

            return tasks;
        }
    }

    #endregion

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


        bool icb = PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("impostercount", out object ic);
        bool kcb = PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("killcooldown", out object kc);
        bool tcb = PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("taskcount", out object tc);

        killCooldown = System.Convert.ToInt32(kc as string);
        desiredImposters = System.Convert.ToInt32(ic as string);
        taskAmount = System.Convert.ToInt32(tc as string);

        int actornr = PhotonNetwork.LocalPlayer.ActorNumber;
        localPlayer = PhotonNetwork.Instantiate(playerPrefab.name, levelSpawnPoints.SpawnPositions[actornr].position, Quaternion.identity, 0);

        if (PhotonNetwork.IsMasterClient)
        {
            this.Invoke("GameStart", 0.5f);
        }
    }

    void GameStart()
    {

        Photon.Realtime.Room currentRoom = PhotonNetwork.CurrentRoom;
        int amountOfPlayers = currentRoom.Players.Count;

        int imposterCount = 0;

        if (amountOfPlayers > 6) imposterCount = 2;
        else imposterCount = 1;

        if (amountOfPlayers > desiredImposters)
            imposterCount = desiredImposters;



        int[] imposters = new int[imposterCount];
        for (int i = 0; i < imposterCount; i++)
        {
            imposters[i] = RandomNumberGenerator.GetInt32(0, amountOfPlayers);
            Debug.LogFormat("Making player {0} into imposter", imposters[i]);
        }

        var enumerator = currentRoom.Players.Values.GetEnumerator();
        enumerator.MoveNext();

        Debug.Log("Assigning roles now");
        for (int i = 0; i < amountOfPlayers; i++)
        {

            if (!imposters.Contains(i))
            {
                photonView.RPC("AssignRole", RpcTarget.All, enumerator.Current, PlayerRole.PlayerRoles.Crewmate);
                Debug.LogFormat("Sent {0} to {1}", PlayerRole.PlayerRoles.Crewmate, enumerator.Current.NickName);
            }
            else
            {
                photonView.RPC("AssignRole", RpcTarget.All, enumerator.Current, PlayerRole.PlayerRoles.Imposter);
                Debug.LogFormat("Sent {0} to {1}", PlayerRole.PlayerRoles.Imposter, enumerator.Current.NickName);
            }

            enumerator.MoveNext();
        }

        GenerateTasks();
    }

    void GenerateTasks()
    {
        foreach (Player p in allPlayers)
        {
            // In case player is NOT an imposter
            if (p.GetComponent<ImposterRole>() == null)
            {
                int[] playerTasks = new int[TaskAmount];

                for (int i = 0; i < TaskAmount; i++)
                {
                    if (availableTasks != null && availableTasks.Length != 0) playerTasks[i] = RandomNumberGenerator.GetInt32(0, availableTasks.Length);
                    else playerTasks[i] = -1;
                }

                photonView.RPC("AssignTasks", p.photonView.Owner, playerTasks as object);
            }
        }
    }

    void Update()
    {

    }

    #region Debug Window

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

        ImGui.Separator();

        

        List<Task> tasks = LocalPlayerTasks;
        
        for(int i = 0; i < tasks.Count; i++)
        {
            ImGui.Columns(3, "tasks", true);
            ImGui.SetColumnWidth(0, 50);
            ImGui.SetColumnWidth(1, 300);
            ImGui.SetColumnWidth(1, 100);

            ImGui.TextUnformatted((i + 1).ToString());

            ImGui.NextColumn();
            ImGui.TextUnformatted(tasks[i].TaskName);

            ImGui.NextColumn();
            ImGui.TextUnformatted("Resolved: " + tasks[i].IsResolved.ToString());

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

    #endregion

    [PunRPC]
    void AssignTasks(int[] taskNames)
    {
        string tasksRecieved = "Recieved tasks: ";

        foreach (int o in taskNames)
        {
            if (System.Convert.ToInt32(o) >= 0) tasksRecieved = string.Concat(tasksRecieved, ", ", availableTasks[o].TaskName);
            else tasksRecieved = string.Concat(tasksRecieved, ", ", "Incorrect Task");
        }

        assignedTasks = taskNames;
        for(int i = 0; i < assignedTasks.Length; i++)
        {
            availableTasks[i].SetAsUnresolved();
        }

        Debug.Log(tasksRecieved);
    }

    [PunRPC]
    void AssignRole(Photon.Realtime.Player player, PlayerRole.PlayerRoles playerRole, PhotonMessageInfo messageInfo)
    {
        GameObject p = player.TagObject as GameObject;
        p.SendMessage("RecieveRole", playerRole);
    }

    [PunRPC]
    void ResolveTask(int id, PhotonMessageInfo messageInfo)
    {

    }

}
