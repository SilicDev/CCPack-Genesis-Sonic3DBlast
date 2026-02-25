//#define DEBUG

using ConnectorLib;
using CrowdControl.Games.SmartEffects;
using System.Threading.Tasks;

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
#if DEBUG
            return true;
#else
            short level = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Read16(DirectorsCutAddresses.ADDR_CURRENT_LEVEL_INDEX, out level);
            else
                Connector.Read16(Sonic3DBlastAddresses.ADDR_CURRENT_LEVEL_INDEX, out level);
            Log.Message($"Level: {level}");
            return !(level % 3 == 0 || level == 0x16 || level >= 0x13);
#endif
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
            return anim != (short)SonicAnimations.DIEING && anim != (short)SonicAnimations.FROZEN && anim != (short)SonicAnimations.ON_A_RING;
        }

        public override bool StartAction()
        {
            new Task(FreezeFix).Start();
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                bool success = Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_FROZEN_HITS, 4);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC + 0x8E, 1);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SOUND1, (ushort)Sounds.SFX_FREEZE);
                return success & Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.FROZEN);
            }
            else
            {
                bool success = Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_FROZEN_HITS, 4);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC + 0x8E, 1);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SOUND1, (ushort)Sounds.SFX_FREEZE);
                return success & Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.FROZEN);
            }
        }

        private void FreezeFix()
        {
            ushort frozen_hits = 0;
            short level = 0;
            ushort ld_0x0C;
            ushort ld_0x10;
            ushort ld_0x0E;
            ushort ld_0x0A;
            ushort ld_0x12;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                Connector.Read16(DirectorsCutAddresses.ADDR_CURRENT_LEVEL_INDEX, out level);
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_FROZEN_HITS, out frozen_hits);
                Connector.Read16(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x0C, out ld_0x0C);
                Connector.Read16(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x10, out ld_0x10);
                Connector.Read16(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x0E, out ld_0x0E);
                Connector.Read16(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x0A, out ld_0x0A);
                Connector.Read16(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x12, out ld_0x12);
                Connector.Freeze16(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x0C, ld_0x0C);
                Connector.Freeze16(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x10, ld_0x10);
                Connector.Freeze16(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x0E, ld_0x0E);
                Connector.Freeze16(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x0A, ld_0x0A);
                Connector.Freeze16(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x12, ld_0x12);
                while (frozen_hits > 0)
                {
                    Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.FROZEN);
                    Thread.Sleep(50);
                    Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_FROZEN_HITS, out frozen_hits);
                }
                Connector.Unfreeze(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x0C);
                Connector.Unfreeze(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x10);
                Connector.Unfreeze(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x0E);
                Connector.Unfreeze(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x0A);
                Connector.Unfreeze(DirectorsCutAddresses.ADDR_LEVEL_DATA + 0x12);
                Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.IDLE);
            }
            else
            {
                Connector.Read16(Sonic3DBlastAddresses.ADDR_CURRENT_LEVEL_INDEX, out level);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_FROZEN_HITS, out frozen_hits);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x0C, out ld_0x0C);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x10, out ld_0x10);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x0E, out ld_0x0E);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x0A, out ld_0x0A);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x12, out ld_0x12);
                Connector.Freeze16(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x0C, ld_0x0C);
                Connector.Freeze16(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x10, ld_0x10);
                Connector.Freeze16(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x0E, ld_0x0E);
                Connector.Freeze16(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x0A, ld_0x0A);
                Connector.Freeze16(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x12, ld_0x12);
                while (frozen_hits > 0)
                {
                    Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.FROZEN);
                    Thread.Sleep(50);
                    Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_FROZEN_HITS, out frozen_hits);
                }
                Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x0C);
                Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x10);
                Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x0E);
                Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x0A);
                Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_LEVEL_DATA + 0x12);
                Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.IDLE);
            }
        }
    }
}