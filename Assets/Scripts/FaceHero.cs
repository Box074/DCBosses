
using HutongGames.PlayMaker;
using UnityEngine;

public class FaceHero : FsmStateAction
{
    public override void OnEnter()
    {
        DoFaceHero();
        if(!everyFrame)
        {
            Finish();
        }
    }
    public override void OnUpdate()
    {
        if(everyFrame)
        {
            DoFaceHero();
        }
    }
    private void DoFaceHero()
    {
        var self = this.self.OwnerOption == OwnerDefaultOption.UseOwner ? this.Fsm.GameObject : this.self.GameObject.Value;
        var heroX = hero.Value.transform.position.x;
        var selfX = self.transform.position.x;
        var scaleX = Mathf.Abs(self.transform.localScale.x);
        if(selfX > heroX) scaleX *= -1;
        if(!spriteFacingRight) scaleX *= -1;
        var s = self.transform.localScale;
        s.x = scaleX;
        self.transform.localScale = s;
    }

    public override void Reset()
    {
        everyFrame = false;
        hero = new FsmGameObject() {
            UseVariable = false
        };
        self = new FsmOwnerDefault() {
            OwnerOption = OwnerDefaultOption.UseOwner
        };
    }
    [UIHint(UIHint.Variable)]
    public FsmGameObject hero;
    public FsmOwnerDefault self;
    public bool spriteFacingRight;
    public bool everyFrame = false;
}
