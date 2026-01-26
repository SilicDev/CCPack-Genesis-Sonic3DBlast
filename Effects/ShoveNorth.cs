using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("ShoveNorth")]
    public class ShoveNorth : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public ShoveNorth(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "ShoveNorth" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "sonic" };

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                return Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Z, 0xFF9C);
            else
                return Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Z, 0xFF9C);
        }
    }
}