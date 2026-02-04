using ConnectorLib;
using ConnectorLib.Inject.Emulator;
using ConnectorLib.sd2snes.usb2snes;
using CrowdControl.Common;
using CrowdControl.Games.SmartEffects;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConnectorType = CrowdControl.Common.ConnectorType;


//ccpragma { "include" : [ ".\\Effects\\*.cs" ] }
namespace CrowdControl.Games.Packs.Sonic3DBlast
{
    [UsedImplicitly]
    public partial class Sonic3DBlast : GenesisEffectPack, IHandlerCollection
    {

        public override Game Game { get; } = new("Sonic 3D Blast", "Sonic3DBlast", "Genesis", ConnectorType.GenesisConnector);

        public Sonic3DBlast(UserRecord player, Func<CrowdControlBlock, bool> responseHandler, Action<object> statusUpdateHandler) : base(player, responseHandler, statusUpdateHandler)
        {
        }

        public override ROMTable ROMTable
        {
            get
            {
                return new[]
                {
                    new ROMInfo("Sonic 3D Blast (USA, Europe, Korea) (En).md", null, Patching.Ignore, ROMStatus.ValidPatched, s => Patching.MD5(s, "50ACBEA2461D179B2BF11460A1CC6409")),
                    new ROMInfo("Sonic 3D Blast - Director's Cut (World) (Unl).md", null, Patching.Ignore, ROMStatus.ValidPatched, s => Patching.MD5(s, "742DD5C98D143FF6716800DEE6A25DC5")),
                };
            }
        }

        public override EffectList Effects
        {
            get
            {
                List<Effect> effects =
                [
                    new("Give Ring", "AddRing")
                        { Price = 1, Description = "Give the player a ring." },
                    new("Take Ring", "TakeRing")
                        { Price = 2, Description = "Take a ring from the player" },
                    new("Slap", "Slap")
                        { Price = 5, Description = "Give the player a nice Slap." },
                    new("Kill", "Kill")
                        { Price = 100, Description = "Kills the player." },
                    new("SEGA!", "SEGA")
                        { Price = 10, SessionCooldown = 10, Description = "SEGA!" },
                    new("Give Shield", "Shield")
                        { Price = 10, Description = "Gives A Shield to the player." },
                    new("Give Yellow Shield", "YellowShield")
                        { Price = 20, Description = "Gives A Yellow Shield to the player." },
                    new("Give Red Shield", "RedShield")
                        { Price = 20, Description = "Gives A Red Shield to the player." },
                    new("Grant Invincibility", "Invincibility")
                        { Price = 20, Description = "Gives Invicibility to the player." },
                    new("Grant Speed Shoes", "SpeedShoes")
                        { Price = 20, Description = "Gives Speed Shoes to the player." },
                    new("Drop Flicky", "LoseFlicky")
                        { Price = 20, Description = "Force the player to drop a flicky." },
                    new("Shove North", "ShoveNorth")
                        { Price = 10, Description = "Shove the player north." },
                    new("Shove West", "ShoveWest")
                        { Price = 10, Description = "Shove the player west." },
                    new("Shove South", "ShoveSouth")
                        { Price = 10, Description = "Shove the player south." },
                    new("Shove East", "ShoveEast")
                        { Price = 10, Description = "Shove the player east." },
                    new("Launch", "Launch")
                        { Price = 20, Description = "Launch the player." },
                    new("Stop", "Stop")
                        { Price = 25, Description = "Stop the player for a moment." },
                    new("Invert Controls", "InvertControls")
                        { Price = 50, Duration = 15, Description = "Invert the players controls." },
                    new("No Jump", "NoJump")
                        { Price = 50, Duration = 15, Description = "Prevent the player from jumping." },
                    new("No Spin", "NoSpin")
                        { Price = 50, Duration = 15, Description = "Prevent the player from spindashing." },
                    new("Freeze Player", "Freeze")
                        { Price = 75, Description = "Freeze the player." },
                    /*new("Spawn Bumper", "Bumper")
                        { Price = 10, Description = "Spawns a bumper." },*/
                ];
                return effects;
            }
        }

