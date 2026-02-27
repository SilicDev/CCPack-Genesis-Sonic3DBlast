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

        public override SITimeSpan FollowUpDelay => SITimeSpan.FromSeconds(2.5);

        public override bool StartCondition()
        {
            if (EffectPack.IsSpecialStage())
            {
                EffectPack.Respond(Request, EffectStatus.FailTemporary, "Unavailable in special stages!");
                return false;
            }
            return base.StartCondition();
        }

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                bool success = true;
                if (EffectPack.IsSpecialStage())
                {
                    success &= Connector.Freeze16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_X, 0);
                    success &= Connector.Freeze16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Y, 0);
                    success &= Connector.Freeze16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Z, 0);
                }
                else
                {
                    success &= Connector.Freeze16(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_VELOCITY_X, 0);
                    success &= Connector.Freeze16(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_VELOCITY_Y, 0);
                    success &= Connector.Freeze16(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_VELOCITY_Z, 0);
                }
                return success;
            }
            else
            {
                bool success = true;
                if (EffectPack.IsSpecialStage())
                {
                    success &= Connector.Freeze16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_X, 0);
                    success &= Connector.Freeze16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Y, 0);
                    success &= Connector.Freeze16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Z, 0);
                }
                else
                {
                    success &= Connector.Freeze16(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_VELOCITY_X, 0);
                    success &= Connector.Freeze16(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_VELOCITY_Y, 0);
                    success &= Connector.Freeze16(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_VELOCITY_Z, 0);
                }
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
                success &= Connector.Unfreeze(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_VELOCITY_X);
                success &= Connector.Unfreeze(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_VELOCITY_Y);
                success &= Connector.Unfreeze(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_VELOCITY_Z);
            }
            else
            {
                success = Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_X);
                success &= Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Y);
                success &= Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Z);
                success &= Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_VELOCITY_X);
                success &= Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_VELOCITY_Y);
                success &= Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_VELOCITY_Z);
            }
            return success;
        }
    }
}