using System;
using HarmonyLib;

namespace CompareSkills;

[HarmonyPatch(typeof(GameManager), nameof(GameManager.PlayerSpawnedInWorld))]
public class PlayerSpawnedPatch
{
    private static void Postfix(GameManager __instance, ClientInfo _cInfo, RespawnType _respawnReason, Vector3i _pos, int _entityId)
    {
        if (GeneralUtility.IsNotRunningOnServer())
        {
            return;
        }

        if (_respawnReason != RespawnType.EnterMultiplayer && _respawnReason != RespawnType.JoinMultiplayer)
        {
            return;
        }

        try
        {
            ProgressionUtility.SendPlayerProgressionToAllPlayers(_entityId);
            ProgressionUtility.SendAllPlayerProgressionsToPlayer(_entityId);
        }
        catch (Exception ex)
        {
            CompareSkillsMod.Instance.Logger.LogLine(ex.StackTrace);
        }
    }
}