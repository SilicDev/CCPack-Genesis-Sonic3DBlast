using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("Invincibility")]
    public class Invincibility : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public Invincibility(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "Invincibility" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "invincibility" };

        public override bool StartCondition()
        {
            ushort invinc = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_INVINCIBILITY, out invinc);
            else
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_INVINCIBILITY, out invinc);
            return invinc == 0;
        }

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                return Connector.Write16(DirectorsCutAddresses.ADDR_MONITOR_EFFECT, (ushort)MonitorEffects.INCINCIBILITY);
            else
                return Connector.Write16(Sonic3DBlastAddresses.ADDR_MONITOR_EFFECT, (ushort)MonitorEffects.INCINCIBILITY);
        }
    }
}