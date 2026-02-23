using ConnectorLib;
using CrowdControl.Games.SmartEffects;
using System.Threading.Tasks;

namespace CrowdControl.Games.Packs.Sonic3DBlast;

public partial class Sonic3DBlast
{
    [EffectHandler("Bumper")]
    public class Bumper : EffectHandler<Sonic3DBlast, IGenesisConnector>
    {
        public Bumper(Sonic3DBlast pack) : base(pack) {
            new Task(UpdateBumpers).Start();
        }

        public override EffectHandlerType Type => EffectHandlerType.Instant;

        public override IList<string> Codes { get; } = new[] { "Bumper" };

        public override EffectPack.Mutex Mutexes { get; } = new[] { "" };

        //public override SITimeSpan FollowUpDelay => SITimeSpan.FromSeconds(5);
        private byte[][] BumperData { get; } = new byte[][]
        {
            new byte[]{
                0x00, 0x00, 0x0c, 0xc1,
                0x00, 0x0c, 0xce, 0xe8,
                0x00, 0xe9, 0xee, 0x58,
                0x09, 0xee, 0x55, 0x58,
                0x0e, 0xe5, 0x54, 0x81,
                0xe8, 0x88, 0x88, 0x81,
                0xe9, 0x81, 0x11, 0x11,
                0xee, 0x58, 0x88, 0x11,
            },
            new byte[]{
                0xee, 0x57, 0x78, 0x11,
                0xee, 0x54, 0x48, 0x11,
                0xef, 0xe5, 0x48, 0x18,
                0x0f, 0xf5, 0x88, 0x84,
                0x0e, 0xfe, 0x89, 0x74,
                0x00, 0xe9, 0x9f, 0x55,
                0x00, 0x0e, 0xf7, 0xff,
                0x00, 0x00, 0x0e, 0xff,
            },
            new byte[]{
                0x1c, 0xc0, 0x00, 0x00,
                0x8e, 0xec, 0xc0, 0x00,
                0x85, 0xee, 0x9e, 0x00,
                0x85, 0x55, 0xee, 0x90,
                0x18, 0x45, 0x5e, 0xe0,
                0x18, 0x88, 0x88, 0x8e,
                0x11, 0x11, 0x18, 0x9e,
                0x11, 0x88, 0x85, 0xee,
            },
            new byte[]{
                0x11, 0x87, 0x75, 0xee,
                0x11, 0x84, 0x54, 0xee,
                0x81, 0x84, 0x5e, 0xfe,
                0x48, 0x88, 0x5f, 0xf0,
                0x47, 0x98, 0xef, 0xe0,
                0x55, 0xf9, 0x9e, 0x00,
                0xff, 0x7f, 0xe0, 0x00,
                0xff, 0xe0, 0x00, 0x00,
            },
        };

        private List<uint> Bumpers = new List<uint>();

