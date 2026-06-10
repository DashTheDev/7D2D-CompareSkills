namespace CompareSkills;

public readonly struct DisplayPlayer
{
    public DisplayPlayer(int entityID, string name)
    {
        EntityID = entityID;
        Name = name;
    }

    public int EntityID { get; }
    public string Name { get; }
}
