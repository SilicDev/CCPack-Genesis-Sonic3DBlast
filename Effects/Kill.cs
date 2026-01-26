using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("Kill")]
    public class Kill : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public Kill(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "Kill" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "live" };

        public override bool StartCondition()
        {
            short anim = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_ANIMATION, out anim);
            else
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_ANIMATION, out anim);
            return anim != (short)SonicAnimations.DIEING;
        }

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                Connector.Read16(DirectorsCutAddresses.ADDR_RINGS, out ushort rings);
                Connector.Write16(DirectorsCutAddresses.ADDR_RINGS, 0);
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_INVINCIBILITY, out ushort invinc);
                Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_INVINCIBILITY, 0);
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_SPEED_SHOES, out ushort shoes);
                Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_SPEED_SHOES, 0);
                bool success = Connector.Write16(DirectorsCutAddresses.ADDR_SHIELD, 0);
                if (!success)
                {
                    Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_INVINCIBILITY, invinc);
                    Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_SPEED_SHOES, shoes);
                    return ResetRings(rings);
                }
                return Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_JUST_HURT, -1);
            }
            else
            {
                Connector.Read16(Sonic3DBlastAddresses.ADDR_RINGS, out ushort rings);
                Connector.Write16(Sonic3DBlastAddresses.ADDR_RINGS, 0);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_INVINCIBILITY, out ushort invinc);
                Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_INVINCIBILITY, 0);
                bool success = Connector.Write16(Sonic3DBlastAddresses.ADDR_SHIELD, 0);
                if (!success)
                {
                    Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_INVINCIBILITY, invinc);
                    return ResetRings(rings);
                }
                return Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_JUST_HURT, -1);
            }
        }

        private bool ResetRings(ushort rings)
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Write16(DirectorsCutAddresses.ADDR_RINGS, rings);
            else
                Connector.Write16(Sonic3DBlastAddresses.ADDR_RINGS, rings);
            return false;
        }
    }
}