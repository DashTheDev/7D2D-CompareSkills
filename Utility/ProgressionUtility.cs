namespace CompareSkills;

public static class ProgressionUtility
{
    public static void SendPlayerProgressionToAllPlayers(int progEntityId)
    {
        if (GeneralUtility.IsNotRunningOnServer())
        {
            return;
        }

        SendPlayerProgression(progEntityId);
    }

    public static void SendAllPlayerProgressionsToPlayer(int targetEntityId)
    {
        if (GeneralUtility.IsNotRunningOnServer())
        {
            return;
        }

        foreach (EntityPlayer player in GameManager.Instance.World.Players.list)
        {
            SendPlayerProgression(player.entityId, targetEntityId);
        }
    }

    private static void SendPlayerProgression(int progEntityId, int? targetEntityId = null)
    {
        if (GeneralUtility.IsNotRunningOnServer() || progEntityId == targetEntityId)
        {
            return;
        }

        EntityPlayer? player = GameManager.Instance.GetPlayer(progEntityId);

        if (player == null || player.Progression == null)
        {
            return;
        }

        SkillLevels skillLevels = SkillLevels.FromProgression(player.Progression);
        NetPackagePlayerProgressionClient package = NetPackageManager.GetPackage<NetPackagePlayerProgressionClient>().Setup(progEntityId, skillLevels);

        if (targetEntityId.HasValue)
        {
            SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, _attachedToEntityId: targetEntityId.Value);
        }
        else
        {
            SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, _allButAttachedToEntityId: progEntityId);
        }

        CompareSkillsMod.Instance.Logger.LogLine($"Sending player progression package to client{(targetEntityId.HasValue ? "" : "s")}!");
    }
    
    public static void SendPlayerSkillLevelUpdates(int entityId, string skillName, int skillLevel)
    {
        CompareSkillsMod.Instance.Logger.LogLine("Sending player set skill level packages to clients!");

        if (GeneralUtility.IsRunningOnServer())
        {
            NetPackageEntitySetSkillLevelClient package = NetPackageManager.GetPackage<NetPackageEntitySetSkillLevelClient>().Setup(entityId, skillName, skillLevel);
            SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, _allButAttachedToEntityId: entityId);
        }
        else
        {
            NetPackageEntitySetSkillLevelServer package = NetPackageManager.GetPackage<NetPackageEntitySetSkillLevelServer>().Setup(entityId, skillName, skillLevel);
            SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(package);
        }
    }

    public static void UpdateSkillLevelsForPlayer(int entityId, SkillLevels skillLevels)
    {
        EntityPlayer? player = GameManager.Instance.World.GetEntity(entityId) as EntityPlayer;

        if (player == null || player.Progression == null)
        {
            return;
        }

        foreach ((string skillName, int skillLevel) in skillLevels)
        {
            ProgressionValue? progValue = player.Progression.GetProgressionValue(skillName);

            if (progValue == null)
            {
                continue;
            }

            progValue.Level = skillLevel;
        }
    }
}