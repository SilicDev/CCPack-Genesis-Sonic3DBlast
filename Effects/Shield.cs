using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("Shield")]
    public class Shield : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public Shield(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "Shield" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "shield" };

        public override bool StartCondition()
        {
            ushort shield = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Read16(DirectorsCutAddresses.ADDR_SHIELD, out shield);
            else
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SHIELD, out shield);
            return (shield & 0xDFFF) != ((ushort)Shields.DEFAULT);
        }

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                return Connector.Write16(DirectorsCutAddresses.ADDR_MONITOR_EFFECT, (ushort)MonitorEffects.SHIELD);
            else
                return Connector.Write16(Sonic3DBlastAddresses.ADDR_MONITOR_EFFECT, (ushort)MonitorEffects.SHIELD);
        }
    }
}