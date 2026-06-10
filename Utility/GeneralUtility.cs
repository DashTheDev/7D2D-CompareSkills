using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace CompareSkills;

public class GeneralUtility
{
    public static void LogLine(string str)
    {
        if (!CompareSkillsMod.IsDebug)
        {
            return;
        }

        Log.Out($"[{CompareSkillsMod.ModInstance.Name}](v{CompareSkillsMod.ModInstance.VersionString}) {str}");
    }

    public static void LogTranspilerBefore(string methodName, List<CodeInstruction> instructions)
    {
        LogTranspiler(methodName, true, instructions);
    }

    public static void LogTranspilerAfter(string methodName, List<CodeInstruction> instructions)
    {
        LogTranspiler(methodName, false, instructions);
    }

    private static void LogTranspiler(string methodName, bool isBefore, List<CodeInstruction> instructions)
    {
        if (!CompareSkillsMod.Config.DebugTranspilers)
        {
            return;
        }

        string timingDescription = isBefore ? "BEFORE" : "AFTER";
        LogLine($"=== {methodName} Transpiler - {timingDescription} ===");

        for (int i = 0; i < instructions.Count; i++)
        {
            LogLine($" [{i}] {instructions[i].opcode} {instructions[i].operand}");
        }
    }

    public static bool IsRunningOnServer()
    {
        if (SingletonMonoBehaviour<ConnectionManager>.Instance == null)
        {
            return false;
        }

        return SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
    }

    public static bool IsNotRunningOnServer()
    {
        return !IsRunningOnServer();
    }

    public static bool CheckKeyDown(string value)
    {
        try
        {
            return Input.GetKeyDown(value.ToLower());
        }
        catch
        {
            return false;
        }
    }
}