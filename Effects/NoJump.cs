using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("NoJump")]
    public class NoJump : ControlOverrideHandler
    {
        public NoJump(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Durational;

        public override IList<string> Codes { get; } = new[] { "NoJump" };

        public override SITimeSpan RefreshInterval { get; } = SITimeSpan.FromSeconds(0.05);

        public override bool StartAction()
        {
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