        public override bool StartCondition()
        {
            short anim = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_ANIMATION, out anim);
            else
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_ANIMATION, out anim);
            return anim != (short)SonicAnimations.DIEING && anim != (short)SonicAnimations.ON_A_RING;
        }

        public override bool StartAction()
        {
            // make sure we are VDP aligned
            ushort vdp_proxy = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                Connector.Write16(DirectorsCutAddresses.ADDR_VDP_DATA_PROXY, 0);
                Connector.Write16(DirectorsCutAddresses.ADDR_VDP_CONTROL_PROXY, (ushort)0xC000);
                Connector.Read16(DirectorsCutAddresses.ADDR_VDP_CONTROL_PROXY, out vdp_proxy);
                while (vdp_proxy != 0)
                {
                    Thread.Sleep(10);
                    Connector.Read16(DirectorsCutAddresses.ADDR_VDP_CONTROL_PROXY, out vdp_proxy);
                }
            }
            else
            {
                Connector.Write16(Sonic3DBlastAddresses.ADDR_VDP_DATA_PROXY, 0);
                Connector.Write16(Sonic3DBlastAddresses.ADDR_VDP_CONTROL_PROXY, (ushort)0xC000);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_VDP_CONTROL_PROXY, out vdp_proxy);
                while (vdp_proxy != 0)
                {
                    Thread.Sleep(10);
                    Connector.Read16(Sonic3DBlastAddresses.ADDR_VDP_CONTROL_PROXY, out vdp_proxy);
                }
            }
            // Load Bumper Sprite
            uint tile = 0;
            uint tile_offset = 0x40 * 0x20 - 0x30;
            while (tile < BumperData.Length)
            {
                Connector.Write((uint)((tile + tile_offset) * 0x20), "VRAM", BumperData[tile]);
                tile++;
            }
            short x = 0;
            short y = 0;
            short z = 0;
            short vel_x = 0;
            short vel_y = 0;
            short vel_z = 0;
            if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
            {
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_POSITION_X, out x);
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_POSITION_Y, out y);
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_POSITION_Z, out z);
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_X, out vel_x);
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Y, out vel_y);
                Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Z, out vel_z);
            }
            else
            {
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_POSITION_X, out x);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_POSITION_Y, out y);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_POSITION_Z, out z);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_X, out vel_x);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Y, out vel_y);
                Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Z, out vel_z);
            }
            uint object_ptr = EffectPack.CreateObjectAtXYZ(new SpawnData {
                collide_flag = 0x0, // special collide if == 0xXX00
                gravity = 0x0, // gravity
                sprite_flags = 0xC7D0, // !ppv hxxx xxxx xxxx !=priority p=palette v=flip_v h=flip_h x=index
                f_0x06 = 0xFFFF, // ---- ---- ---- ----
                f_0x0A = 0x306,
                lifetime = 600, // lifetime
                f_0x0E = 0x60000, // ---- ---- ---- -??- ---- ---- ---- ---- 
            }, (short)(x + vel_x), (short)(y + 0), (short)(z + vel_z));
            Bumpers.Add(object_ptr);
            return true;
        }

        private void UpdateBumpers()
        {
            while (true)
            {
                List<int> to_remove = new List<int>();
                for (int i = 0; i < Bumpers.Count; i++)
                {
                    Connector.Read16(Bumpers[i] + 0x32, out short lifetime);
                    if (lifetime == 0)
                    {
                        to_remove.Add(i);
                        continue;
                    }
                    Connector.Read16(Bumpers[i] + 0x06, out short x);
                    Connector.Read16(Bumpers[i] + 0x0a, out short y);
                    Connector.Read16(Bumpers[i] + 0x0e, out short z);
                    short s_x = 0;
                    short s_y = 0;
                    short s_z = 0;
                    if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                    {
                        Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_POSITION_X, out s_x);
                        Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_POSITION_Y, out s_y);
                        Connector.Read16(DirectorsCutAddresses.ADDR_SONIC_POSITION_Z, out s_z);
                    }
                    else
                    {
                        Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_POSITION_X, out s_x);
                        Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_POSITION_Y, out s_y);
                        Connector.Read16(Sonic3DBlastAddresses.ADDR_SONIC_POSITION_Z, out s_z);
                    }
                    if (Math.Abs(x - s_x) < 20 && Math.Abs(y - s_y) < 20 && Math.Abs(z - s_z) < 20)
                    {
                        ushort vel_x = 0;
                        ushort vel_z = 0;
                        if (Math.Abs(x - s_x) > 5)
                        {
                            if (s_x < x)
                            {
                                vel_x = 0xFF9C;
                            }
                            if (s_x > x)
                            {
                                vel_x = 0x0064;
                            }
                        }
                        if (Math.Abs(z - s_z) > 5)
                        {
                            if (s_z < z)
                            {
                                vel_z = 0xFF9C;
                            }
                            if (s_z > z)
                            {
                                vel_z = 0x0064;
                            }
                        }
                        if (vel_x == 0 && vel_z == 0)
                        {
                            vel_x = 0xFF9C;
                        }
                        if (EffectPack.rom_type == ROMType.DIRECTORS_CUT)
                        {
                            if (vel_x != 0)
                                Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_X, vel_x);
                            if (vel_z != 0)
                                Connector.Write16(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Z, vel_z);
                            Connector.Write16(DirectorsCutAddresses.ADDR_SOUND2, (ushort)Sounds.SFX_BUMPER);
                        }
                        else
                        {
                            if (vel_x != 0)
                                Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_X, vel_x);
                            if (vel_z != 0)
                                Connector.Write16(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Z, vel_z);
                            Connector.Write16(Sonic3DBlastAddresses.ADDR_SOUND2, (ushort)Sounds.SFX_BUMPER);
                        }
                    }
                }
                to_remove.Sort((a, b) => b.CompareTo(a));
                for (int i = 0; i < to_remove.Count; i++)
                {
                    Bumpers.RemoveAt(i);
                }
                Thread.Sleep(10);
            }
        }
    }
}