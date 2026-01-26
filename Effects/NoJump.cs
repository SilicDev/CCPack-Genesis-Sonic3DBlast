using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("NoJump")]
    public class NoJump : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public NoJump(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Durational;

        public override IList<string> Codes { get; } = new[] { "NoJump" };

        public override SITimeSpan RefreshInterval { get; } = SITimeSpan.FromSeconds(0.05);

        public override bool StartAction()
        {
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                return Connector.IsEqual16(DirectorsCutAddresses.ADDR_FORCED_CONTROLS_TIMER, 0);
            else
                return Connector.IsEqual16(Sonic3DBlastAddresses.ADDR_FORCED_CONTROLS_TIMER, 0);
        }

        public override bool RefreshAction()
        {
            byte temp = 0;
            bool success = true;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                success &= Connector.Write16(DirectorsCutAddresses.ADDR_FORCED_CONTROLS_TIMER, 5);
                success &= Connector.Read8(DirectorsCutAddresses.ADDR_CONTROLS_OVERRIDDEN_BACKUP, out temp);
            }
            else
            {
                success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_FORCED_CONTROLS_TIMER, 5);
                success &= Connector.Read8(Sonic3DBlastAddresses.ADDR_CONTROLS_OVERRIDDEN_BACKUP, out temp);
            }
            Buttons buttons = (Buttons)temp;
            Buttons out_buttons = (buttons & (Buttons.BUTTON_DPAD | Buttons.BUTTON_B | Buttons.BUTTON_START)) | (Buttons.BUTTON_A | Buttons.BUTTON_C);
            Buttons old = EffectPack.buttons_to_send == Buttons.BUTTON_ALL ? buttons : EffectPack.buttons_to_send;
            //EffectPack.buttons_to_send = old & (Buttons.BUTTON_DPAD | Buttons.BUTTON_B | Buttons.BUTTON_START) | (out_buttons & (Buttons.BUTTON_A | Buttons.BUTTON_C));
            //Connector.SendMessage(string.Format("R {0:X} W {1:X}", buttons, out_buttons));
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                return success & Connector.Write16(DirectorsCutAddresses.ADDR_FORCED_CONTROLS, (byte)out_buttons);
            else
                return success & Connector.Write16(Sonic3DBlastAddresses.ADDR_FORCED_CONTROLS, (byte)out_buttons);
        }

        public override bool StopAction()
        {
            EffectPack.buttons_to_send = Buttons.BUTTON_ALL;
            return true;
        }
    }
}