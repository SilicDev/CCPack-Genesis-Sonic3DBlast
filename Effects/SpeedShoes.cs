using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("SpeedShoes")]
    public class SpeedShoes : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public SpeedShoes(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "SpeedShoes" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "speed_shoes" };

        public override bool StartCondition()
        {
            ushort shoes = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_SPEED_SHOES, out shoes);
            else
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_SPEED_SHOES, out shoes);
            return shoes == 0;
        }

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                return Connector.Write16(DirectorsCutAddresses.ADDR_MONITOR_EFFECT, (ushort)MonitorEffects.SPEED_SHOES);
            else
                return Connector.Write16(Sonic3DBlastAddresses.ADDR_MONITOR_EFFECT, (ushort)MonitorEffects.SPEED_SHOES);
        }
    }
}