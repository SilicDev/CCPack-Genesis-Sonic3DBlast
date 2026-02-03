using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("InvertControls")]
    public class InvertControls : ControlOverrideHandler
    {
        public InvertControls(Sonic3DBlast pack) : base(pack) { }

        public override IList<string> Codes { get; } = new[] { "InvertControls" };

        public override bool StartAction()
        {
            EffectPack.controlOverrides |= ControlOverrides.INVERT_DPAD;
            return true;
        }

        public override bool StopAction()
        {
            EffectPack.controlOverrides &= ~ControlOverrides.INVERT_DPAD;
            return true;
        }
    }
}