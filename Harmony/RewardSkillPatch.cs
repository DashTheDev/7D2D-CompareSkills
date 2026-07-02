using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace CompareSkills;

[HarmonyPatch(typeof(RewardSkill), nameof(RewardSkill.GiveReward))]
public class RewardSkillPatch
{
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> codes = new(instructions);

        CompareSkillsMod.Instance.Logger.LogTranspilerBefore(nameof(RewardSkillPatch), codes);

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
                    // Prepare arg1: player.entityId (player = argument 1, entityId = field)
                    new(ReadableOpCodes.LoadArgument1),
                    new(ReadableOpCodes.LoadField, AccessTools.Field(typeof(Entity), nameof(Entity.entityId))),

                    // Prepare arg2: progressionValue.Name (progressionValue = local variable 0, Name = call virtual method getter)
                    new(ReadableOpCodes.LoadLocalVariable0),
                    new(ReadableOpCodes.CallVirtualMethod, AccessTools.PropertyGetter(typeof(ProgressionValue), nameof(ProgressionValue.Name))),

                    // Prepare arg3: progressionValue.Level (progressionValue = local variable 0, Level = call virtual method getter)
                    new(ReadableOpCodes.LoadLocalVariable0),
                    new(ReadableOpCodes.CallVirtualMethod, AccessTools.PropertyGetter(typeof(ProgressionValue), nameof(ProgressionValue.Level))),

                    // Call method: ProgressionUtility.SendPlayerSkillLevelUpdates(entityId, name, level)
                    new(ReadableOpCodes.CallMethod, replacementMethod)
                };

                // There is an if statement before the code block that will be removed that if false uses a label to jump to the first line of the code
                // that's being removed. So we need to make sure that the label is transferred to the start of the replacement code.
                // So that when the if statement is false it knows to jump to the start of of the new code.
                Label branchLabelToTransfer = codes[blockStart].labels.First();
                replacementInstructions[0].labels.Add(branchLabelToTransfer);

                codes.RemoveRange(blockStart, blockEnd - blockStart);
                codes.InsertRange(blockStart, replacementInstructions);

                patched = true;
                break;
            }

            break;
        }

        CompareSkillsMod.Instance.Logger.LogLine($"{nameof(RewardSkillPatch)} Transpiler patch {(patched ? "was" : "was NOT")} applied!");
        CompareSkillsMod.Instance.Logger.LogTranspilerAfter(nameof(RewardSkillPatch), codes);

        return codes;
    }
}