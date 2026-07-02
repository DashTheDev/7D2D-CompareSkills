using UnityEngine;

namespace CompareSkills;

public class GeneralUtility
{
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