using ConnectorLib;
using CrowdControl.Games.SmartEffects;
using static CrowdControl.Games.Packs.Sonic3DBlast.Sonic3DBlast;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("SEGA")]
    public class SEGA : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public SEGA(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "SEGA" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "sound" };

        public override SITimeSpan FollowUpDelay => SITimeSpan.FromSeconds(1.5);

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                return Connector.Write16(DirectorsCutAddresses.ADDR_SOUND2, (ushort)Sounds.SFX_SEGA_SCREAM);
            else
                return Connector.Write16(Sonic3DBlastAddresses.ADDR_SOUND2, (ushort)Sounds.SFX_SEGA_SCREAM);
        }

        public override bool StartFollowup()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                bool success = Connector.Read16(DirectorsCutAddresses.ADDR_CURRENT_BGM, out ushort bgm);
                return success & Connector.Write16(DirectorsCutAddresses.ADDR_SOUND2, bgm);
            }
            else
            {
                bool success = Connector.Read8(Sonic3DBlastAddresses.ADDR_CURRENT_BGM, out byte bgm);
                return success & Connector.Write16(Sonic3DBlastAddresses.ADDR_SOUND2, bgm);
            }
        }
    }
}