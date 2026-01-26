using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("TakeRing")]
    public class TakeRing : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public TakeRing(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "TakeRing" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "rings" };

        public override bool StartCondition()
        {
            ushort rings = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Read16(DirectorsCutAddresses.ADDR_RINGS, out rings);
            else
                Connector.Read16(Sonic3DBlastAddresses.ADDR_RINGS, out rings);
            return rings > 0;
        }

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                Connector.Read16(DirectorsCutAddresses.ADDR_RINGS, out ushort rings);
                rings -= 1;
                if (rings == ushort.MaxValue)
                    rings = 0;

                Connector.Write16(DirectorsCutAddresses.ADDR_RINGS, rings);
                ushort hud = (ushort)((((rings / 100) % 10) << 8) + (((rings / 10) % 10) << 4) + rings % 10);
                return Connector.Write16(DirectorsCutAddresses.ADDR_RINGS_HUD, hud);
            }
            else
            {
                Connector.Read16(Sonic3DBlastAddresses.ADDR_RINGS, out ushort rings);
                rings -= 1;
                if (rings == ushort.MaxValue)
                    rings = 0;

                Connector.Write16(Sonic3DBlastAddresses.ADDR_RINGS, rings);
                return Connector.Write16(Sonic3DBlastAddresses.ADDR_RINGS_HUD, rings);
            }
        }
    }
}