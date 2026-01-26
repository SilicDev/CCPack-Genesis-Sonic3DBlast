using ConnectorLib;
using CrowdControl.Games.SmartEffects;
using System.Threading.Tasks;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("Stop")]
    public class Stop : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public Stop(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "Stop" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "sonic" };

        public override SITimeSpan FollowUpDelay => SITimeSpan.FromSeconds(0.5);

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                bool success = Connector.Freeze16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_X, 0);
                success &= Connector.Freeze16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Y, 0);
                success &= Connector.Freeze16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Z, 0);
                return success;
            }
            else
            {
                bool success = Connector.Freeze16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_X, 0);
                success &= Connector.Freeze16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Y, 0);
                success &= Connector.Freeze16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Z, 0);
                return success;
            }
        }

        public override bool StartFollowup()
        {
            bool success;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                success = Connector.Unfreeze(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_X);
                success &= Connector.Unfreeze(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Y);
                success &= Connector.Unfreeze(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Z);
            }
            else
            {
                success = Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_X);
                success &= Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Y);
                success &= Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Z);
            }
            return success;
        }
    }
}