using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("ShoveSouth")]
    public class ShoveSouth : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public ShoveSouth(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "ShoveSouth" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "sonic" };

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                return Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Z, 0x0064);
            else
                return Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Z, 0x0064);
        }
    }
}