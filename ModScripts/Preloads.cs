

namespace DCBossesMod;

partial class DeadCellsBosses
{
    [Preload("GG_Hollow_Knight", "Battle Scene/HK Prime")]
    public static GameObject HKPrime;
    [PreloadSharedAssets("nail_clash_tink")]
    public static FsmTemplate fsm_nail_clash_tink;

    [Preload("GG_Grimm", "Boss Scene Controller")]
    public static GameObject _BossSceneControllerPrefab;
    [Preload("GG_Grimm", "_SceneManager")]
    public static GameObject _SceneManagerPrefab;
}
