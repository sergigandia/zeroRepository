using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
namespace PerfectBlitzcranck
{
    using SharpDX;

    class Program
    {
        private static Menu menu;
        public static Spell Q, E, R;
        public static Obj_AI_Hero Player;

        public static Orbwalking.Orbwalker orb;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        public static void loadMenu()
        {
                       menu = new Menu("AwesomeBlitz", "AwesomeBlitz", true);
                  var orbWalkerMenu = new Menu("Orbwalker", "Orbwalker");
            orb = new Orbwalking.Orbwalker(orbWalkerMenu);
            var TargetSelectorMenu = new Menu("TargetSelector", "TargetSelector");
                 var comboMenu = new Menu("Combo", "Combo");
            {
                      comboMenu.AddItem(new MenuItem("useQ", "Use Q").SetValue(true));
                    comboMenu.AddItem(new MenuItem("useE", "Use E").SetValue(true));
                  comboMenu.AddItem(new MenuItem("useR", "Use R").SetValue(true));
                  comboMenu.AddItem(new MenuItem("dontuseR", "Dont Use R if target will dead").SetValue(true));
            }
                   var DrawMenu = new Menu("Combo", "Combo");
            {
                DrawMenu.AddItem(new MenuItem("QD", "Q draw").SetValue(true));
               DrawMenu.AddItem(new MenuItem("RD", "R draw").SetValue(true));
            }
            TargetSelector.AddToMenu(TargetSelectorMenu);
            menu.AddSubMenu(orbWalkerMenu); //ORBWALKER
            menu.AddSubMenu(TargetSelectorMenu);
            menu.AddSubMenu(comboMenu);
               menu.AddSubMenu(DrawMenu);
              menu.AddToMainMenu();
        }


        private static void OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;
            if (Player.ChampionName.ToLower() != "blitzcrank") return;
            Q = new Spell(SpellSlot.Q, 950f);
            Q.SetSkillshot(0.25f, 70f, 1800f, true, SkillshotType.SkillshotLine);
            E = new Spell(SpellSlot.E, 150f);
            R = new Spell(SpellSlot.R, 550f);
            loadMenu();
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;

        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Orbwalking.OrbwalkingMode.Combo == orb.ActiveMode)
            {
                
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            if (Q.IsInRange(target) && menu.Item("useQ").GetValue<bool>())
                {
                    AwesomePrediction.Awesome.SpellPrediction(Q, target,true);
                }
            if (E.IsInRange(target) && menu.Item("useE").GetValue<bool>())
                {
                    E.Cast();
                }
                if (R.IsInRange(target) && menu.Item("useR").GetValue<bool>())
                {
                    if (menu.Item("dontuseR").GetValue<bool>()) // dontuseR
                    {
                        if (!R.IsKillable(target))
                        {
                            R.Cast();
                        }
                    }
                    else
                    {
                        R.Cast();
                    }
                }

            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead) return;
            if (menu.Item("QD").GetValue<bool>())
            {
                Drawing.DrawCircle(Player.Position, Q.Range, System.Drawing.Color.YellowGreen);
            }
            if (menu.Item("RD").GetValue<bool>())
            {
                Drawing.DrawCircle(Player.Position, R.Range, System.Drawing.Color.YellowGreen);
            }

        }
       


    }
}
