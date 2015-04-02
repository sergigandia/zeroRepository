using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace MasterOfInsec
{
    static class Program
    {
        // Master Of Insec LEE


        private static Obj_AI_Hero Player;
        private static Menu menu;
        private static Orbwalking.Orbwalker orb;
        public static Spell Q, W, E, R;
        public static bool SecondQ;
        public static SpellSlot Ignite;
        public static Orbwalking.Orbwalker Orbwalker { get; internal set; }

        public static Orbwalking.OrbwalkingMode OrbwalkerMode
        {
            get { return Orbwalker.ActiveMode; }
        }

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }



        static void Menu()
        {
            menu = new Menu("Master Of Insec", "MasterOfInsec", true);
            var orbWalkerMenu = new Menu("Orbwalker", "Orbwalker");
            orb = new Orbwalking.Orbwalker(orbWalkerMenu);
            var TargetSelectorMenu = new Menu("TargetSelector", "TargetSelector");
            var comboMenu = new Menu("Combo", "Combo");
            {
                comboMenu.AddItem(new MenuItem("Set Q range", "Set Q range").SetValue(new Slider(1100, 0, 1100)));
                comboMenu.AddItem(new MenuItem("comboQ", "Use Q in combo").SetValue(true));
                comboMenu.AddItem(new MenuItem("comboW", "Use W in combo").SetValue(false));
                comboMenu.AddItem(new MenuItem("comboWLH", "Use W if low hp").SetValue(false));
                comboMenu.AddItem(new MenuItem("Set W life %", "Set % W").SetValue(new Slider(100, 0, 100)));
                comboMenu.AddItem(new MenuItem("comboE", "Use E in combo").SetValue(true));
                comboMenu.AddItem(new MenuItem("comboR", "Use R to finish the enemy").SetValue(true));
                comboMenu.AddItem(new MenuItem("Ignite", "USE ignite for kill").SetValue(true));
            }
            var InsecSettingsMenu = new Menu("Insec Settings", "Insec Settings");
            {
                InsecSettingsMenu.AddItem(new MenuItem("Insec Mode", "Insec Mode").SetValue(true));
                InsecSettingsMenu.AddItem(new MenuItem("Not Yet", "Not Yet").SetValue(true));
            }
            var FleeMenu = new Menu("FleeMenu", "Flee Menu");
            {
                FleeMenu.AddItem(new MenuItem("Ward Jump", "Ward Jump").SetValue(new KeyBind('Z', KeyBindType.Press)));
            }
            var LaneclearMenu = new Menu("Laneclear", "Laneclear");
            {
                LaneclearMenu.AddItem(new MenuItem("Use Q in Laneclear", "Use Q in Laneclear").SetValue(true));
                LaneclearMenu.AddItem(new MenuItem("Use W in Laneclear", "Use W in Laneclear").SetValue(false));
                LaneclearMenu.AddItem(new MenuItem("Use E in Laneclear", "Use E in Laneclear").SetValue(true));
            }
            var JungleclearMenu = new Menu("Jungleclear", "Jungleclear");
            {
                JungleclearMenu.AddItem(new MenuItem("Use Q in Laneclear", "Use Q in Laneclear").SetValue(true));
                JungleclearMenu.AddItem(new MenuItem("Use W in Laneclear", "Use W in Laneclear").SetValue(false));
                JungleclearMenu.AddItem(new MenuItem("Use E in Laneclear", "Use E in Laneclear").SetValue(true));
            }
            var DrawSettingsMenu = new Menu("Draw Settings", "Draw Settings");
            {
                //    DrawSettingsMenu.AddItem(new MenuItem("Draw Insec Line", "Draw Insec Line").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("DrawKilleableText", "Draw Killeable Text").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("Draw Q Range", "Draw Q Range").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("Draw W Range", "Draw W Range").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("Draw E Range", "Draw E Range").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("Draw R Range", "Draw R Range").SetValue(true));
            }

            TargetSelector.AddToMenu(TargetSelectorMenu);
            menu.AddSubMenu(comboMenu);          //COMBO
            menu.AddSubMenu(InsecSettingsMenu);  //INSEC
            menu.AddSubMenu(LaneclearMenu);        //LANECLEAR
            menu.AddSubMenu(JungleclearMenu);      //JUNGLECLEAR
            menu.AddSubMenu(FleeMenu);     
            menu.AddSubMenu(DrawSettingsMenu);     //DRAWS
            menu.AddSubMenu(TargetSelectorMenu);   //TS
            menu.AddSubMenu(orbWalkerMenu);        //ORBWALKER
            menu.AddToMainMenu();
        }
        static void OnGameLoad(EventArgs args)
        {
            // if (Player.ChampionName == "LeeSin") return;
            Player = ObjectManager.Player;
            //   Ignite = new Spell(SpellSlot.Summoner1, 1100);
            Q = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 700);
            E = new Spell(SpellSlot.E, 430);
            R = new Spell(SpellSlot.R, 375);
            Ignite = ObjectManager.Player.GetSpellSlot("SummonerDot");
            Q.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
            Menu();
            Game.PrintChat("[LeeSin]Master Of Insec load good luck ;) ver 0.0.4.3");
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnGameUpdate;

        }
        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead) return;
              KsIgnite();
         //     WardJump();
            switch (orb.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Mixed:
                    break;

                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
            }

        }
        private static void WardJump()
        {
            if (menu.Item("Ward Jump").GetValue<bool>())
            {

            }
        }
        private static InventorySlot getBestWardItem()
        {
            InventorySlot ward = Items.GetWardSlot();
            if (ward == default(InventorySlot)) return null;
            return ward;
        }
        private static void Combo()
        {
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            if (Q.IsReady() && GetBool("comboQ") && Q.IsInRange(target))
            {
                if (Q.Instance.Name == "BlindMonkQOne" && Player.Distance(target.Position) <= menu.Item("Set Q range").GetValue<Slider>().Value)
                {
                    Q.CastIfHitchanceEquals(target, HitChance.High); // Continue like that
                }
                else
                {
                    Q.Cast(target);
                }
            }
            if (E.IsReady() && GetBool("comboE") && E.IsInRange(target))
            {
                E.Cast(); // 
            }
            if (W.IsReady() && GetBool("comboW"))
            {
                if (ObjectManager.Player.HealthPercent <= menu.Item("Set W life %").GetValue<Slider>().Value)
                {
                    W.Cast(Player);
                }
            }
            if (R.IsReady() && GetBool("comboR") && R.IsKillable(target))
            {
                R.Cast(target);
            }
            if (Ignite.IsReady())
            {
                if (target.Health - ObjectManager.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite) <= 0)
                 ObjectManager.Player.Spellbook.CastSpell(Ignite, target);
            }
        }

        public static void KsIgnite()
        {
                if (Ignite.IsReady())
                {
                    var igniteKillableEnemy =
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(x => x.IsEnemy)
                        .Where(x => !x.IsDead)
                        .Where(x => x.Distance(ObjectManager.Player.Position) <= 400)
                        .FirstOrDefault();
                    if (igniteKillableEnemy.Health - ObjectManager.Player.GetSummonerSpellDamage(igniteKillableEnemy, Damage.SummonerSpell.Ignite) <= 0)
                        ObjectManager.Player.Spellbook.CastSpell(Ignite, igniteKillableEnemy);
                }
        }

        public static float GetComboDamage(Obj_AI_Hero target)
        {
            return (float)GetMainComboDamage(target);
        }
        public static double GetMainComboDamage(Obj_AI_Base target)
        {
            double damage = Player.GetAutoAttackDamage(target);

            if (Q.IsReady())
                damage += Player.GetSpellDamage(target, SpellSlot.Q)*2;

            if (W.IsReady())
                damage += Player.GetSpellDamage(target, SpellSlot.W);

            if (E.IsReady())
                damage += Player.GetSpellDamage(target, SpellSlot.E);

            if (R.IsReady())
                damage += Player.GetSpellDamage(target, SpellSlot.R);

            if (Ignite.IsReady())
                damage += Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);

            return damage;
        }
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead) return;
            if (menu.Item("Draw Q Range").GetValue<bool>())
            {
                Render.Circle.DrawCircle(Player.Position, menu.Item("Set Q range").GetValue<Slider>().Value, System.Drawing.Color.Green);
            }
            if (menu.Item("Draw W Range").GetValue<bool>())
            {
                Render.Circle.DrawCircle(Player.Position, 700f, System.Drawing.Color.Green);
            }
            if (menu.Item("Draw E Range").GetValue<bool>())
            {
                Render.Circle.DrawCircle(Player.Position, 430f, System.Drawing.Color.Green);
            }
            if (menu.Item("Draw R Range").GetValue<bool>())
            {
                Render.Circle.DrawCircle(Player.Position, 375f, System.Drawing.Color.Green);
            }
            //draw % de vida
            if(menu.Item("DrawKilleableText").GetValue<bool>())
            {
                var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
              if(target.Health<=GetComboDamage(Player))
              {
                  var wts = Drawing.WorldToScreen(target.Position);
                  Drawing.DrawText(wts[0] - 35, wts[1] + 10, System.Drawing.Color.White, "Killeable");
              }
            }
        }

        public static T GetValue<T>(string name)
        {
            return menu.Item(name).GetValue<T>();
        }

        public static bool GetBool(string name)
        {
            return GetValue<bool>(name);
        }

    }
}
