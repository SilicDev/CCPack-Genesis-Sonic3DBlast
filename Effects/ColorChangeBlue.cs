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
    [EffectHandler("ColorChangeBlue")]
    public class ColorChangeBlue : ColorChange
    {
        public ColorChangeBlue(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override EffectPack.Mutex Mutexes { get; } = new[] { "color" };

        public override IList<string> Codes { get; } = new[] { "ColorChangeBlue" };

        public override ushort[] Colors { get; } = new ushort[]
        {
            0xEA4, //rgb(64,160,224)
            0xE60, //rgb(0,96,224)
            0xC42, //rgb(32,64,192)
            0xA22, //rgb(32,32,160)
            0x620, //rgb(0,32,96)
            0x200, //rgb(0,0,32)
        };
    }
}
