using System;
using System.Collections.Generic;

namespace CompareSkills;

public class XUiC_PlayerSelector : XUiController
{
    private PlayerSelectorFilter filter = PlayerSelectorFilter.All;
    public DisplayPlayer[] players = [];
    private int currentIndex = 0;
    private bool isDebugMode;

    public DisplayPlayer? SelectedPlayer => players.Length > 0 ? players[currentIndex] : null;

    public event EventHandler? SelectedPlayerChanged;

    public override void Init()
    {
        base.Init();
        GetChildById("prevPlayer").OnPress += (s, m) => PrevPlayer();
        GetChildById("nextPlayer").OnPress += (s, m) => NextPlayer();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        RefreshPlayerList();
    }

    public override bool ParseAttribute(string _name, string _value, XUiController _parent)
    {
        switch (_name)
        {
            case "filter":
                if (Enum.TryParse<PlayerSelectorFilter>(_value, true, out PlayerSelectorFilter result))
                {
                    filter = result;
                }

                return true;

            default:
                return base.ParseAttribute(_name, _value, _parent);
        }
    }

    private void RefreshPlayerList()
    {
        players = GetPlayerList();
        currentIndex = 0;
        RefreshBindings();
        SelectedPlayerChanged?.Invoke(this, EventArgs.Empty);
    }

    private DisplayPlayer[] GetPlayerList()
    {
        int ownerEntityId = xui.playerUI.entityPlayer.entityId;
        List<DisplayPlayer> players = [];

        foreach (EntityPlayer player in GameManager.Instance.World.Players.list)
        {
            if (!isDebugMode)
            {
                if (player.entityId == ownerEntityId)
                {
                    continue;
                }

                if (filter == PlayerSelectorFilter.Ally && !player.IsFriendOfLocalPlayer)
                {
                    continue;
                }

                if (filter == PlayerSelectorFilter.Party && !player.IsInPartyOfLocalPlayer)
                {
                    continue;
                }
            }

            players.Add(new DisplayPlayer(player.entityId, player.PlayerDisplayName));
        }

        return players.ToArray();
    }

    private void PrevPlayer()
    {
        if (players.Length <= 1)
        {
            return;
        }

        int newIndex = (currentIndex - 1 + players.Length) % players.Length;
        
        if (currentIndex == newIndex)
        {
            return;
        }

        currentIndex = newIndex;
        RefreshBindings();
        SelectedPlayerChanged?.Invoke(this, EventArgs.Empty);
    }

    private void NextPlayer()
    {
        if (players.Length <= 1)
        {
            return;
        }

        int newIndex = (currentIndex + 1 + players.Length) % players.Length;

        if (currentIndex == newIndex)
        {
            return;
        }

        currentIndex = newIndex;
        RefreshBindings();
        SelectedPlayerChanged?.Invoke(this, EventArgs.Empty);
    }

    public override bool GetBindingValueInternal(ref string value, string bindingName)
    {
        switch (bindingName)
        {
            case "compareplayername":
                value = SelectedPlayer?.Name ?? "No one";
                return true;

            case "hasprevplayer":
                value = (players.Length > 1).ToString();
                return true;

            case "hasnextplayer":
                value = (players.Length > 1).ToString();
                return true;

            default:
                return base.GetBindingValueInternal(ref value, bindingName);
        }
    }

    public void ToggleDebugMode()
    {
        isDebugMode = !isDebugMode;
        RefreshPlayerList();
    }
}

public enum PlayerSelectorFilter
{
    All,
    Ally,
    Party
}