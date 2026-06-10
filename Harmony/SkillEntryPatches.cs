using HarmonyLib;
using UnityEngine;

namespace CompareSkills;

[HarmonyPatch(typeof(XUiC_SkillEntry))]
public class SkillEntryPatches
{
    [HarmonyPatch(nameof(XUiC_SkillEntry.GetBindingValueInternal))]
    private static bool Prefix(XUiC_SkillEntry __instance, ref string value, string bindingName, ref bool __result)
    {
        switch (bindingName)
        {
            case "comparedgrouplevel":
                value = GetComparedGroupLevel(__instance);
                __result = true;
                return false;

            case "canpurchase":
                if (__instance.DisplayType != ProgressionClass.DisplayTypes.Standard)
                {
                    value = "false";
                    __result = true;
                    return false;
                }

                return true;

            default:
                return true;
        }
    }

    [HarmonyPatch(nameof(XUiC_SkillEntry.GetGroupLevel))]
    private static bool Prefix(XUiC_SkillEntry __instance, ref string __result)
    {
        string groupLevel = GetGroupLevelForPlayerSkill(__instance, __instance.xui.playerUI.entityPlayer, __instance.currentSkill);
        __result = groupLevel;
        return string.IsNullOrEmpty(groupLevel);
    }

    private static string GetGroupLevelForPlayerSkill(XUiC_SkillEntry __instance, EntityPlayer player, ProgressionValue skill)
    {
        if (skill == null)
        {
            return string.Empty;
        }

        if (skill.ProgressionClass.IsBookGroup)
        {
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < skill.ProgressionClass.Children.Count; i++)
            {
                num++;
                if (player.Progression.GetProgressionValue(skill.ProgressionClass.Children[i].Name).Level == 1)
                {
                    num2++;
                }
            }

            num2 = Mathf.Min(num2, num - 1);
            return __instance.groupLevelFormatter.Format(num2, num2, num - 1);
        }
        else if (skill.ProgressionClass.Type != ProgressionType.None)
        {
            return __instance.groupLevelFormatter.Format(skill.Level, skill.CalculatedLevel(player), skill.ProgressionClass.MaxLevel);
        }

        return string.Empty;
    }

    private static string GetComparedGroupLevel(XUiC_SkillEntry __instance)
    {
        if (__instance.currentSkill == null || __instance.skillList == null || __instance.skillList.SkillListWindow == null)
        {
            return string.Empty;
        }

        XUiC_CompareSkillListWindow? compareSkillListWindow = __instance.skillList.SkillListWindow as XUiC_CompareSkillListWindow;
        EntityPlayer? playerToCompare = compareSkillListWindow?.SelectedPlayer;
        DictionaryNameId<ProgressionValue>? playerToCompareProgValues = playerToCompare?.Progression?.ProgressionValues;

        if (playerToCompareProgValues == null || playerToCompareProgValues.Count <= 0)
        {
            return string.Empty;
        }

        if (!playerToCompareProgValues.Contains(__instance.currentSkill.Name))
        {
            return string.Empty;
        }

        ProgressionValue? comparedValue = playerToCompareProgValues.Get(__instance.currentSkill.name);

        if (comparedValue == null)
        {
            return string.Empty;
        }

        return GetGroupLevelForPlayerSkill(__instance, playerToCompare, comparedValue);
    }
}