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

        //combo working
        //ignite working
        //ward jump x

        public  static Map map;
        public static Obj_AI_Hero Player;
        public static Menu menu;
        private static Orbwalking.Orbwalker orb;
        public static Spell Q, W, E, R,RInsec,QHarrash;
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
                comboMenu.AddItem(new MenuItem("seth", "Q Hitchance")).SetValue(new Slider(3, 1, 4));
                comboMenu.AddItem(new MenuItem("comboQ", "Use Q in combo").SetValue(true));
                comboMenu.AddItem(new MenuItem("comboW", "Use W in combo").SetValue(false));
                comboMenu.AddItem(new MenuItem("comboWLH", "Use W if low hp").SetValue(false));
                comboMenu.AddItem(new MenuItem("Set W life %", "Set % W").SetValue(new Slider(100, 0, 100)));
                comboMenu.AddItem(new MenuItem("comboE", "Use E in combo").SetValue(true));
                comboMenu.AddItem(new MenuItem("comboR", "Use R to finish the enemy").SetValue(true));
                comboMenu.AddItem(new MenuItem("IgniteR", "Use R+Ignite for kill").SetValue(true));
                comboMenu.AddItem(new MenuItem("Ignite", "Use ignite for kill").SetValue(true));
                comboMenu.AddItem(new MenuItem("combokey", "Combo key").SetValue(new KeyBind(32, KeyBindType.Press)));
                comboMenu.AddItem(new MenuItem("Starcombokey", "StarCombo key").SetValue(new KeyBind('X', KeyBindType.Press)));
                comboMenu.AddItem(new MenuItem("starcombo", "StarCombo: E1 + Q1 + R + delay + Q2"));
            }
            var HarrashMenu = new Menu("Harrash", "Harrash");
            {
                HarrashMenu.AddItem(new MenuItem("QH", "Use Q in Harrash").SetValue(true));
                HarrashMenu.AddItem(new MenuItem("WH", "Use W for go out").SetValue(false));
                HarrashMenu.AddItem(new MenuItem("EH", "Use E in Harrash").SetValue(true));
                HarrashMenu.AddItem(new MenuItem("Harrash key", "Harrash key").SetValue(new KeyBind('C', KeyBindType.Press)));
            }
            var InsecSettingsMenu = new Menu("Insec Settings", "Insec Settings");
            {
//                InsecSettingsMenu.AddItem(new MenuItem("Mode", "Mode").SetValue(new StringList(new[] { "Insec to Tower", "Insec to Ally", "Insec to Mouse" }, 1))); 
                InsecSettingsMenu.AddItem(new MenuItem("inseckey", "Insec key").SetValue(new KeyBind('T', KeyBindType.Press)));
                InsecSettingsMenu.AddItem(new MenuItem("useflash", "Use flash if not ward").SetValue(true));
            }
            var LaneclearMenu = new Menu("Laneclear", "Laneclear");
            {
                LaneclearMenu.AddItem(new MenuItem("QL", "Use Q in Laneclear").SetValue(true));
                LaneclearMenu.AddItem(new MenuItem("WL", "Use W in Laneclear").SetValue(false));
                LaneclearMenu.AddItem(new MenuItem("EL", "Use E in Laneclear").SetValue(true));
               LaneclearMenu.AddItem(new MenuItem("laneclearkey", "LaneClear key").SetValue(new KeyBind('V', KeyBindType.Press)));
            }
            var JungleclearMenu = new Menu("Jungleclear", "Jungleclear");
            {
                JungleclearMenu.AddItem(new MenuItem("QJ", "Use Q in JungleClear").SetValue(true));
                JungleclearMenu.AddItem(new MenuItem("WJ", "Use W in JungleClear").SetValue(false));
                JungleclearMenu.AddItem(new MenuItem("EJ", "Use E in JungleClear").SetValue(true));
                JungleclearMenu.AddItem(new MenuItem("jungleclearkey", "JungleClear key").SetValue(new KeyBind('V', KeyBindType.Press)));
            }
            var ItemMenu = new Menu("Item Menu", "itemmenu");
            {
               ItemMenu.AddItem(new MenuItem("tiamat","Use Tiamat").SetValue(true));
               ItemMenu.AddItem(new MenuItem("hydra", "Ravenous Hydra").SetValue(true));
               ItemMenu.AddItem(new MenuItem("yomu", "Youmuu's Ghostblade").SetValue(true));
            }
            var DrawSettingsMenu = new Menu("Draw Settings", "Draw Settings");
            {
                DrawSettingsMenu.AddItem(new MenuItem("DrawInsec", "Draw Insec Line").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("DrawKilleableText", "Draw Killeable Text").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("Draw Q Range", "Draw Q Range").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("Draw W Range", "Draw W Range").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("Draw E Range", "Draw E Range").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("Draw R Range", "Draw R Range").SetValue(true));
            }

            TargetSelector.AddToMenu(TargetSelectorMenu);
            menu.AddSubMenu(orbWalkerMenu);        //ORBWALKER
            menu.AddSubMenu(TargetSelectorMenu);   //TS
            menu.AddSubMenu(comboMenu);//COMBO
            menu.AddSubMenu(HarrashMenu);  //Harrash
            menu.AddSubMenu(InsecSettingsMenu);  //INSEC
            menu.AddSubMenu(ItemMenu);
            menu.AddSubMenu(LaneclearMenu);        //LANECLEAR
            menu.AddSubMenu(JungleclearMenu);      //JUNGLECLEAR
            menu.AddItem(new MenuItem("wardjump", "WardJump key").SetValue(new KeyBind('Z', KeyBindType.Press)));
            menu.AddSubMenu(DrawSettingsMenu);     //DRAWS
            menu.AddToMainMenu();
        }
        static void OnGameLoad(EventArgs args)
        {
            // if (Player.ChampionName == "LeeSin") return;
            map = new Map();
            Player = ObjectManager.Player;
            //   Ignite = new Spell(SpellSlot.Summoner1, 1100);
            Q = new Spell(SpellSlot.Q, 1100);
            QHarrash = new Spell(SpellSlot.Q,700);
            W = new Spell(SpellSlot.W, 700);
            E = new Spell(SpellSlot.E, 430);
            R = new Spell(SpellSlot.R, 375);
            RInsec = new Spell(SpellSlot.R, 375);
            Ignite = ObjectManager.Player.GetSpellSlot("SummonerDot");
            QHarrash.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
            Q.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
            RInsec.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
            Menu();
            Game.PrintChat("[LeeSin]Master Of Insec load good luck ;) ver 0.0.9.0.1");
            Drawing.OnDraw += Drawing_OnDraw;
            Obj_AI_Base.OnProcessSpellCast += Oncast;
            Game.OnUpdate += Game_OnGameUpdate;

        }
        public static void Oncast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
{
if (!sender.IsMe) return;
if (args.SData.Name == R.ChargedSpellName && MasterOfInsec.Insec.Steps == "five")
    MasterOfInsec.Insec.Steps = "one";
}
        private static void Game_OnGameUpdate(EventArgs args)
        {
            Q.MinHitChance = HitchanceCheck(menu.Item("seth").GetValue<Slider>().Value);
            if (Player.IsDead) return;
         //   KsIgnite();
            if(menu.Item("combokey").GetValue<KeyBind>().Active)
                Combo();
            if (menu.Item("wardjump").GetValue<KeyBind>().Active)
                WardJump.jump();
           if (menu.Item("jungleclearkey").GetValue<KeyBind>().Active)
                JungleClear();
            if (menu.Item("Harrash key").GetValue<KeyBind>().Active)
            {
                Harrash();
            }
            if (menu.Item("Starcombokey").GetValue<KeyBind>().Active)
            {
                StarCombo();
            }
            else
            {
                steps = "One";
            }
            if (menu.Item("inseckey").GetValue<KeyBind>().Active)
            {
              MasterOfInsec.Insec.updateInsec();
             //   Insec();
            }
            else
            {
               MasterOfInsec.Insec.ResetInsecStats();
                oldPositionbool = true;
               
            }
        }
        static bool oldPositionbool;
        static bool HarrashComplete;
        static Vector3 oldPosition;
        static bool da;
        static string steps="One";
        static bool StarComboa;
        public static void StarCombo()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, 150));
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            if (!StarComboa)
            {
                if (!R.IsReady() || !Q.IsReady() || !E.IsReady() || target == null) return;
            }
               StarComboa = true;
            if (steps == "One") //First hit q
            {
                if (E.IsInRange(target, E.Range) && E.CanCast(target))
                {

                    E.Cast();
                    steps = "Two";
                }

            }
            else if (steps == "Two") // hit second q
            {
                if (Program.Q.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "BlindMonkQOne")
                {
                    Program.Q.CastIfHitchanceEquals(target, Program.HitchanceCheck(Program.menu.Item("seth").GetValue<Slider>().Value)); // Continue like that
                    steps = "Three";
                }
            }
            else if (steps == "Three") // hit second q
            {
                //
                if (R.CanCast(target))
                {
                    Program.R.CastOnUnit(target);
                    steps = "Four";
                }
            }
            else if (steps == "Four") // hit second q
            {
                if (Q.CanCast(target))
                {

                    Utility.DelayAction.Add(500, () => CastQ());
                }
            }
            else
            {
                steps = "One";
                StarComboa = false;
            }

        }
        public static void CastQ()
        {
            Q.Cast();
            steps = "One";
            StarComboa = false;
        }
        public static Vector3 Insecpos(Obj_AI_Hero ts)
        {
            return Game.CursorPos.Extend(ts.Position, Game.CursorPos.Distance(ts.Position) + 250);
        }
        public static Obj_AI_Hero GetWInsecTarget()
        {
return  ObjectManager.Get<Obj_AI_Hero>()
                 .Where(x => x.IsEnemy)
                 .Where(x => !x.IsDead)
                 .Where(x => x.Distance(ObjectManager.Player.Position) <= W.Range)
                 .FirstOrDefault();
        }
        private static void Harrash()
        {
            if (oldPositionbool)
            {
                 oldPosition = Player.Position;
                oldPositionbool = false;
            }
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            if (target != null)
            {
                if (QHarrash.IsReady() && GetBool("QH") && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "BlindMonkQOne")
                {
                    QHarrash.CastIfHitchanceEquals(target, HitchanceCheck(menu.Item("seth").GetValue<Slider>().Value)); // Continue like that
                }
                if (GetBool("QH") && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "blindmonkqtwo")
                {
                    QHarrash.Cast(target);
                }
                if (E.IsReady() && GetBool("EH") && E.IsInRange(target))
                {
                    E.Cast();
                    if (Items.CanUseItem(3077) && Player.Distance(target.Position) < 350)
                        Items.UseItem(3077);
                    if (Items.CanUseItem(3074) && Player.Distance(target.Position) < 350)
                        Items.UseItem(3074);
                    if (Items.CanUseItem(3142) && Player.Distance(target.Position) < 350)
                        Items.UseItem(3142);
                    HarrashComplete = true;
                }
                if (GetBool("WH") &&HarrashComplete)
                {
                    if (WardJump.Harrasjump(oldPosition))
                    {
                        HarrashComplete = false;
                        oldPositionbool = true;
                    }
                     
                }
            //    oldPositionbool = true;
              

            }
        }
        private static void JungleClear()
        {
            var minion = MinionManager.GetMinions(Q.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();
            var useQ = Program.menu.Item("QJ").GetValue<bool>();
            var useW = Program.menu.Item("WJ").GetValue<bool>();
            var useE = Program.menu.Item("EJ").GetValue<bool>();
            if (!minion.IsValidTarget() || minion == null)
            {
                if (menu.Item("laneclearkey").GetValue<KeyBind>().Active)
                    LaneClear();
            }
            if (Q.IsReady() && useQ)
            {
                if (minion.Distance(ObjectManager.Player.Position) <= Q.Range)
                {
                    Q.Cast(minion);
                    if (W.IsReady() && useW)
                    {
                        W.Cast(Player);
                    }
                }
            } 
            if (E.IsReady() && useE)
            {
                if (minion.Distance(ObjectManager.Player.Position) < E.Range)
                {
                    Player.IssueOrder(GameObjectOrder.AutoAttack, minion);
                    E.Cast();
                    if (Items.CanUseItem(3077) && Player.Distance(minion.Position) < 350)
                        Items.UseItem(3077);
                    if (Items.CanUseItem(3074) && Player.Distance(minion.Position) < 350)
                        Items.UseItem(3074);
                }
            }
        }
        private static void LaneClear()
        {
            var MinionN = MinionManager.GetMinions(Q.Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth).FirstOrDefault();
            var useQ = Program.menu.Item("QL").GetValue<bool>();
            var useW = Program.menu.Item("WL").GetValue<bool>();
            var useE = Program.menu.Item("EL").GetValue<bool>();
       
            if (Q.IsReady() && useQ)
            {
                if (MinionN.Distance(ObjectManager.Player.Position) <= Q.Range)
                {
                    Q.Cast(MinionN);
                    if (W.IsReady() && useW)
                    {
                        W.Cast(Player);
                    }
                }
            }
            if (E.IsReady() && useE)
            {
                if (MinionN.Distance(ObjectManager.Player.Position) < E.Range)
                {
                    E.Cast();
                    if (Items.CanUseItem(3077) && Player.Distance(MinionN.Position) < 350)
                        Items.UseItem(3077);
                    if (Items.CanUseItem(3074) && Player.Distance(MinionN.Position) < 350)
                        Items.UseItem(3074);
                }
            }


        }
        public static HitChance HitchanceCheck(int i)
        {
            switch (i)
            {
                case 1:
                  return  HitChance.Low;
                case 2:
                  return HitChance.Medium;
                case 3:
                 return HitChance.High;
                case 4:
                   return HitChance.VeryHigh;

            }
            return HitChance.Low;
        }
        private static void Combo()
        {
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            if (target != null)
            {
                if (Q.IsReady() && GetBool("comboQ") && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "BlindMonkQOne")
                {
                    Q.CastIfHitchanceEquals(target, HitchanceCheck(menu.Item("seth").GetValue<Slider>().Value)); // Continue like that
                }
                if ( GetBool("comboQ") && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "blindmonkqtwo" )
                {
                    Q.Cast();
                }
                //work 
                #region work
                if (E.IsReady() && GetBool("comboE") && E.IsInRange(target))
                {
                    E.Cast();
                    if (Items.CanUseItem(3077) && Player.Distance(target.Position) < 350)
                        Items.UseItem(3077);
                    if (Items.CanUseItem(3074) && Player.Distance(target.Position) < 350)
                        Items.UseItem(3074);
                    if (Items.CanUseItem(3142) && Player.Distance(target.Position) < 350)
                        Items.UseItem(3142);
                }
                if (W.IsReady() && GetBool("comboW"))
                {
                    if (ObjectManager.Player.HealthPercent <= menu.Item("Set W life %").GetValue<Slider>().Value)
                    {
                        W.Cast(Player);
                    }
                }
                //can kill
                if (E.IsReady() && E.IsKillable(target)) // si la e mata
                {
                    E.Cast(target);
                }
                else if (R.IsReady() && GetBool("comboR") && R.IsKillable(target)) // si solo la r mata
                {
                    R.Cast(target);
                }
                else if (Ignite.IsReady() && R.IsReady() && menu.Item("IgniteR").GetValue<bool>()) // si ignite R mata
                {
                    double DamageRIgnite = ObjectManager.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite) + Player.GetSpellDamage(target, SpellSlot.R);
                    if (target.Health - DamageRIgnite <= 0)
                    {
                        R.Cast(target);
                        ObjectManager.Player.Spellbook.CastSpell(Ignite, target);
                    }
                }
                //end can kill
                if (Ignite.IsReady()) // ignite cuando esta bajo
                {
                    if (target.Health - ObjectManager.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite) <= 0)
                        ObjectManager.Player.Spellbook.CastSpell(Ignite, target);
                }
                #endregion
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
                damage += Player.GetSpellDamage(target, SpellSlot.Q) * 2;
            damage += Player.GetAutoAttackDamage(target) * 4;
            if (E.IsReady())
                damage += Player.GetSpellDamage(target, SpellSlot.E);
            if (R.IsReady())
                damage += Player.GetSpellDamage(target, SpellSlot.R);
            if (Ignite.IsReady())
                damage += Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);

            return damage;
        }
        public static Vector3 InsecFinishPos(Obj_AI_Hero ts)
        {
            return Game.CursorPos.Extend(ts.Position, Game.CursorPos.Distance(ts.Position) - R.Range*2);
        }
        private static void Drawing_OnDraw(EventArgs args)
        {

            if (Player.IsDead) return;
            if (menu.Item("Draw Q Range").GetValue<bool>())
            {
                Drawing.DrawCircle(Player.Position, Q.Range, System.Drawing.Color.Green);
            }
            if (menu.Item("Draw W Range").GetValue<bool>())
            {
                Drawing.DrawCircle(Player.Position, 700f, System.Drawing.Color.Green);
            }
            if (menu.Item("Draw E Range").GetValue<bool>())
            {
                Drawing.DrawCircle(Player.Position, 430f, System.Drawing.Color.Green);
            }
            if (menu.Item("Draw R Range").GetValue<bool>())
            {
                Drawing.DrawCircle(Player.Position, 375f, System.Drawing.Color.Green);
            }
            //draw % de vida
            if (menu.Item("DrawKilleableText").GetValue<bool>())
            {
                foreach (Obj_AI_Hero target in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy).Where(x => !x.IsDead).Where(x => x.IsVisible).ToList())
                {
                    if (target.Health <= GetComboDamage(Player))
                    {
                        if (target.Health <= GetComboDamage(Player) - 300)
                        {
                            var wts = Drawing.WorldToScreen(target.Position);
                            Drawing.DrawText(wts[0] - 35, wts[1] + 10, System.Drawing.Color.Yellow, "Finish Him");
                        }
                        else
                        {
                            var wts = Drawing.WorldToScreen(target.Position);
                            Drawing.DrawText(wts[0] - 35, wts[1] + 10, System.Drawing.Color.Yellow, "Killeable");
                        }
                    }
                }
            }
            var targets = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            var wtse = Drawing.WorldToScreen(targets.Position);
        //    Drawing.DrawText(wtse[0] - 35, wtse[1], System.Drawing.Color.Yellow, steps);
                  if (menu.Item("inseckey").GetValue<KeyBind>().Active && menu.Item("DrawInsec").GetValue<bool>())
                  {
                        var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
                        var wtsx = Drawing.WorldToScreen(InsecFinishPos(target));
                        var wts = Drawing.WorldToScreen(target.Position);
                        var wtssx = Drawing.WorldToScreen(target.Position);           
                        Drawing.DrawLine(wts[0],wts[1],wtsx[0],wtsx[1],5f,System.Drawing.Color.Red);
                        Render.Circle.DrawCircle(Insecpos(target), 110, System.Drawing.Color.Blue, 5);
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
