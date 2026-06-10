using HarmonyLib;

namespace CompareSkills;

[HarmonyPatch(typeof(NetPackageEntitySetSkillLevelClient), nameof(NetPackageEntitySetSkillLevelClient.ProcessPackage))]
public class NetPackageEntitySetSkillLevelClientPatch
{
    private static void Postfix(NetPackageEntitySetSkillLevelClient __instance, World _world, GameManager _callbacks)
    {
        GeneralUtility.LogLine("Received player set skill level package from server!");
        SetSkillLevelNotifier.BroadcastUpdate(__instance.entityId);
    }
}