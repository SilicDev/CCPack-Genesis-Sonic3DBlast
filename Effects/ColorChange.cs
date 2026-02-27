using ConnectorLib;
using CrowdControl.Games.SmartEffects;
using System.Threading.Tasks;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    public abstract class ColorChange : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public ColorChange(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override EffectPack.Mutex Mutexes { get; } = new[] { "color" };

        public virtual ushort[] Colors { get; } = [0,0,0,0,0,0];

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
            new Task(ChangeColor).Start();
            return true;
        }

        private void ChangeColor()
        {
            uint pos = 0;
            ushort vdp_proxy = 0;
            while (pos < Colors.Length)
            {
                if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                {
                    Connector.Write16(DirectorsCutAddresses.ADDR_VDP_DATA_PROXY, Colors[pos]);
                    Connector.Write16(DirectorsCutAddresses.ADDR_VDP_CONTROL_PROXY, (ushort)(0xC000 + (0x44 + pos * 2)));
                    Connector.Read16(DirectorsCutAddresses.ADDR_VDP_CONTROL_PROXY, out vdp_proxy);
                    while (vdp_proxy != 0)
                    {
                        Thread.Sleep(10);
                        Connector.Read16(DirectorsCutAddresses.ADDR_VDP_CONTROL_PROXY, out vdp_proxy);
                    }
                }
                else
                {
                    Connector.Write16(Sonic3DBlastAddresses.ADDR_VDP_DATA_PROXY, Colors[pos]);
                    Connector.Write16(Sonic3DBlastAddresses.ADDR_VDP_CONTROL_PROXY, (ushort)(0xC000 + (0x44 + pos * 2)));
                    Connector.Read16(Sonic3DBlastAddresses.ADDR_VDP_CONTROL_PROXY, out vdp_proxy);
                    while (vdp_proxy != 0)
                    {
                        Thread.Sleep(10);
                        Connector.Read16(Sonic3DBlastAddresses.ADDR_VDP_CONTROL_PROXY, out vdp_proxy);
                    }
                }
                pos++;
            }
        }
    }
}