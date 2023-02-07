
namespace DCBossesMod;

abstract class BossBase
{
    public abstract void ModifyStatue(BossStatue statue);
    public abstract string Name { get; }
    public abstract string PrefabStatueName { get; }
    public abstract string BossSceneName { get; }
    public abstract bool IsImpl { get; }
    public abstract void Init();
    public abstract void ModifyBossScene(Scene scene, SceneManager sceneManager, BossSceneController ctrl);
    public abstract Vector2 StatuePos { get; }
    public static List<BossBase> bosses = new() {
        new QueenBoss()
    };

    public static void ApplyHitEffectsUninfected(HealthManager hm)
    {
        var he = hm.gameObject.AddComponent<EnemyHitEffectsUninfected>();
        he.slashEffectGhost1 = DeadCellsBosses.hitEffect.slashEffectGhost1;
        he.slashEffectGhost2 = DeadCellsBosses.hitEffect.slashEffectGhost2;
        he.uninfectedHitPt = DeadCellsBosses.hitEffect.uninfectedHitPt;

        GameObject audios = new GameObject();
        audios.transform.parent = hm.transform;
        he.audioPlayerPrefab = audios.AddComponent<AudioSource>();

        ReflectionHelper.SetField<HealthManager, IHitEffectReciever>(hm, "hitEffectReceiver", he);
    }
}
