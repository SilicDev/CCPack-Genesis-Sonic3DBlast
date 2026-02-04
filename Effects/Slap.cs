using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("Slap")]
    public class Slap : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public Slap(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "Slap" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "sonic" };

        public override bool StartCondition()
        {
            ushort anim = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_ANIMATION, out anim);
            else
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_ANIMATION, out anim);
            return anim != (short)SonicAnimations.DIEING && anim != (short)SonicAnimations.ON_A_RING;
        }

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                bool success = Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.GETTING_HURT);
                success &= Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_ANGLE, out ushort value);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_ANGLE, (ushort)(value & 0xFF00));
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_ANGLE_TARGET, (ushort)(value & 0xFF00));
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Y, 0xFFFB);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_40, 100);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_48, 0);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC + 0x4A, 0);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_ON_GROUND, 0);
                return Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_JUST_HURT, -2);
            }
            else
            {
                bool success = Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.GETTING_HURT);
                success &= Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_ANGLE, out ushort value);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_ANGLE, (ushort)(value & 0xFF00));
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_ANGLE_TARGET, (ushort)(value & 0xFF00));
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Y, 0xFFFB);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_40, 100);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_48, 0);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_ON_GROUND, 0);
                return Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_JUST_HURT, -2);
            }
        }
    }
}