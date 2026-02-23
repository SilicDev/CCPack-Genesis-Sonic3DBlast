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
    [EffectHandler("ColorChangePink")]
    public class ColorChangePink : ColorChange
    {
        public ColorChangePink(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override EffectPack.Mutex Mutexes { get; } = new[] { "color" };

        public override IList<string> Codes { get; } = new[] { "ColorChangePink" };

        public override ushort[] Colors { get; } = new ushort[]
        {
            0xEAE, //rgb(224,160,224)
            0xE6E, //rgb(224,96,224)
            0xC4C, //rgb(192,64,192)
            0xA2A, //rgb(160,32,160)
            0x626, //rgb(96,32,96)
            0x202, //rgb(32,0,32)
        };
    }
}
