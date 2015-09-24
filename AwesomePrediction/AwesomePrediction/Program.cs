using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaPrediction
{
    class Program
    {
        public static Obj_AI_Hero Player;
        private static Menu _menu;
        public static Spell Q,R,E;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }
        private static void OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;
           Q = new Spell(SpellSlot.Q, 1000f);
           E = new Spell(SpellSlot.E, 0f);
           R = new Spell(SpellSlot.R, 450f);
            Q.SetSkillshot(0.25f, 70f, 1800f, true, SkillshotType.SkillshotLine);
       //     Game.PrintChat("" + Player.BoundingRadius);
            Drawing.OnDraw += draw;
            Menu();
            Game.OnUpdate += OnGameUpdate;
        }

        private static void OnGameUpdate(EventArgs args)
        {
            if (_menu.Item("combokey").GetValue<KeyBind>().Active)
            {
                var qcombo = Program._menu.Item("QC").GetValue<bool>();

                var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);

                if (qcombo)
                {
              //      Game.PrintChat("tiramos el gancho");
                    Prediction.SpellPrediction(Program.Q, target);
                //    if (R.IsReady()&&R.IsInRange((target)) && R.IsKillable((target))) R.Cast();
                    if(E.IsReady()&&Player.Distance(target)<200) E.Cast();
                        
                
                    //        Prediction.SpellAOEPrediction(Program.R);
                }
            }
        }
        public static void Menu()
        {
            _menu = new Menu("Prediction Try", "Prediction Try", true);
            var orbWalkerMenu = new Menu("Orbwalker", "Orbwalker");
            var orbwalker = new Orbwalking.Orbwalker(orbWalkerMenu);
            var targetSelectorMenu = new Menu("TargetSelector", "TargetSelector");
            var comboMenu = new Menu("Combo", "Combo");
            {
                comboMenu.AddItem(new MenuItem("QC", "Use Q in combo").SetValue(true));
                comboMenu.AddItem(new MenuItem("combokey", "Combo key").SetValue(new KeyBind(32, KeyBindType.Press)));
            }
            TargetSelector.AddToMenu(targetSelectorMenu);
            _menu.AddSubMenu(orbWalkerMenu);        //ORBWALKER
            _menu.AddSubMenu(targetSelectorMenu);   //TS
            _menu.AddSubMenu(comboMenu);
            //      comboMenu.AddSubMenu(rMenu);
            _menu.AddToMainMenu();
        }

        public static void draw(EventArgs args)
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            if(target!=null)
            {
                Prediction.MinionCollideLine(Player.Position,target.Position,Q);
                Render.Circle.DrawCircle(Player.Position, Q.Range, System.Drawing.Color.Red, 2);
                Render.Circle.DrawCircle(Player.Position, 300, System.Drawing.Color.Red, 2);
            }
            else
            {
                Render.Circle.DrawCircle(Player.Position, 300, System.Drawing.Color.YellowGreen, 2);
                Render.Circle.DrawCircle(Player.Position, Q.Range, System.Drawing.Color.YellowGreen, 2);
            }
            Render.Circle.DrawCircle(target.Position, target.BoundingRadius , System.Drawing.Color.Yellow, 2);
            
        }
    }
}
