using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("NoJump")]
    public class NoJump : ControlOverrideHandler
    {
        public NoJump(Sonic3DBlast pack) : base(pack) { }

        public override IList<string> Codes { get; } = new[] { "NoJump" };

        public override bool StartAction()
        {
            if ((EffectPack.controlOverrides & ControlOverrides.NO_JUMP) != ControlOverrides.NONE)
                return false;
            EffectPack.controlOverrides |= ControlOverrides.NO_JUMP;
            return true;
        }

        public override bool StopAction()
        {
            EffectPack.controlOverrides &= ~ControlOverrides.NO_JUMP;
            return true;
        }
    }
}