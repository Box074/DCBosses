
namespace DCBossesMod;

class OnlyWalkAction : MonoBehaviour
{
    private bool HookTemplate<T>(T orig, HeroController self)  where T : Delegate
    {
        if(this == null || gameObject == null || !enabled || !gameObject.activeInHierarchy) return (bool)orig.DynamicInvoke(self);
        return false;
    }
    private void Awake() {
        On.HeroController.CanJump += HookTemplate;
        On.HeroController.CanSuperDash += HookTemplate;
        On.HeroController.CanNailCharge += HookTemplate;
        On.HeroController.CanNailArt += HookTemplate;
        On.HeroController.CanCast += HookTemplate;
        On.HeroController.CanDash += HookTemplate;
        //gameObject.AddComponent<WalkArea>();
    }
    private void OnDestroy() {
        On.HeroController.CanJump -= HookTemplate;
        On.HeroController.CanSuperDash -= HookTemplate;
        On.HeroController.CanNailCharge -= HookTemplate;
        On.HeroController.CanNailArt -= HookTemplate;
        On.HeroController.CanCast -= HookTemplate;
        On.HeroController.CanDash -= HookTemplate;
    }
}
