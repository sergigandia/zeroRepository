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
        private static int InterruptNum;

        public static List<String> InterruptSpell; 
        static readonly string[] Interrupt = new[]
            {
                "KatarinaR", "GalioIdolOfDurand", "Crowstorm", "Drain", "AbsoluteZero", "ShenStandUnited", "UrgotSwap2",
                "AlZaharNetherGrasp", "FallenOne", "Pantheon_GrandSkyfall_Jump", "VarusQ", "CaitlynAceintheHole",
                "MissFortuneBulletTime", "InfiniteDuress", "LucianR"
            };
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }
        public static SpellSlot Trans(int i)
        {
            switch (i)
            {
                case 0:
                    return SpellSlot.Q;
                case 1:
                    return SpellSlot.W;
                case 2:
                    return SpellSlot.E;
                case 3:
                    return SpellSlot.R;
            }
            return SpellSlot.Q;
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

                   var DrawMenu = new Menu("DrawMenu", "Draw Menu");
            {
                DrawMenu.AddItem(new MenuItem("QS", "Q Score").SetValue(true));
                DrawMenu.AddItem(new MenuItem("QD", "Q draw").SetValue(true));
               DrawMenu.AddItem(new MenuItem("RD", "R draw").SetValue(true));
            }
            List<String> spellsin = new List<string>();
            foreach (Obj_AI_Hero hero in HeroManager.Enemies)
            {
                for (int i = 0; i < 4; i++)
                {
                    //  hero.GetSpell(Trans(i)).Name;
                    foreach (String s in Interrupt)
                    {
                        if (s == hero.GetSpell(Trans(i)).Name)
                        {
                            spellsin.Add("[" + hero.ChampionName + "]" + s);
                        }
                    }
                }
            }
         InterruptSpell = spellsin;
            int num = 0;
            var interruptMenu = new Menu("SpellInterrupt", "R Interrupt spells");
            {
                interruptMenu.AddItem(new MenuItem("UseRInterrupt", "Use R Interrupt").SetValue(true));
                foreach (String s in spellsin)
                {
                    interruptMenu.AddItem(new MenuItem("S" + num, s).SetValue(true));
                    num++;
                }
            }
            InterruptNum = num;
            TargetSelector.AddToMenu(TargetSelectorMenu);
            menu.AddSubMenu(orbWalkerMenu); //ORBWALKER
            menu.AddSubMenu(TargetSelectorMenu);
            menu.AddSubMenu(comboMenu);
               menu.AddSubMenu(DrawMenu);
              menu.AddToMainMenu();
        }

        private static int goodgrabs;

        private static int totalgrabs;
        public static void Game_ProcessSpell(Obj_AI_Base hero, GameObjectProcessSpellCastEventArgs args)
        {
            if (hero.IsMe && args.SData.Name == "RocketGrabMissile")
            {
                totalgrabs++;
            }
            if (menu.Item("UseRInterrupt").GetValue<bool>())
                for (int i = 0; i < InterruptNum; i++)
                {
                    if (menu.Item("S" + i).GetValue<bool>())
                    {

                        if (args.SData.Name == InterruptSpell[i])
                        {
                            if (Player.Distance(hero) <= 550)
                            {
                                Program.E.Cast(hero);
                            }
                        }
                    }
                }
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
            Obj_AI_Base.OnProcessSpellCast += Game_ProcessSpell;
            Obj_AI_Base.OnBuffAdd += Game_ProcessSpell;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;

        }

        private static void Game_ProcessSpell(Obj_AI_Base sender, Obj_AI_BaseBuffAddEventArgs args)
        {
       //   rocketgrab2
            if (args.Buff.Name == "rocketgrab2")
            {
                goodgrabs++;
            }
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
            if (menu.Item("QS").GetValue<bool>())

            {
                Drawing.DrawText(10, 150, System.Drawing.Color.Yellow, "Total Q : " +goodgrabs);
                Drawing.DrawText(10, 175, System.Drawing.Color.Yellow, "Total Good Q : " + totalgrabs);
                var percent = ((float)goodgrabs / (float)totalgrabs) * 100f;
                Drawing.DrawText(10, 200, System.Drawing.Color.Yellow, "Q successful % : " + percent + "%");
            }
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
