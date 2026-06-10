using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace CompareSkills;

public static class HarmonyExtensions
{
    public static bool MatchesMethod(this CodeInstruction codeInstruction, MethodInfo methodToCompareTo, bool opCodeIsVirtual = true)
    {
        OpCode opCodeToCheck = opCodeIsVirtual ? ReadableOpCodes.CallVirtualMethod : ReadableOpCodes.CallMethod;

        if (codeInstruction.opcode != opCodeToCheck)
        {
            return false;
        }

        if (codeInstruction.operand is not MethodInfo method)
        {
            return false;
        }

        return method == methodToCompareTo;
    }

    public static bool DoesNotMatchMethod(this CodeInstruction codeInstruction, MethodInfo methodToCompareTo, bool opCodeIsVirtual = true)
    {
        return !codeInstruction.MatchesMethod(methodToCompareTo, opCodeIsVirtual);
    }
}
