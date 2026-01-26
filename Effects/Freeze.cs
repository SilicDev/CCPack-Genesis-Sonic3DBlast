using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("Freeze")]
    public class Freeze : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public Freeze(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "AddRing" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "sonic" };

        public override bool StartCondition()
        {
            short anim = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_ANIMATION, out anim);
            else
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_ANIMATION, out anim);
            return anim != (short)SonicAnimations.DIEING && anim != (short)SonicAnimations.FROZEN;
        }

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                bool success = Connector.Write16(DirectorsCutAddresses.ADDR_SONIC + 0x88, 4);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC + 0x8E, 1);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SOUND1, (ushort)Sounds.SFX_FREEZE);
                return success & Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.FROZEN);
            }
            else
            {
                bool success = Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC + 0x88, 4);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC + 0x8E, 1);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SOUND1, (ushort)Sounds.SFX_FREEZE);
                return success & Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.FROZEN);
            }
        }
    }
}