        protected override GameState GetGameState()
        {
            if (Connector.IsEqual16(ADDR_CHECKSUM, SONIC3D_BLAST_CHECKSUM))
            {
                rom_type = ROMType.DEFAULT;
            } 
            else if (Connector.IsEqual16(ADDR_CHECKSUM, SONIC3D_BLAST_DX_CHECKSUM))
            {
                rom_type = ROMType.DIRECTORS_CUT;
            }
            else
            {
                rom_type = ROMType.UNKNOWN;
            }
            if (old_rom_type != rom_type)
            {
                Log.Message($"Rom found: {rom_type}");
                old_rom_type = rom_type;
            }
            switch (rom_type)
            {
                case ROMType.DIRECTORS_CUT:
                    {
                        if (!Connector.IsEqual16(DirectorsCutAddresses.ADDR_SPECIAL_STAGE_FLAG, 0))
                            return GameState.WrongMode;
                        if (!Connector.IsEqual16(DirectorsCutAddresses.ADDR_IN_DEMO, 0))
                            return GameState.Cutscene;
                        if (!Connector.IsEqual16(DirectorsCutAddresses.ADDR_PAUSE_STATE, 0))
                            return GameState.Paused;
                        if (!Connector.IsEqual16(DirectorsCutAddresses.ADDR_LEVEL_CLEAR_SCREEN, 0))
                            return GameState.Loading;
                        if (!Connector.IsEqual16(DirectorsCutAddresses.ADDR_CURRENT_LEVEL, 0))
                        {
                            if (!Connector.Freeze16(DirectorsCutAddresses.ADDR_LIVES, 9))
                                return GameState.Error;
                            return GameState.Ready;
                        }
                        return GameState.Loading;
                    }
                case ROMType.UNKNOWN:
                case ROMType.DEFAULT:
                    {
                        if (!Connector.IsEqual16(Sonic3DBlastAddresses.ADDR_SPECIAL_STAGE_FLAG, 0))
                            return GameState.WrongMode;
                        if (!Connector.IsEqual16(Sonic3DBlastAddresses.ADDR_IN_DEMO, 0))
                            return GameState.Cutscene;
                        if (!Connector.IsEqual16(Sonic3DBlastAddresses.ADDR_PAUSE_STATE, 0))
                            return GameState.Paused;
                        if (!Connector.IsEqual16(Sonic3DBlastAddresses.ADDR_LEVEL_CLEAR_SCREEN, 0))
                            return GameState.Loading;
                        if (!Connector.IsEqual16(Sonic3DBlastAddresses.ADDR_CURRENT_LEVEL, 0))
                        {
                            if (!Connector.Freeze16(Sonic3DBlastAddresses.ADDR_LIVES, 9))
                                return GameState.Error;
                            return GameState.Ready;
                        }
                        return GameState.Loading;
                    }
            }
            return GameState.Error;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private ControlOverrides controlOverrides = ControlOverrides.NONE;
        private ROMType rom_type = ROMType.UNKNOWN;
        private ROMType old_rom_type = ROMType.UNKNOWN;

        private const ushort SONIC3D_BLAST_CHECKSUM = 0xA934;
        private const ushort SONIC3D_BLAST_DX_CHECKSUM = 0x71D4;
        private const uint ADDR_CHECKSUM = 0x0000018E;
        private const uint ADDR_RAM = 0x00FF0000;

        public class Sonic3DBlastAddresses
        {
            public const uint ADDR_CURRENT_LEVEL = 0x00FF0678;
            public const uint ADDR_CURRENT_LEVEL_INDEX = 0x00FF067E;
            public const uint ADDR_IN_DEMO = 0x00FF0410;
            public const uint ADDR_LIVES = 0x00FF0680;
            public const uint ADDR_SPECIAL_STAGE_FLAG = 0x00FF0684;
            public const uint ADDR_CONTROLS_OVERRIDDEN_BACKUP = 0x00FF06BA;
            public const uint ADDR_FORCED_CONTROLS_TIMER = 0x00FF0A18;
            public const uint ADDR_FORCED_CONTROLS = 0x00FF0A1A;
            public const uint ADDR_RINGS_HUD = 0x00FF0A56;
            public const uint ADDR_MONITOR_EFFECT = 0x00FF0A86;
            public const uint ADDR_RINGS = 0x00FF0A5A;
            public const uint ADDR_FLICKIES_FOLLOWING = 0x00FF0A8E;
            public const uint ADDR_FLICKIES_SCATTER = 0x00FF0AA6;
            public const uint ADDR_SHIELD = 0x00FF0AC2;
            public const uint ADDR_SOUND1 = 0x00FF0AC4;
            public const uint ADDR_SOUND2 = 0x00FF0AC6;
            public const uint ADDR_CURRENT_BGM = 0x00FF0AC8;
            public const uint ADDR_PAUSE_STATE = 0x00FF0AE0;
            public const uint ADDR_LEVEL_CLEAR_SCREEN = 0x00FF0B76;

            public const uint ADDR_SONIC = 0x00FFC1E6;
            // position and velocity are 2.2 byte fixed point numbers
            public const uint ADDR_SONIC_POSITION_X = 0x00FFC1EC;
            public const uint ADDR_SONIC_POSITION_Y = 0x00FFC1F0;
            public const uint ADDR_SONIC_POSITION_Z = 0x00FFC1F4;
            public const uint ADDR_SONIC_VELOCITY_X = 0x00FFC1F8;
            public const uint ADDR_SONIC_VELOCITY_Y = 0x00FFC1FC;
            public const uint ADDR_SONIC_VELOCITY_Z = 0x00FFC200;
            public const uint ADDR_SONIC_ANGLE = 0x00FFC218;
            public const uint ADDR_SONIC_ANGLE_TARGET = 0x00FFC21A;
            public const uint ADDR_SONIC_JUST_HURT = 0x00FFC224;
            public const uint ADDR_SONIC_40 = 0x00FFC226;
            public const uint ADDR_SONIC_ON_GROUND = 0x00FFC22A;
            // Set to 0 to launch player
            public const uint ADDR_SONIC_48 = 0x00FFC22E;
            public const uint ADDR_SONIC_FLOOR_HEIGHT = 0x00FFC232;
            public const uint ADDR_SONIC_INVINCIBILITY = 0x00FFC262;
            public const uint ADDR_SONIC_SPEED_SHOES = 0x00FFC264;
            public const uint ADDR_SONIC_ANIMATION = 0x00FFC27A;

            public const uint ADDR_OBJECT_OFFSET = 0x00FFC28C;
            public const uint ADDR_NEXT_OBJECT = 0x00FF0AAE;
        }

        public class DirectorsCutAddresses
        {
            // has to be non-zero for DX exclusive effects
            public const uint ADDR_DX_ENABLED = 0x00FF0410;

            public const uint ADDR_CURRENT_LEVEL = 0x00FF06B2;
            public const uint ADDR_CURRENT_LEVEL_INDEX = 0x00FF06B8;
            public const uint ADDR_IN_DEMO = 0x00FF0440;
            public const uint ADDR_LIVES = 0x00FF06BA;
            public const uint ADDR_SPECIAL_STAGE_FLAG = 0x00FF06BE;
            public const uint ADDR_CONTROLS_OVERRIDDEN_BACKUP = 0x00FF03BF;
            public const uint ADDR_FORCED_CONTROLS_TIMER = 0x00FF0A60;
            public const uint ADDR_FORCED_CONTROLS = 0x00FF0A62;
            public const uint ADDR_RINGS_HUD = 0x00FF0A9E; // Hex index per digit
            public const uint ADDR_MONITOR_EFFECT = 0x00FF0AF0;
            public const uint ADDR_RINGS = 0x00FF0AA2;
            public const uint ADDR_FLICKIES_FOLLOWING = 0x00FF0AF8;
            public const uint ADDR_FLICKIES_SCATTER = 0x00FF0B10;
            public const uint ADDR_SHIELD = 0x00FF0B2C;
            public const uint ADDR_SOUND1 = 0x00FF0B2E;
            public const uint ADDR_SOUND2 = 0x00FF0B30;
            public const uint ADDR_CURRENT_BGM = 0x00FF0B32;
            public const uint ADDR_PAUSE_STATE = 0x00FF0BB0;
            public const uint ADDR_LEVEL_CLEAR_SCREEN = 0x00FF0C48;

            public const uint ADDR_SONIC = 0x00FFC358;
            // position and velocity are 2.2 byte fixed point numbers
            public const uint ADDR_SONIC_POSITION_X = 0x00FFC35E;
            public const uint ADDR_SONIC_POSITION_Y = 0x00FFC362;
            public const uint ADDR_SONIC_POSITION_Z = 0x00FFC366;
            public const uint ADDR_SONIC_VELOCITY_X = 0x00FFC36A;
            public const uint ADDR_SONIC_VELOCITY_Y = 0x00FFC36E;
            public const uint ADDR_SONIC_VELOCITY_Z = 0x00FFC372;
            public const uint ADDR_SONIC_ANGLE = 0x00FFC38A;
            public const uint ADDR_SONIC_ANGLE_TARGET = 0x00FFC38C;
            public const uint ADDR_SONIC_JUST_HURT = 0x00FFC396;
            public const uint ADDR_SONIC_40 = 0x00FFC398;
            public const uint ADDR_SONIC_ON_GROUND = 0x00FFC39C;
            // Set to 0 to launch player
            public const uint ADDR_SONIC_48 = 0x00FFC3A0;
            public const uint ADDR_SONIC_FLOOR_HEIGHT = 0x00FFC3A4;
            public const uint ADDR_SONIC_INVINCIBILITY = 0x00FFC3D4;
            public const uint ADDR_SONIC_SPEED_SHOES = 0x00FFC3D6;
            public const uint ADDR_SONIC_ANIMATION = 0x00FFC3EE;

            public const uint ADDR_OBJECT_OFFSET = 0x00FFC400;
            public const uint ADDR_NEXT_OBJECT = 0x00FF0B18;
        }

        enum ROMType : byte
        {
            UNKNOWN,
            DEFAULT,
            DIRECTORS_CUT,
        }

        [Flags]
        enum ControlOverrides : byte
        {
            NONE = 0,
            INVERT_DPAD = 1,
            NO_JUMP = 2,
            NO_SPIN = 4,
        }

        enum SonicAnimations : byte
        {
            IDLE,
            RUNNING,
            SPINNING,
            DUCKING,
            STOPPING_LEFT,
            STOPPING_RIGHT,
            STOPPING_BACK,
            SPINDASH,
            GETTING_HURT,
            ON_A_RING,
            STOPPING_ROTATING,
            WAITING,
            ON_FANS,
            SPRING,
            SPRING_NO_FALL,
            SLIDING,
            ROTATING, //RRZ
            DIEING,
            FROZEN, //DDZ
            SLIPPING, //DDZ
            // DX only
            TRANSFORM_SUPER,
        }

        enum Shields : ushort
        {
            NONE = 0,
            DEFAULT = 0x46C0,
            YELLOW = 0x66C0,
            RED = 0x06C0
        }

        enum MonitorEffects : ushort
        {
            NONE = 0,
            EXTRA_LIFE = 0x80,
            SHIELD = 0x180,
            YELLOW_SHIELD = 0x200,
            INCINCIBILITY = 0x280,
            SPEED_SHOES = 0x300,
            RED_SHIELD = 0x380,
        }

        enum Enemies : byte
        {
            NONE = 0,
            GGZ_ANT = 0x04,
            GGZ_SPIKE_BALL = 0x08,
            GGZ_BADNIK = 0x0C,

            DDZ_ICE_CANNON = 0xD4,

            DDZ_ICE_DUST = 0xE4,
        }

        [Flags]
        enum Buttons : ushort
        {
            BUTTON_NONE = 0x00,
            BUTTON_UP = 0x01,
            BUTTON_DOWN = 0x02,
            BUTTON_LEFT = 0x04,
            BUTTON_RIGHT = 0x08,
            BUTTON_DPAD = BUTTON_UP | BUTTON_DOWN | BUTTON_LEFT | BUTTON_RIGHT,
            BUTTON_B = 0x10, // Spin
            BUTTON_C = 0x20, // Jump
            BUTTON_A = 0x40, // Jump
            BUTTON_ABC = BUTTON_B | BUTTON_A | BUTTON_C,
            BUTTON_START = 0x80,
            BUTTON_ALL = BUTTON_DPAD | BUTTON_ABC | BUTTON_START,
            // 6 Button controls (unused?)
            BUTTON_Z = 0x0100,
            BUTTON_Y = 0x0200,
            BUTTON_X = 0x0400,
            BUTTON_XYZ = BUTTON_Z | BUTTON_Y | BUTTON_X,
            BUTTON_ABCXYZ = BUTTON_ABC | BUTTON_XYZ,
            BUTTON_MODE = 0x0800,
            BUTTON_STARTMODE = BUTTON_START | BUTTON_MODE,
            BUTTON_ALL_6BUTTON = BUTTON_ALL | BUTTON_XYZ | BUTTON_MODE,
        }

        enum Sounds : byte
        {
            NONE = 0,
            BGM_GGZ1 = 0x1,
            BGM_GGZ2 = 0x2,
            BGM_RRZ1 = 0x3,
            BGM_RRZ2 = 0x4,
            BGM_VVZ2 = 0x5,
            BGM_VVZ1 = 0x6,
            BGM_SSZ1 = 0x7,
            BGM_SSZ2 = 0x8,
            BGM_DDZ1 = 0x9,
            BGM_DDZ2 = 0xA,
            BGM_GeGaZ1 = 0xB,
            BGM_GeGaZ2 = 0xC,
            BGM_PPZ2 = 0xD,
            BGM_FFZ = 0xE,
            BGM_OUTRO = 0xF,
            BGM_SS = 0x10,
            BGM_PPZ1 = 0x11,
            BGM_BOSS2 = 0x12,
            BGM_BOSS1 = 0x13,
            BGM_INTRO = 0x14,
            BGM_CREDITS = 0x15,
            JINGLE_GAMEOVER = 0x16,
            BGM_CONTINUE = 0x17,
            JINGLE_LEVELCLEAR = 0x18,
            JINGLE_LIFE_GET = 0x19, // Beeps at the end
            JINGLE_CHAOS_EMERALD = 0x1A,
            BGM_INVINCIBILITY = 0x1B,
            BGM_MAIN_MENU = 0x1C,
            // Remaining ids map to main menu
            BGM_END = 0x32,
            // SFX (a lot of these seem left over from S3K)
            SFX_RING_RIGHT = 0x33,
            SFX_RING_LEFT = 0x34,
            SFX_DIE = 0x35,
            SFX_SKID = 0x36,
            SFX_HIT_SPIKES = 0x37,
            SFX_COLLECT_FLICKY = 0x38,
            SFX_SPLASH = 0x39,
            SFX_SHIELD = 0x3A,
            SFX_FLICKY_CHIRP = 0x3B,
            SFX_ROLL = 0x3C,
            SFX_BADNIK_POP = 0x3D,
            SFX_SHIELD_LOW = 0x3E,
            SFX_SHIELD_BUBBLE = 0x3F,
            SFX_BEEP = 0x40,
            SFX_SHIELD_LIGHTNING = 0x41,
            SFX_INSTASHIELD = 0x42,
            SFX_SHIELD_FIRE_DASH = 0x43,
            SFX_SHIELD_BUBBLE_BOUNCE = 0x44,
            SFX_SHIELD_LIGHTNING_JUMP = 0x45,
            SFX_FLICKY_CHIRPS = 0x46,
            SFX_GRAB = 0x4A,
            SFX_BULLET_SHOOT = 0x4D,
            SFX_EXPLOSION = 0x4E,
            SFX_ALARM = 0x50,
            SFX_BOMB_FALL = 0x51,
            SFX_SPIKES_MOVE = 0x52,
            SFX_CRUMBLE = 0x59,
            SFX_SPIN_CHARGE = 0x5A,
            SFX_MENU_TICK = 0x5B,
            SFX_JUMP = 0x62,
            SFX_WOOSH = 0x66,
            SFX_WARP = 0x6A,
            SFX_FREEZE = 0x80,
            SFX_CHAIN = 0x94,
            SFX_COLLIDE = 0x9A,
            SFX_BUMPER = 0xAA,
            SFX_SPINDASHREV = 0xAB,
            SFX_SCORE_DING = 0xB0,
            SFX_SPRING = 0xB1,
            SFX_SPINDASH_RELEASE = 0xB6,
            SFX_GAMBA_REEL = 0xB7,
            SFX_SIREN = 0xBE,
            SFX_SWING = 0xC9,
            SFX_DEZ_RINGS = 0xCA,
            SFX_SAW = 0xD8,
            // starting from 0xDA random silence and noise
            SFX_CTRL_FADEOUT = 0xE2,
            SFX_CTRL_UNKN = 0xE3,
            SFX_SEGA_SCREAM = 0xFF
        }

        protected override bool IsReady(EffectRequest? request)
        {
            return GetGameState() == GameState.Ready && Connector.Read8(0x00b1, out byte b) && (b < 0x80);
        }

        public override bool StopAllEffects()
        {
            bool success = base.StopAllEffects(); 
            try
            {
                if (rom_type == ROMType.DIRECTORS_CUT)
                {
                    success &= Connector.Unfreeze(DirectorsCutAddresses.ADDR_LIVES);
                    success &= Connector.Unfreeze(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_X);
                    success &= Connector.Unfreeze(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Y);
                    success &= Connector.Unfreeze(DirectorsCutAddresses.ADDR_SONIC_VELOCITY_Z);
                }
                else
                {
                    success &= Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_LIVES);
                    success &= Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_X);
                    success &= Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Y);
                    success &= Connector.Unfreeze(Sonic3DBlastAddresses.ADDR_SONIC_VELOCITY_Z);
                }
            }
            catch { success = false; }
            return success;
        }

        public struct SpawnData
        {
            public ushort collide_flag = 0;
            public ushort gravity = 0;
            public ushort sprite_flags = 0;
            public uint f_0x06 = 0;
            public ushort f_0x0A = 0;
            public ushort lifetime = 0;
            public uint f_0x0E = 0;

            public SpawnData() { }
        }

        public uint GetNextObjectSlot()
        {
            uint offset_base = rom_type == ROMType.DIRECTORS_CUT ? DirectorsCutAddresses.ADDR_OBJECT_OFFSET : Sonic3DBlastAddresses.ADDR_OBJECT_OFFSET;
            short next_offset = 0;
            uint object_ptr = 0;
            bool success = true;
            while (true)
            {
                if (rom_type == ROMType.DIRECTORS_CUT)
                    success &= Connector.Read16(DirectorsCutAddresses.ADDR_NEXT_OBJECT, out next_offset);
                else
                    success &= Connector.Read16(Sonic3DBlastAddresses.ADDR_NEXT_OBJECT, out next_offset);
                next_offset += 0x38;
                if (next_offset > 0x68f)
                    next_offset = 0;

                if (rom_type == ROMType.DIRECTORS_CUT)
                    success &= Connector.Write16(DirectorsCutAddresses.ADDR_NEXT_OBJECT, next_offset);
                else
                    success &= Connector.Write16(Sonic3DBlastAddresses.ADDR_NEXT_OBJECT, next_offset);

                object_ptr = (uint)(offset_base + next_offset);
                short x_pos = 0;
                success &= Connector.Read16(object_ptr + 0x6, out x_pos);
                short flag = 0;
                success &= Connector.Read16(object_ptr + 0x30, out flag);
                if (x_pos == 0 || (flag & 0x100) == 0)
                    break;
                offset_base = (uint)(offset_base - next_offset);
            }
            return object_ptr;
        }

        public uint CreateObjectAtXYZ(SpawnData spawn_data, short x, short y, short z)
        {
            uint object_ptr = GetNextObjectSlot();
            bool success = Connector.Write16(object_ptr + 0x04, spawn_data.collide_flag);
            success &= Connector.Write16(object_ptr + 0x06, x);
            success &= Connector.Write16(object_ptr + 0x08, 0);
            success &= Connector.Write16(object_ptr + 0x0A, y);
            success &= Connector.Write16(object_ptr + 0x0C, 0);
            success &= Connector.Write16(object_ptr + 0x0E, z);
            success &= Connector.Write16(object_ptr + 0x10, 0);
            success &= Connector.Write16(object_ptr + 0x22, 0x0); // id
            success &= Connector.Write16(object_ptr + 0x24, spawn_data.gravity);
            success &= Connector.Write16(object_ptr + 0x26, (ushort)((z & 0xFFE0) * 8 + (x & 0xFFE0) >> 4));
            success &= Connector.Write16(object_ptr + 0x28, 0);
            success &= Connector.Write16(object_ptr + 0x2A, (ushort)(spawn_data.f_0x06 >> 16));
            success &= Connector.Write16(object_ptr + 0x2C, (ushort)(spawn_data.f_0x06 & 0xFFFF));
            success &= Connector.Write16(object_ptr + 0x2E, spawn_data.sprite_flags);
            success &= Connector.Write16(object_ptr + 0x30, spawn_data.f_0x0A);
            success &= Connector.Write16(object_ptr + 0x32, spawn_data.lifetime);
            success &= Connector.Write16(object_ptr + 0x34, (ushort)(spawn_data.f_0x0E >> 16));
            success &= Connector.Write16(object_ptr + 0x36, (ushort)(spawn_data.f_0x0E & 0xFFFF));
            if (!success)
                return 0xFFFFFFFF;
            return object_ptr;
        }
    }
}
