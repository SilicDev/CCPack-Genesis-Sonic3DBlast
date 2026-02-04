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

        public override IList<string> Codes { get; } = new[] { "Freeze" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "sonic" };

        public override bool RetryOnFail => IsValidLevel();

        private bool IsValidLevel()
        {
            short level = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Read16(DirectorsCutAddresses.ADDR_CURRENT_LEVEL_INDEX, out level);
            else
                Connector.Read16(Sonic3DBlastAddresses.ADDR_CURRENT_LEVEL_INDEX, out level);
            Log.Message($"Level: {level}");
            return !(level % 3 == 0 || level == 0x16 || level == 4 || level >= 0x13);
        }

        public override bool StartCondition()
        {
            if (!IsValidLevel())
            {
                EffectPack.Respond(Request, EffectStatus.FailTemporary, "Freeze breaks in this level");
                return false;
            }
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