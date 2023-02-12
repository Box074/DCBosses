
namespace DCBossesMod;

class QueenBoss : BossBase
{
    public override string PrefabStatueName => "GG_Statue_Grimm";

    public override string BossSceneName => "QueenScene";

    public override void Init()
    {
        _ = Application.platform switch{
            RuntimePlatform.WindowsPlayer => ModRes.AB_QUEEN_WIN,
            RuntimePlatform.LinuxPlayer => ModRes.AB_QUEEN_LINUX,
            RuntimePlatform.OSXPlayer => ModRes.AB_QUEEN_MAC,
            _ => throw new PlatformNotSupportedException()
        };
    }
    public override string Name => "Queen";

    public override Vector2 StatuePos => new(219.4694f, 6.4081f);

    public override ImplMode Impl => ImplMode.Public;

    public override string NameKey => "NAME_DC_QUEEN";

    public override string DescKey => "DESC_DC_QUEEN";

    public override string LockedKey => "LOCK_DC_QUEEN";

    public override void ModifyBossScene(Scene scene, SceneManager sceneManager, BossSceneController ctrl)
    {
        var heroEnter = GameObject.Find("Hero Enter").transform.position;
        ctrl.transform.Find("Dream Entry").position = heroEnter;
        ctrl.transform.Find("door_dreamEnter").position = heroEnter;

        var boss = scene.FindGameObject("QueenBoss");
        boss.transform.SetScaleX(-1);
        var hm = boss.GetComponent<HealthManager>();
        boss.FindChildWithPath("Intro", "CameraLock").layer = (int)GlobalEnums.PhysLayers.DEFAULT;
        ApplyHitEffectsUninfected(hm);
        
        hm.StartCoroutine(BattelControl());
    }

    public override void ModifyStatue(BossStatue statue)
    {

        var statueIcon = statue.gameObject.FindChildWithPath("Base", "Statue", "GG_statues_0006_5")
            .GetComponent<SpriteRenderer>();
        ModRes.ICON_QUEEN.filterMode = FilterMode.Point;
        statueIcon.sprite = ModRes.SPRITE_ICON_QUEEN;
        statueIcon.drawMode = SpriteDrawMode.Sliced;
        statueIcon.size = new Vector2(18, 18);
        statueIcon.transform.localPosition = new Vector3(0.3757f, 2.8963f, 2.699f);
        statueIcon.transform.localScale = new Vector3(0.3f, 0.4f, 1);
    }

    private static IEnumerator BattelControl()
    {
        yield return new WaitForFinishedEnteringScene();
        HeroController.instance.transform.position = GameObject.Find("Hero Enter").transform.position;
        BossSceneController.Instance.bossesDeadWaitTime = 0;
        yield return null;
        while (true)
        {
            yield return null;
            if (HeroController.instance.transform.position.y < 0)
            {
                yield return HeroController.instance.HazardRespawn();
            }
        }
    }
}
