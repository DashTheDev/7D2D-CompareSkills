namespace CompareSkills;

public static class GameManagerExtensions
{
    public static EntityPlayer? GetPlayer(this GameManager instance, int entityId)
    {
        return instance.World.GetEntity(entityId) as EntityPlayer;
    }
}