using ConnectorLib;
using CrowdControl.Games.SmartEffects;
using System.Diagnostics.CodeAnalysis;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("Launch")]
    public class Launch : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public Launch(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "Launch" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "sonic" };

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                bool success = Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.SPRING);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_ON_GROUND, 0);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC + 0x48, 0);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC + 0x4A, 0);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_40, 100);
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_SOUND1, (ushort)Sounds.SFX_SPRING);
                return success & Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Y, 0xFFF6);
            }
            else
            {
                bool success = Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_ANIMATION, (short)SonicAnimations.SPRING);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_ON_GROUND, 0);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC + 0x48, 0);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_40, 100);
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_SOUND1, (ushort)Sounds.SFX_SPRING);
                return success & Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Y, 0xFFF6);
            }
        }
    }
}