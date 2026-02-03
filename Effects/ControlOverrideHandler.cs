using ConnectorLib;
using CrowdControl.Games.SmartEffects;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    public abstract class ControlOverrideHandler : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public ControlOverrideHandler(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Durational;

        public override SITimeSpan RefreshInterval { get; } = SITimeSpan.FromSeconds(0.05);

        public override bool RefreshAction()
        {
            byte temp = 0;
            bool success = true;
            Buttons out_buttons = Buttons.BUTTON_NONE;
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
            if ((EffectPack.controlOverrides & ControlOverrides.INVERT_DPAD) != ControlOverrides.NONE)
            {
                if ((buttons & Buttons.BUTTON_UP) == Buttons.BUTTON_UP)
                    out_buttons |= Buttons.BUTTON_DOWN;
                if ((buttons & Buttons.BUTTON_DOWN) == Buttons.BUTTON_DOWN)
                    out_buttons |= Buttons.BUTTON_UP;
                if ((buttons & Buttons.BUTTON_LEFT) == Buttons.BUTTON_LEFT)
                    out_buttons |= Buttons.BUTTON_RIGHT;
                if ((buttons & Buttons.BUTTON_RIGHT) == Buttons.BUTTON_RIGHT)
                    out_buttons |= Buttons.BUTTON_LEFT;
                buttons &= ~Buttons.BUTTON_DPAD;
            }
            if ((EffectPack.controlOverrides & ControlOverrides.NO_JUMP) != ControlOverrides.NONE)
            {
                out_buttons |= Buttons.BUTTON_A | Buttons.BUTTON_C;
            }
            if ((EffectPack.controlOverrides & ControlOverrides.NO_SPIN) != ControlOverrides.NONE)
            {
                out_buttons |= Buttons.BUTTON_B;
            }
            out_buttons |= buttons;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                return success & Connector.Write16(DirectorsCutAddresses.ADDR_FORCED_CONTROLS, (byte)out_buttons);
            else
                return success & Connector.Write16(Sonic3DBlastAddresses.ADDR_FORCED_CONTROLS, (byte)out_buttons);
        }
    }
}