using System.Linq;

namespace CompareSkills;

public class NetPackagePlayerProgressionClient : NetPackage
{
    private const int PackageIntSize = 4;
    private const int PackageStringCharSize = 2;

    public int EntityId { get; private set; }
    public SkillLevels SkillLevels { get; private set; }

    public override NetPackageDirection PackageDirection => NetPackageDirection.ToClient;

    public NetPackagePlayerProgressionClient Setup(int entityId, SkillLevels skillLevels)
    {
        EntityId = entityId;
        SkillLevels = skillLevels;
        return this;
    }

    public override void read(PooledBinaryReader _reader)
    {
        EntityId = _reader.ReadInt32();
        SkillLevels = SkillLevels.FromReader(_reader);
    }

    public override void write(PooledBinaryWriter writer)
    {
        base.write(writer);
        writer.Write(EntityId);
        SkillLevels.WriteToWriter(writer);
    }

    public override int GetLength()
    {
        int skillsSize = SkillLevels.Sum(kvp => kvp.Key.Length * PackageStringCharSize + PackageIntSize);
        return PackageIntSize * 2 + skillsSize;
    }

    public override void ProcessPackage(World world, GameManager callbacks)
    {
        if (world == null)
        {
            return;
        }

        EntityAlive entity = world.GetEntity(EntityId) as EntityAlive;

        if (entity == null)
        {
            return;
        }

        GeneralUtility.LogLine("Received player progression package from server!");
        ProgressionUtility.UpdateSkillLevelsForPlayer(EntityId, SkillLevels);
    }
}