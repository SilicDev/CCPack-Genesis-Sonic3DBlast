using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("LoseFlicky")]
    public class LoseFlicky : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public LoseFlicky(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "LoseFlicky" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "flicky" };

        public override bool StartCondition()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                bool success = Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_ANIMATION, out ushort anim);
                if (!success)
                    return false;
                if (anim == (ushort)SonicAnimations.ON_A_RING || anim == (ushort)SonicAnimations.DIEING)
                    return false;
                success &= Connector.Read16(DirectorsCutAddresses.ADDR_FLICKIES_FOLLOWING, out ushort flickies_follow);
                success &= Connector.Read16(DirectorsCutAddresses.ADDR_FLICKIES_SCATTER, out ushort flickies);
                return success & flickies_follow > 0 && flickies == 0;
            }
            else
            {
                bool success = Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_ANIMATION, out ushort anim);
                if (!success)
                    return false;
                if (anim == (ushort)SonicAnimations.ON_A_RING || anim == (ushort)SonicAnimations.DIEING)
                    return false;
                success &= Connector.Read16(Sonic3DBlastAddresses.ADDR_FLICKIES_FOLLOWING, out ushort flickies_follow);
                success &= Connector.Read16(Sonic3DBlastAddresses.ADDR_FLICKIES_SCATTER, out ushort flickies);
                return success & flickies_follow > 0 && flickies == 0;
            }
        }

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                Connector.Write16(DirectorsCutAddresses.ADDR_SOUND1, (ushort)Sounds.SFX_FLICKY_CHIRPS);
                return Connector.Write16(DirectorsCutAddresses.ADDR_FLICKIES_SCATTER, 1);
            }
            else
            {
                Connector.Write16(Sonic3DBlastAddresses.ADDR_SOUND1, (ushort)Sounds.SFX_FLICKY_CHIRPS);
                return Connector.Write16(Sonic3DBlastAddresses.ADDR_FLICKIES_SCATTER, 1);
            }
        }
    }
}