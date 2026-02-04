using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("NoSpin")]
    public class NoSpin : ControlOverrideHandler
    {
        public NoSpin(Sonic3DBlast pack) : base(pack) { }

        public override IList<string> Codes { get; } = new[] { "NoSpin" };

        public override bool StartAction()
        {
            if ((EffectPack.controlOverrides & ControlOverrides.NO_SPIN) != ControlOverrides.NONE)
                return false;
            EffectPack.controlOverrides |= ControlOverrides.NO_SPIN;
            return true;
        }

        public override bool StopAction()
        {
            EffectPack.controlOverrides &= ~ControlOverrides.NO_SPIN;
            return true;
        }
    }
}