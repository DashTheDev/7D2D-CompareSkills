using HarmonyLib;

namespace CompareSkills;

[HarmonyPatch(typeof(NetPackageEntitySetSkillLevelServer), nameof(NetPackageEntitySetSkillLevelServer.ProcessPackage))]
public class NetPackageEntitySetSkillLevelServerPatch
{
    private static void Postfix(NetPackageEntitySetSkillLevelServer __instance, World _world, GameManager _callbacks)
    {
        GeneralUtility.LogLine("Received player set skill level package from client!");
        ProgressionUtility.SendPlayerSkillLevelUpdates(__instance.entityId, __instance.skill, __instance.level);
        SetSkillLevelNotifier.BroadcastUpdate(__instance.entityId);
    }
}