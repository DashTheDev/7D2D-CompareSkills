namespace CompareSkills;

public class CompareSkillsConfig
{
    public bool IsEnabled = true;

#if DEBUG
    public bool IsDebug = true;
#else
    public bool IsDebug;
#endif

    public bool DebugTranspilers;
}