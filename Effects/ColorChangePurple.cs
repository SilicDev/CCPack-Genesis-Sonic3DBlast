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
    [EffectHandler("ColorChangePurple")]
    public class ColorChangePurple : ColorChange
    {
        public ColorChangePurple(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override EffectPack.Mutex Mutexes { get; } = new[] { "color" };

        public override IList<string> Codes { get; } = new[] { "ColorChangePurple" };

        public override ushort[] Colors { get; } = new ushort[]
        {
            0xEAA, //rgb(160,160,224)
            0xE66, //rgb(96,96,224)
            0xC44, //rgb(64,64,192)
            0xA22, //rgb(32,32,160)
            0x622, //rgb(32,32,96)
            0x200, //rgb(0,0,32)
        };
    }
}
