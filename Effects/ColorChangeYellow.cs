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
    [EffectHandler("ColorChangeYellow")]
    public class ColorChangeYellow : ColorChange
    {
        public ColorChangeYellow(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override EffectPack.Mutex Mutexes { get; } = new[] { "color" };

        public override IList<string> Codes { get; } = new[] { "ColorChangeYellow" };

        public override ushort[] Colors { get; } = new ushort[]
        {
            0xAEE, //rgb(224,224,160)
            0x6EE, //rgb(224,224,96)
            0x4CC, //rgb(192,192,64)
            0x2AA, //rgb(160,160,32)
            0x266, //rgb(96,96,32)
            0x022, //rgb(32,32,0)
        };
    }
}
