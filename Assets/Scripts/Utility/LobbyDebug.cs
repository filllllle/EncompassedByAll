#if DEBUG || UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;
using Photon.Pun;

public class LobbyDebug : MonoBehaviour
{
    private void OnEnable()
    {
        ImGuiUn.Layout += OnLayout;
    }

    private void OnDisable()
    {
        ImGuiUn.Layout -= OnLayout;
    }

    void OnLayout()
    {
        ImGui.Begin("Lobby Debug Info");

        Photon.Realtime.Room cr = PhotonNetwork.CurrentRoom;
        string[] players = new string[cr.PlayerCount];
        int i = 0;
        int cItem = 0;
        foreach (var p in cr.Players.Values)
        {
            players[i] = p.NickName;
            i++;
        }

        ImGui.ListBox(PhotonNetwork.CurrentRoom.Name, ref cItem, players, players.Length);

        ImGui.End();
    }
}

#endif