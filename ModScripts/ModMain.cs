
using Language;

namespace DCBossesMod;

partial class DeadCellsBosses : ModBase, ILocalSettings<Settings>
{
    static DeadCellsBosses()
    {
        _ = System.Reflection.Assembly.Load(ModRes.SCRIPT_ASSEMBLY_BYTE);
    }
    public Settings settings = new();
    void ILocalSettings<Settings>.OnLoadLocal(Settings s) => settings = s;
    Settings ILocalSettings<Settings>.OnSaveLocal() => settings;
    public static readonly MusicCue emptyCue = new MusicCue();
    public static EnemyHitEffectsUninfected hitEffect;
    protected override List<(SupportedLanguages, string)> LanguagesEx => new()
    {
        (SupportedLanguages.ZH, "lang.zh"),
        (SupportedLanguages.EN, "lang.en")
    };
    public DeadCellsBosses()
    {

    }
    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        I18n.UseLanguageHook = true;

        NailParryFsm.onLoad += (NailParryFsm self) =>
        {
            var fsm = self.gameObject.AddComponent<PlayMakerFSM>();
            fsm.SetFsmTemplate(fsm_nail_clash_tink);
        };

        ReflectionHelper.SetField(emptyCue, "channelInfos", Enumerable.Range(0, 8).Select(x => new MusicCue.MusicChannelInfo()).ToArray());
        UnityEngine.Object.Destroy(_SceneManagerPrefab.LocateMyFSM("FSM"));

        hitEffect = HKPrime.GetComponent<EnemyHitEffectsUninfected>();

        foreach (var v in BossBase.bosses)
        {
            v.Init();
        }

        ModHooks.GetPlayerVariableHook += (type, name, orig) =>
        {
            if (type != typeof(BossStatue.Completion)) return orig;
            foreach (var v in BossBase.bosses)
            {
                var pname = $"GG_Statue_DC_{v.Name}";
                if (name != pname) continue;
                var result = settings.status.TryGetOrAddValue(v.Name, () =>
                {
                    return new()
                    {
                        usingAltVersion = false,
                        isUnlocked = v.Impl == BossBase.ImplMode.Public,
                        hasBeenSeen = false
                    };
                });
                result.isUnlocked = v.Impl == BossBase.ImplMode.Public;
                if (DebugManager.IsDebug(this) && v.Impl == BossBase.ImplMode.Debug)
                {
                    result.isUnlocked = true;
                }
                if (v.Impl != BossBase.ImplMode.Public)
                {
                    result.hasBeenSeen = false;
                }
                return result;
            }
            return orig;
        };
        ModHooks.SetPlayerVariableHook += (type, name, orig) =>
        {
            if (type != typeof(BossStatue.Completion) || name != "statueStateDCQueen") return orig;
            foreach (var v in BossBase.bosses)
            {
                var pname = $"GG_Statue_DC_{v.Name}";
                if (name != pname) continue;
                settings.status[v.Name] = (BossStatue.Completion)orig;
            }
            return orig;
        };
        ModHooks.GetPlayerBoolHook += (name, orig) =>
        {
            if (!name.StartsWith("isUnlockDCBoss")) return orig;
            foreach (var v in BossBase.bosses)
            {
                var pname = $"isUnlockDCBoss{v.Name}";
                if (name != pname) continue;
                if (v.Impl == BossBase.ImplMode.Public) return true;
                if (v.Impl == BossBase.ImplMode.Debug && DebugManager.IsDebug(this)) return true;
                return false;
            }
            return orig;
        };

        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += (currnet, next) =>
        {
            if (next.name == "GG_Workshop")
            {
                ModifyStatue(next);
                return;
            }
            foreach (var v in BossBase.bosses)
            {
                if (v.BossSceneName != next.name) continue;

                GameManager.instance.AudioManager.ApplyMusicCue(emptyCue, 0, 0, false);

                var smg = GameObject.Instantiate(_SceneManagerPrefab);
                smg.name = "_SceneManager";
                var sm = smg.GetComponent<SceneManager>();
                var bcg = GameObject.Instantiate(_BossSceneControllerPrefab);
                bcg.name = "Boss Scene Controller";
                var bc = bcg.GetComponent<BossSceneController>();
                v.ModifyBossScene(next, sm, bc);

                smg.SetActive(true);
                bcg.SetActive(true);
            }
        };
    }
    private void ModifyStatue(Scene gg)
    {
        foreach (var v in BossBase.bosses)
        {
            var prefab = GameObject.Find(v.PrefabStatueName);
            if (prefab == null)
            {
                LogError("Not found Prefab Statue: " + v.PrefabStatueName);
                continue;
            }
            var pos = (Vector3)v.StatuePos;
            pos.z = prefab.transform.position.z;
            var sg = GameObject.Instantiate(prefab, pos, Quaternion.identity, null);
            sg.name = $"GG_Statue_DC_{v.Name}";
            var s = sg.GetComponent<BossStatue>();
            s.statueStatePD = "statueStateDC" + v.Name;
            s.dreamBossScene = null;
            s.dreamStatueStatePD = "";

            var plaqueR = sg.FindChildWithPath("Base", "Plaque", "Plaque_Trophy_Right");
            plaqueR?.SetActive(false);
            var plaqueL = sg.FindChildWithPath("Base", "Plaque", "Plaque_Trophy_Left");
            plaqueL?.SetActive(false);
            sg.FindChildWithPath("dream_version_switch")?.SetActive(false);

            var sd = s.bossDetails;
            sd.nameKey = v.NameKey;
            sd.descriptionKey = v.DescKey;
            s.bossDetails = sd;

            var locked = sg.FindChild("Inspect_Locked").LocateMyFSM("inspect_region");
            locked.FsmVariables.FindFsmString("Game Text Convo").Value = v.LockedKey;

            s.bossScene = new()
            {
                sceneName = v.BossSceneName,
                bossTests = new BossScene.BossTest[] {
                    new()
                    {
                        boolTests = new BossScene.BossTest.BoolTest[] {
                            new() {
                                playerDataBool = "isUnlockDCBoss" + v.Name,
                                value = true
                            }
                        },
                        intTests = new BossScene.BossTest.IntTest[0],
                        persistentBool = new() {
                            id = "",
                            sceneName = ""
                        }
                    }
                }
            };

            v.ModifyStatue(s);
        }
    }

    private int count = 0;
}
