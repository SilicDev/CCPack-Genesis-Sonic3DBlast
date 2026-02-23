using ConnectorLib;
using CrowdControl.Games.SmartEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdControl.Games.Packs.Sonic3DBlast;
public partial class Sonic3DBlast
{
    [EffectHandler("ColorChangeGreen")]
    public class ColorChangeGreen : ColorChange
    {
        public ColorChangeGreen(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override EffectPack.Mutex Mutexes { get; } = new[] { "color" };

        public override IList<string> Codes { get; } = new[] { "ColorChangeGreen" };

        public override ushort[] Colors { get; } = new ushort[]
        {
            0xAE4, //rgb(64,224,160)
            0x6E0, //rgb(0,224,96)
            0x4C2, //rgb(32,192,64)
            0x2A2, //rgb(32,160,32)
            0x260, //rgb(0,96,32)
            0x020, //rgb(0,32,0)
        };
    }
}
