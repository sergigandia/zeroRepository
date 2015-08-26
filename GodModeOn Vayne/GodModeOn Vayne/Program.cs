using LeagueSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using SharpDX;

namespace GodModeOn_Vayne
{
    class Program
    {
        public static Obj_AI_Hero Player;
        public static Spell Q, W, E, R;
        public static SpellSlot Flash;
        public static Menu menu;
        private static Orbwalking.Orbwalker orb;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }
        private static void OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;
            if (Player.ChampionName.ToLower() != "vayne") return;
            Q = new Spell(SpellSlot.Q, 0f);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 545f);
            R = new Spell(SpellSlot.R);
            Flash = Player.GetSpellSlot("summonerflash");
            Menu();
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }
        private static Obj_AI_Hero targetE()
        {
            return TargetSelector.GetTarget(550,TargetSelector.DamageType.Physical,false);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
                            var DrawE = Program.menu.Item("DE").GetValue<bool>();
                            var DrawR = Program.menu.Item("DR").GetValue<bool>();
                            if (DrawR)
                            {
                                Render.Circle.DrawCircle(Player.Position, 800f, System.Drawing.Color.YellowGreen, 2);
                            }
                            if (DrawE && E.IsReady())
                            {
                        //        var wtst = Drawing.WorldToScreen(Efinishpos(targetE()));
                       //         var wtsp = Drawing.WorldToScreen(targetE().Position);
                       //         Drawing.DrawLine(wtsp.X, wtsp.Y, wtst.X, wtst.Y, 5f, System.Drawing.Color.Red);
                             //   Drawing.DrawCircle(Player.Position, 100, System.Drawing.Color.Yellow);
                                var d = targetE().Position.Distance(Program.Efinishpos(targetE()));
                                for (var i = 0; i < d; i += 10)
                                {
                                    var dist = i > d ? d : i;
                                    var point = targetE().Position.Extend(Program.Efinishpos(targetE()), dist);
                                    Render.Circle.DrawCircle(point, 1, System.Drawing.Color.YellowGreen);
                                }
                            }
        }

       public static Vector3 Efinishpos(Obj_AI_Base ts)
        {
            return Player.Position.Extend(ts.Position, ObjectManager.Player.Distance(ts.Position) + 490);
        
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (menu.Item("combokey").GetValue<KeyBind>().Active)
            {
                Combo.Combo.Do();
            }
            if (menu.Item("lanekey").GetValue<KeyBind>().Active)
            {
                Combo.LaneClear.Do();
            }
            if (menu.Item("junglekey").GetValue<KeyBind>().Active)
            {
                Combo.JungleClear.Do();
            }
        }
        private static void Menu()
        {
            menu = new Menu("GodModeOn Vayne", "GodModeOn Vayne", true);
            var orbWalkerMenu = new Menu("Orbwalker", "Orbwalker");
            orb = new Orbwalking.Orbwalker(orbWalkerMenu);
            var TargetSelectorMenu = new Menu("TargetSelector", "TargetSelector");


            var ComboMenu = new Menu("Combo", "Combo");
            {
                ComboMenu.AddItem(new MenuItem("QC", "Use Q in combo").SetValue(true));
                ComboMenu.AddItem(new MenuItem("EC", "AutoEWall").SetValue(true));
                ComboMenu.AddItem(new MenuItem("ECT", "OnlyUse E to sel. target").SetValue(true));
                ComboMenu.AddItem(new MenuItem("combokey", "Combo key").SetValue(new KeyBind(32, KeyBindType.Press)));
                ComboMenu.AddItem(new MenuItem("RC", "Use R in combo").SetValue(true));
                ComboMenu.AddItem(new MenuItem("CNr", "Num enemys for R")).SetValue(new Slider(2, 1, 5));
                //        ComboMenu.AddItem(new MenuItem());
                //        ComboMenu.AddItem(new MenuItem("EC", "Q+E").SetValue(true));
            }
            var LaneClearMenu = new Menu("LaneClear", "LaneClear");
            {
                LaneClearMenu.AddItem(new MenuItem("QL", "Use Q in laneclear").SetValue(true));
               LaneClearMenu.AddItem(new MenuItem("lanekey", "LaneClear key").SetValue(new KeyBind('V', KeyBindType.Press)));
            }
            var JungleClearMenu = new Menu("JungleClear", "JungleClear");
            {
                JungleClearMenu.AddItem(new MenuItem("QJ", "Use Q in laneclear").SetValue(true));
                JungleClearMenu.AddItem(new MenuItem("EJ", "Use E in laneclear").SetValue(true));
                JungleClearMenu.AddItem(new MenuItem("junglekey", "jungleClear key").SetValue(new KeyBind('V', KeyBindType.Press)));
            }
            var DrawMenu = new Menu("Draw", "Draw");
            {
                DrawMenu.AddItem(new MenuItem("DE", "Draw e line").SetValue(true));
                DrawMenu.AddItem(new MenuItem("DR", "Draw R range").SetValue(true));
            }
            TargetSelector.AddToMenu(TargetSelectorMenu);
            menu.AddSubMenu(orbWalkerMenu);
            menu.AddSubMenu(TargetSelectorMenu);
            menu.AddSubMenu(ComboMenu);
            menu.AddSubMenu(LaneClearMenu);
            menu.AddSubMenu(JungleClearMenu);
            menu.AddSubMenu(DrawMenu);
            menu.AddToMainMenu();
        }
    }
}
