
namespace DCBossesMod;

class TimeKeeperBoss : BossBase
{
    public override string Name => "TimeKeeper";
    public override string PrefabStatueName => "GG_Statue_Grimm";

    public override string BossSceneName => "UNKNOWN TIMEKEEPER";

    public override string NameKey => "NAME_DC_TIMEKEEPER";

    public override string DescKey => "DESC_DC_TIMEKEEPER";

    public override string LockedKey => "LOCK_DC_TIMEKEEPER";

    public override ImplMode Impl => ImplMode.None;

    public override Vector2 StatuePos => new(215.2987f, 6.4081f);

    public override void Init()
    {
        
    }

    public override void ModifyBossScene(Scene scene, SceneManager sceneManager, BossSceneController ctrl)
    {
        
    }

    public override void ModifyStatue(BossStatue statue)
    {
        
    }
}
