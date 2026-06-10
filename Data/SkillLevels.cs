using System;
using System.Collections.Generic;

namespace CompareSkills;

public class SkillLevels : Dictionary<string, int>
{
    private SkillLevels() { }
    private SkillLevels(int count) : base(count) { }

    public void WriteToWriter(PooledBinaryWriter writer)
    {
        writer.Write(Count);
        foreach (var kvp in this)
        {
            writer.Write(kvp.Key);
            writer.Write(kvp.Value);
        }
    }

    public void Add(ProgressionValue progValue)
    {
        if (progValue == null)
        {
            return;
        }

        if (TryGetValue(progValue.name, out int foundLevel))
        {
            this[progValue.name] = Math.Max(foundLevel, progValue.level);
        }
        else
        {
            this.Add(progValue.name, progValue.level);
        }
    }

    public static SkillLevels Empty()
    {
        return [];
    }

    public static SkillLevels FromReader(PooledBinaryReader _reader)
    {
        int count = _reader.ReadInt32();
        SkillLevels dict = new SkillLevels(count);

        for (int i = 0; i < count; i++)
        {
            string skill = _reader.ReadString();
            int level = _reader.ReadInt32();
            dict[skill] = level;
        }

        return dict;
    }

    public static SkillLevels FromProgression(Progression progression)
    {
        SkillLevels dict = [];

        if (progression?.ProgressionValueQuickList == null || progression.ProgressionValueQuickList.Count <= 0)
        {
            return dict;
        }

        for (int i = 0; i < progression.ProgressionValueQuickList.Count; i++)
        {
            ProgressionValue progValue = progression.ProgressionValueQuickList[i];
            dict.Add(progValue);
        }

        return dict;
    }

    public static SkillLevels FromProgressionValue(ProgressionValue progValue)
    {
        SkillLevels dict = [];
        dict.Add(progValue);
        return dict;
    }
}
