using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("ShoveSouth")]
    public class ShoveSouth : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public ShoveSouth(Sonic3DBlast pack) : base(pack) { } // bottom left

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "ShoveSouth" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "sonic" };

        public override bool StartCondition()
        {
            if (EffectPack.IsSpecialStage())
            {
                EffectPack.Respond(Request, EffectStatus.FailTemporary, "Unavailable in special stages!");
                return false;
            }
            if (EffectPack.IsOoB())
            {
                return false;
            }
            return base.StartCondition();
        }

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                if (EffectPack.IsSpecialStage())
                    return Connector.Write16(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_VELOCITY_Y, 0x0064);
                else
                    return Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Y, 0x0064);
            }
            else
            {
                if (EffectPack.IsSpecialStage())
                    return Connector.Write16(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_VELOCITY_Y, 0x0064);
                else
                    return Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Y, 0x0064);
            }
        }
    }
}