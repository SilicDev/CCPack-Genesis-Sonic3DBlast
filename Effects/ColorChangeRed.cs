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
    [EffectHandler("ColorChangeRed")]
    public class ColorChangeRed : ColorChange
    {
        public ColorChangeRed(Sonic3DBlast pack) : base(pack) { }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override EffectPack.Mutex Mutexes { get; } = new[] { "color" };

        public override IList<string> Codes { get; } = new[] { "ColorChangeRed" };

        public override ushort[] Colors { get; } = new ushort[]
        {
            0xA4E, //rgb(224,64,160)
            0x60E, //rgb(224,0,96)
            0x42C, //rgb(192,32,64)
            0x22A, //rgb(160,32,32)
            0x206, //rgb(96,0,32)
            0x002, //rgb(32,0,0)
        };
    }
}
