using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace CompareSkills;

[HarmonyPatch(typeof(XUiC_SkillAttributeLevel), nameof(XUiC_SkillAttributeLevel.btnBuy_OnPress))]
public class SkillAttributeLevelBuyPatch
{
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> codes = new(instructions);

        CompareSkillsMod.Instance.Logger.LogTranspilerBefore(nameof(SkillAttributeLevelBuyPatch), codes);

        MethodInfo connectionManagerIsServerGetter = AccessTools.PropertyGetter(typeof(ConnectionManager), nameof(ConnectionManager.IsServer));
        MethodInfo connectionManagerSendToServerMethod = AccessTools.Method(typeof(ConnectionManager), nameof(ConnectionManager.SendToServer));
        MethodInfo replacementMethod = AccessTools.Method(typeof(ProgressionUtility), nameof(ProgressionUtility.SendPlayerSkillLevelUpdates));

        bool patched = false;

        for (int i = 0; i < codes.Count; i++)
        {
            if (codes[i].DoesNotMatchMethod(connectionManagerIsServerGetter))
            {
                continue;
            }

            int blockStart = i - 1;

            for (int j = i + 1; j < codes.Count; j++)
            {
                if (codes[j].DoesNotMatchMethod(connectionManagerSendToServerMethod))
                {
                    continue;
                }

                int blockEnd = j + 1;

                List<CodeInstruction> replacementInstructions = new()
                {
                    // Prepare arg1: entityPlayer.entityId (entityPlayer = local variable 0, entityId = field)
                    new(ReadableOpCodes.LoadLocalVariable0),
                    new(ReadableOpCodes.LoadField, AccessTools.Field(typeof(Entity), nameof(Entity.entityId))),

                    // Prepare arg2: this.CurrentSkill.Name (this = argument 0, CurrentSkill = call method getter, Name = call virtual method getter)
                    new(ReadableOpCodes.LoadArgument0),
                    new(ReadableOpCodes.CallMethod, AccessTools.PropertyGetter(typeof(XUiC_SkillAttributeLevel), nameof(XUiC_SkillAttributeLevel.CurrentSkill))),
                    new(ReadableOpCodes.CallVirtualMethod, AccessTools.PropertyGetter(typeof(ProgressionValue), nameof(ProgressionValue.Name))),

                    // Prepare arg3: this.CurrentSkill.Level (this = argument 0, CurrentSkill = call method getter, Level = call virtual method getter)
                    new(ReadableOpCodes.LoadArgument0),
                    new(ReadableOpCodes.CallMethod, AccessTools.PropertyGetter(typeof(XUiC_SkillAttributeLevel), nameof(XUiC_SkillAttributeLevel.CurrentSkill))),
                    new(ReadableOpCodes.CallVirtualMethod, AccessTools.PropertyGetter(typeof(ProgressionValue), nameof(ProgressionValue.Level))),

                    // Call method: ProgressionUtility.SendPlayerSkillLevelUpdates(entityId, name, level)
                    new(ReadableOpCodes.CallMethod, replacementMethod),
                };

                codes.RemoveRange(blockStart, blockEnd - blockStart);
                codes.InsertRange(blockStart, replacementInstructions);

                patched = true;
                break;
            }

            break;
        }

        CompareSkillsMod.Instance.Logger.LogLine($"{nameof(SkillAttributeLevelBuyPatch)} Transpiler patch {(patched ? "was" : "was NOT")} applied!");
        CompareSkillsMod.Instance.Logger.LogTranspilerAfter(nameof(SkillAttributeLevelBuyPatch), codes);

        return codes;
    }
}