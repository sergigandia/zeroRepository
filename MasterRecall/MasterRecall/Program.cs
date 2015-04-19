using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using LeagueSharp;
using LeagueSharp.Common;
namespace MasterRecall
{
    class Program
    {
        private static  Obj_AI_Hero hero;
        private static Menu menu;
        private static Spell R;
        private static Obj_AI_Hero player;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += onGameLoad;
        }
        private static void loadMenu()
        {
            R = new Spell(SpellSlot.R);
            player = ObjectManager.Player;
            if (player.ChampionName != "Jinx") 
            R.SetSkillshot(0.7f, 140f, 1500f, false, SkillshotType.SkillshotLine); // jinx
            else if (player.ChampionName != "Ezreal") 
            R.SetSkillshot(1.2f, 160f, 2000f, false, SkillshotType.SkillshotLine); // ezreal
            else  if (player.ChampionName != "Ashe") 
            R.SetSkillshot(0.3f, 250f, 1600f, false, SkillshotType.SkillshotLine); // ashe
            else return;
            menu = new Menu("MasterRecallOfTheDie", "MasterRecallOfTheDie", true);
            var options = new Menu("options", "Options");
            {
                options.AddItem(new MenuItem("drawactive", "Draw Active").SetValue(true));
                options.AddItem(new MenuItem("active", "Active").SetValue(true));
                options.AddItem(new MenuItem("notuse", "Not Use Key").SetValue(new KeyBind(32, KeyBindType.Press)));
            }
            menu.AddSubMenu(options);
            Game.OnProcessPacket += OnProcessPacket;
            menu.AddToMainMenu();
        }

        static void onGameLoad(EventArgs args)
        {
            loadMenu();
            Game.PrintChat("MasterOfRecallOfTheDie Loaded ;)");
            Obj_AI_Base.OnTeleport += onTeleport;
       
        }
        public static int cont=0; 
        private static void OnProcessPacket(GamePacketEventArgs args)
        {
            short header = BitConverter.ToInt16(args.PacketData, 0);

            if (hero.IsRecalling())
            {
              //  Packet.S2C.Teleport.
          //     Game.PrintChat(cont+": Packet Header is: " + header +  " Lenght: " + BitConverter.ToString(args.PacketData, 0));
                cont++;
            }
            else
            {
                cont = 0;
            }
             
        }
        private static void onTeleport(GameObject sender, GameObjectTeleportEventArgs args)
        {
           //si alguien esta backeando
          //  Game.PrintChat("El chaval esta en fov");
            if (!menu.Item("active").GetValue<bool>()) return;

             if (HeroManager.Enemies
                   .FindAll(hero => hero.IsRecalling()) // 200 * 200
                   .OrderBy(h => h.Distance(Game.CursorPos, true)).FirstOrDefault() != null) // siempre que estes en fov esto sera null
              {
                  hero = HeroManager.Enemies
                   .FindAll(heros => heros.IsRecalling()) // 200 * 200
                   .OrderBy(h => h.Distance(Game.CursorPos, true)).FirstOrDefault(); //
              }
             else
             {
                 //aqui estamos en fov
                 Game.PrintChat("Ulti on fov dont work on this version wait :)");
             }

            //
              Drawing.OnDraw += draw;
        
        }
        private static void useR()
        {
            Program.R.Cast(hero); // Continue like that
        }
        private static void draw(EventArgs args)
        {
            if (hero == null || hero.IsDead ||!hero.IsRecalling()) return;
            Render.Circle.DrawCircle(hero.Position, 110, System.Drawing.Color.Blue, 5);
            if(R.IsKillable(hero))
            useR();
        }
    }
}
