using HarmonyLib;

namespace CompareSkills;

[HarmonyPatch(typeof(NetPackagePlayerStats), nameof(NetPackagePlayerStats.ProcessPackage))]
public class NetPackagePlayerStatsPatch
{
    private static void Postfix(NetPackagePlayerStats __instance, World _world, GameManager _callbacks)
    {
        if (__instance?.entityNetworkStats?.hasProgression != true)
        {
            return;
        }

        SetSkillLevelNotifier.BroadcastUpdate(__instance.entityId);
    }
}