using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ZephyrMod.NPCs.Boss
{
    [AutoloadBossHead]
    public class ArchImp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arch Imp");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BossImp");
            npc.lifeMax = 7200;
            npc.damage = 50;
            npc.width = 130;
            npc.defense = 2;
            npc.knockBackResist = 0f;
            npc.height = 164;
            animationType = 62;
            npc.aiStyle = 14;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.buffImmune[24] = true;
            npc.buffImmune[67] = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }


        public override void BossLoot(ref string name, ref int potionType)
        {
            Item.NewItem(npc.getRect(), mod.ItemType("SinBar"), 30 + Main.rand.Next(20));
            if (Main.expertMode)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ExpertImpTalon"));
            }
            if (!Main.expertMode && Main.rand.NextBool(10))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("VoodooBook"));
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 1.00f * bossLifeScale);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 1.2f);  //boss damage increase in expermode
        }
        public override void AI()
        {
            npc.ai[0]++;
            Player P = Main.player[npc.target];
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
            }
            npc.netUpdate = true;

            npc.ai[1]++;
            if (npc.ai[1] >= 145)  // 230 is projectile fire rate
            {
                float Speed = 16f;  //projectile speed
                Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
                int damage = 40;  //projectile damage
                int type = mod.ProjectileType("AI");  //put your projectile
                Main.PlaySound(26, (int)npc.position.X, (int)npc.position.Y, 17);
                float rotation = (float)Math.Atan2(vector8.Y - (P.position.Y + (P.height * 0.5f)), vector8.X - (P.position.X + (P.width * 0.5f)));
                int num54 = Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, 0);
                npc.ai[1] = 0;
            }
            if (npc.ai[0] % 1200 == 3)  //Npc spown rate

            {
                NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("ImpGuard"));  //NPC name
            }
            npc.ai[1] += 0;
            if (npc.life <= 1500)  //when the boss has less than 70 health he will do the charge attack
                npc.ai[2]++;                //Charge Attack
            if (npc.ai[2] >= 20)
            {
                npc.velocity.X *= 10.98f;
                npc.velocity.Y *= 10.98f;
                Vector2 vector8 = new Vector2(npc.position.X + (npc.width * 1.5f), npc.position.Y + (npc.height * 0.5f));
                {
                    float rotation = (float)Math.Atan2((vector8.Y) - (Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)), (vector8.X) - (Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)));
                    npc.velocity.X = (float)(Math.Cos(rotation) * 12) * -1;
                    npc.velocity.Y = (float)(Math.Sin(rotation) * 12) * -1;
                }
                //Dust
                npc.ai[0] %= (float)Math.PI * 2f;
                Vector2 offset = new Vector2((float)Math.Cos(npc.ai[0]), (float)Math.Sin(npc.ai[0]));
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 20);
                npc.ai[2] = -300;
                Color color = new Color();
                Rectangle rectangle = new Rectangle((int)npc.position.X, (int)(npc.position.Y + ((npc.height - npc.width) / 2)), npc.width, npc.width);
                int count = 30;
                for (int i = 1; i <= count; i++)
                {
                    int dust = Dust.NewDust(npc.position, rectangle.Width, rectangle.Height, 6, 0, 0, 100, color, 2.5f);
                    Main.dust[dust].noGravity = false;
                }
                return;
            }

        }
    }
}
//ThrowerEmblem