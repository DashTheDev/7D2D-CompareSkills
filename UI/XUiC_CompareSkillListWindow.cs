namespace CompareSkills;

public class XUiC_CompareSkillListWindow : XUiC_SkillListWindow
{
    private XUiC_PlayerSelector? playerSelector;

    public EntityPlayer? SelectedPlayer { get; private set; }

    public override void Init()
    {
        base.Init();
        playerSelector = GetChildByType<XUiC_PlayerSelector>();
        playerSelector?.SelectedPlayerChanged += OnSelectedPlayerChanged;
    }

    public override void OnOpen()
    {
        base.OnOpen();
        SetSkillLevelNotifier.EntitySkillLevelUpdated += OnEntitySkillLevelUpdated;
    }

    public override void OnClose()
    {
        base.OnClose();
        SetSkillLevelNotifier.EntitySkillLevelUpdated -= OnEntitySkillLevelUpdated;
    }

    public override void Update(float _dt)
    {
        base.Update(_dt);

        if (CompareSkillsMod.Instance.Config.IsDebug && GeneralUtility.CheckKeyDown("`"))
        {
            playerSelector?.ToggleDebugMode();
        }
    }

    private void OnSelectedPlayerChanged(object sender, System.EventArgs e)
    {
        int? playerEntityId = playerSelector?.SelectedPlayer?.EntityID;

        if (playerEntityId == null)
        {
            // TODO: Hide stuff
            SelectedPlayer = null;
            RefreshSkillList();
            return;
        }

        EntityPlayer? player = GameManager.Instance.World.GetEntity(playerSelector.SelectedPlayer.Value.EntityID) as EntityPlayer;

        if (player == null)
        {
            SelectedPlayer = null;
            RefreshSkillList();
            return;
        }

        SelectedPlayer = player;
        RefreshSkillList();
    }

    private void OnEntitySkillLevelUpdated(object sender, int entityId)
    {
        if (SelectedPlayer == null || entityId != SelectedPlayer.entityId)
        {
            return;
        }

        RefreshSkillList();
    }

    private void RefreshSkillList()
    {
        if (skillList.pagingControl == null)
        {
            skillList.RefreshSkillList();
            return;
        }

        int oldPageNumber = skillList.pagingControl.CurrentPageNumber;
        skillList.RefreshSkillList();
        skillList.pagingControl.CurrentPageNumber = oldPageNumber;
        skillList.PagingControl_OnPageChanged();
    }
}