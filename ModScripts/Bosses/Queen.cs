
namespace DCBossesMod;

class QueenBoss : BossBase
{
    public override string PrefabStatueName => "GG_Statue_Grimm";

    public override string BossSceneName => "QueenScene";

    public override void Init()
    {
        _ = ModRes.AB_WIN;
    }
    public override string Name => "Queen";

    public override Vector2 StatuePos => new(200.43f, 36.46f);

    public override bool IsImpl => true;

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
        var sd = statue.bossDetails;
        sd.nameKey = "NAME_DC_QUEEN";
        sd.descriptionKey = "DESC_DC_QUEEN";
        statue.bossDetails = sd;

        var plaqueR = statue.gameObject.FindChildWithPath("Base", "Plaque", "Plaque_Trophy_Right");
        plaqueR?.SetActive(false);
        var plaqueL = statue.gameObject.FindChildWithPath("Base", "Plaque", "Plaque_Trophy_Left");
        plaqueL.transform.position = new Vector3(193.359f, 35.1272f, 1.5323f);
        statue.gameObject.FindChildWithPath("dream_version_switch")?.SetActive(false);

        var statueIcon = statue.gameObject.FindChildWithPath("Base", "Statue", "GG_statues_0006_5")
            .GetComponent<SpriteRenderer>();
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
