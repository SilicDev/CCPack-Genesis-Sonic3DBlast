using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("RedShield")]
    public class RedShield : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public RedShield(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "RedShield" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "shield" };

        public override bool StartCondition()
        {
            ushort shield = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Read16(DirectorsCutAddresses.ADDR_SHIELD, out shield);
            else
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SHIELD, out shield);
            return (shield & 0xDFFF) != ((ushort)Shields.RED);
        }

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                return Connector.Write16(DirectorsCutAddresses.ADDR_MONITOR_EFFECT, (ushort)MonitorEffects.RED_SHIELD);
            else
                return Connector.Write16(Sonic3DBlastAddresses.ADDR_MONITOR_EFFECT, (ushort)MonitorEffects.RED_SHIELD);
        }
    }
}