using System;

namespace CompareSkills;

public static class SetSkillLevelNotifier
{
    public static event EventHandler<int> EntitySkillLevelUpdated;

    public static void BroadcastUpdate(int entityId)
    {
        EntitySkillLevelUpdated?.Invoke(null, entityId);
    }
}