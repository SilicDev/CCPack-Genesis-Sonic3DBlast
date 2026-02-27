using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("AddRing")]
    public class AddRings : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public AddRings(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "AddRing" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "rings" };

        public override bool StartCondition()
        {
            if (EffectPack.IsSpecialStage())
            {
                uint rings = 0;
                if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                    Connector.Read32(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_RINGS, out rings);
                else
                    Connector.Read32(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_RINGS, out rings);
                uint actual_rings = (byte)(rings & 0xFF);
                actual_rings += (((rings >> 8) & 0xFF) * 10);
                actual_rings += (((rings >> 16) & 0xFF) * 100);
                return actual_rings < 999;
            }
            else
            {
                ushort rings = 0;
                if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                    Connector.Read16(DirectorsCutAddresses.ADDR_RINGS, out rings);
                else
                    Connector.Read16(Sonic3DBlastAddresses.ADDR_RINGS, out rings);

                return rings < 999;
            }
        }

        public override bool StartAction()
        {
            if (EffectPack.IsSpecialStage())
            {
                uint rings = 0;
                if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                    Connector.Read32(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_RINGS, out rings);
                else
                    Connector.Read32(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_RINGS, out rings);
                uint actual_rings = (byte)(rings & 0xFF);
                actual_rings += (uint)((byte)(rings >> 8) * 10);
                actual_rings += (uint)((byte)(rings >> 16) * 100);
                actual_rings += 1;
                rings = (uint)(actual_rings % 10 + (((actual_rings / 10) % 10) << 8) + (((actual_rings / 100) % 10) << 16));

                if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                {
                    Connector.Write32(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_RINGS, rings);
                    return Connector.Write16(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_RING_UPDATE_FLAG, 1);
                }
                else
                {
                    Connector.Write32(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_RINGS, rings);
                    return Connector.Write16(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_RING_UPDATE_FLAG, 1);
                }
            }
            else
            {
                if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                {
                    Connector.Read16(DirectorsCutAddresses.ADDR_RINGS, out ushort rings);
                    rings += 1;
                    if (rings > 999)
                        rings = 999;
                    Connector.Write16(DirectorsCutAddresses.ADDR_RINGS, rings);
                    ushort hud = (ushort)((((rings / 100) % 10) << 8) + (((rings / 10) % 10) << 4) + rings % 10);
                    return Connector.Write16(DirectorsCutAddresses.ADDR_RINGS_HUD, hud);
                }
                else
                {
                    Connector.Read16(Sonic3DBlastAddresses.ADDR_RINGS, out ushort rings);
                    rings += 1;
                    if (rings > 999)
                        rings = 999;
                    Connector.Write16(Sonic3DBlastAddresses.ADDR_RINGS, rings);
                    return Connector.Write16(Sonic3DBlastAddresses.ADDR_RINGS_HUD, rings);
                }
            }
        }
    }
}