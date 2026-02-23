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
    [EffectHandler("ColorChangeOrange")]
    public class ColorChangeOrange : ColorChange
    {
        public ColorChangeOrange(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override EffectPack.Mutex Mutexes { get; } = new[] { "color" };

        public override IList<string> Codes { get; } = new[] { "ColorChangeOrange" };

        public override ushort[] Colors { get; } = new ushort[]
        {
            0x4AE, //rgb(224,160,64)
            0x06E, //rgb(224,96,0)
            0x24C, //rgb(192,64,32)
            0x22A, //rgb(160,32,32)
            0x026, //rgb(96,32,0)
            0x002, //rgb(32,0,0)
        };
    }
}
