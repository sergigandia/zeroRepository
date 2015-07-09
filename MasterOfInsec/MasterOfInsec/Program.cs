using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace MasterOfInsec
{
    // LaneClear | jungleClear dont use skills
    // StarCombo Totally broken
    // insec didnt kick
    static class Program
    {
        // Master Of Insec LEE

  
        private static Obj_AI_Hero allytarget;
        public static Obj_AI_Hero Player;
        public static Menu menu;
        private static Orbwalking.Orbwalker orb;
        public static Spell Q, W, E, R,RInsec,QHarrash;
        public static SpellSlot Ignite;
        public static SpellSlot Smite;
        public static Orbwalking.Orbwalker Orbwalker { get; internal set; }
        public static Orbwalking.OrbwalkingMode OrbwalkerMode
        {
            get { return Orbwalker.ActiveMode; }
        }

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }
        public static bool Passive()
        {
            if (Player.HasBuff("blindmonkpassive_cosmetic"))
            {
                return true;
            }
            else
                return false;
        }


        static void Menu()
        {
            menu = new Menu("Master Of Insec", "MasterOfInsec", true);
            var orbWalkerMenu = new Menu("Orbwalker", "Orbwalker");
            orb = new Orbwalking.Orbwalker(orbWalkerMenu);
            var TargetSelectorMenu = new Menu("TargetSelector", "TargetSelector");
            var comboMenu = new Menu("Combo", "Combo");
            {
                comboMenu.AddItem(new MenuItem("cpassive", "Use Passive").SetValue(true));
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

            }

            var starcomboMenu = new Menu("StarCombo", "StarCombo");
            {
                starcomboMenu.AddItem(new MenuItem("Starcombokey", "StarCombo key").SetValue(new KeyBind('X', KeyBindType.Press)));
                starcomboMenu.AddItem(new MenuItem("swardjump", "Use wardjump").SetValue(true));
                starcomboMenu.AddItem(new MenuItem("starcombo", "StarCombo: E1 + Q1 + R + delay + Q2"));
            }
            var HarrashMenu = new Menu("Harrash", "Harrash");
            {
                HarrashMenu.AddItem(new MenuItem("hpassive", "Use Passive").SetValue(true));
                HarrashMenu.AddItem(new MenuItem("QH", "Use Q in Harrash").SetValue(true));
                HarrashMenu.AddItem(new MenuItem("WH", "Use W for go out").SetValue(false));
                HarrashMenu.AddItem(new MenuItem("EH", "Use E in Harrash").SetValue(true));
                HarrashMenu.AddItem(new MenuItem("Harrash key", "Harrash key").SetValue(new KeyBind('C', KeyBindType.Press)));
            }
            var InsecFlashSettingsMenu = new Menu("FlashInsec", "R < Flash Insec");
            {

                InsecFlashSettingsMenu.AddItem(new MenuItem("InstaFlashRkey", "InstaFlash R key").SetValue(new KeyBind('G', KeyBindType.Press)));
                InsecFlashSettingsMenu.AddItem(new MenuItem("useflash", "Use flash if not ward").SetValue(true));
            }
            var NormalInsecMenu = new Menu("NormalInsec", "Normal Insec");
            {
                NormalInsecMenu.AddItem(new MenuItem("Mode", "Mode").SetValue(new StringList(new[] { "Insec to Tower", "Insec to Ally", "Insec to Mouse" }, 1)));
                NormalInsecMenu.AddItem(new MenuItem("inseckey", "Insec key").SetValue(new KeyBind('T', KeyBindType.Press)));
                NormalInsecMenu.AddItem(new MenuItem("useflash", "Use flash if not ward").SetValue(true));
                NormalInsecMenu.AddItem(new MenuItem("OrbwalkInsec", "orbwalk on insec mode").SetValue(true));
            }
            var LaneclearMenu = new Menu("Laneclear", "Laneclear");
            {
                LaneclearMenu.AddItem(new MenuItem("lpassive", "Use Passive").SetValue(true));
                LaneclearMenu.AddItem(new MenuItem("QL", "Use Q in Laneclear").SetValue(true));
                LaneclearMenu.AddItem(new MenuItem("WL", "Use W in Laneclear").SetValue(false));
                LaneclearMenu.AddItem(new MenuItem("EL", "Use E in Laneclear").SetValue(true));
                LaneclearMenu.AddItem(new MenuItem("LMinE", "Min. minion e")).SetValue(new Slider(1, 1, 4));
                LaneclearMenu.AddItem(new MenuItem("laneclearkey", "LaneClear key").SetValue(new KeyBind('V', KeyBindType.Press)));
            }
            var LastHitMenu = new Menu("LastHit", "LastHit");
            {
                LastHitMenu.AddItem(new MenuItem("QL", "Use Q for LastHit").SetValue(true));
            }
            var JungleclearMenu = new Menu("Jungleclear", "Jungleclear");
            {
                JungleclearMenu.AddItem(new MenuItem("jpassive", "Use Passive").SetValue(true));
                JungleclearMenu.AddItem(new MenuItem("QJ", "Use Q in JungleClear").SetValue(true));
                JungleclearMenu.AddItem(new MenuItem("WJ", "Use W in JungleClear").SetValue(true));
                JungleclearMenu.AddItem(new MenuItem("EJ", "Use E in JungleClear").SetValue(true));
                JungleclearMenu.AddItem(new MenuItem("jungleclearkey", "JungleClear key").SetValue(new KeyBind('V', KeyBindType.Press)));
            }
            var ItemMenu = new Menu("Item Menu", "itemmenu");
            {
                ItemMenu.AddItem(new MenuItem("tiamat", "Use Tiamat").SetValue(true));
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
            var WardJumpMenu = new Menu("WardJump", "WardJump");
            {
                WardJumpMenu.AddItem(new MenuItem("wardjump", "WardJump key").SetValue(new KeyBind('Z', KeyBindType.Press)));
            }
            var InsecMenu = new Menu("Insecs", "Insecs");
            {
                InsecMenu.AddSubMenu(NormalInsecMenu);
                InsecMenu.AddSubMenu(InsecFlashSettingsMenu);
            }
            var FarmMenu = new Menu("Farm", "Farm");

            {
                FarmMenu.AddSubMenu(LastHitMenu);
                FarmMenu.AddSubMenu(LaneclearMenu);
                FarmMenu.AddSubMenu(JungleclearMenu);
            }
            var CombosMenu = new Menu("Combos", "Combos");
            {
                CombosMenu.AddSubMenu(comboMenu);
                CombosMenu.AddSubMenu(starcomboMenu);
                CombosMenu.AddSubMenu(HarrashMenu);
            }
            TargetSelector.AddToMenu(TargetSelectorMenu);
            menu.AddSubMenu(orbWalkerMenu);        //ORBWALKER
            menu.AddSubMenu(TargetSelectorMenu);   //TS
            menu.AddSubMenu(CombosMenu);//COMBO
            menu.AddSubMenu(FarmMenu);
            menu.AddSubMenu(InsecMenu);  //INSEC
            menu.AddSubMenu(ItemMenu);       //LANECLEAR
            menu.AddSubMenu(DrawSettingsMenu);     //DRAWS
            menu.AddSubMenu(WardJumpMenu);
            menu.AddToMainMenu();
        }
        static void OnGameLoad(EventArgs args)
        {

            Player = ObjectManager.Player;
            if (Player.ChampionName != "LeeSin") return;
            //   Ignite = new Spell(SpellSlot.Summoner1, 1100);
            Q = new Spell(SpellSlot.Q, 1100);
            QHarrash = new Spell(SpellSlot.Q,700);
            W = new Spell(SpellSlot.W, 700);
            E = new Spell(SpellSlot.E, 430);
            R = new Spell(SpellSlot.R, 375);
            //R.SetTargetted();
            RInsec = new Spell(SpellSlot.R, 375);
            Smite = ObjectManager.Player.GetSpellSlot("SummonerDot");
            Ignite = ObjectManager.Player.GetSpellSlot("summonersmite");
            QHarrash.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
            Q.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
            RInsec.SetTargetted(R.Instance.SData.CastFrame / 30, R.Instance.SData.MissileSpeed);
            Menu();
            Game.PrintChat("[LeeSin]Master Of Insec load good luck ;) ver 0.9.9.5");
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnWndProc += GameOnOnWndProc;
            Game.OnUpdate += Game_OnGameUpdate;

        }


        static int charges=0;
       static Obj_AI_Hero trys;
        private static void GameOnOnWndProc(WndEventArgs args)
        {
            if (args.Msg != (uint)WindowsMessages.WM_LBUTTONDOWN)
            {
                return;
            }
            trys = HeroManager.Allies
                     .FindAll(hero => hero.Distance(Game.CursorPos, true) < 40000) // 200 * 200
                     .OrderBy(h => h.Distance(Game.CursorPos, true)).FirstOrDefault();
                if(trys!=null)
                {
                   NormalInsec.insecAlly = HeroManager.Allies
                     .FindAll(hero => hero.Distance(Game.CursorPos, true) < 40000) // 200 * 200
                     .OrderBy(h => h.Distance(Game.CursorPos, true)).FirstOrDefault();
                }
                trys = HeroManager.Enemies
                    .FindAll(hero => hero.IsValidTarget() && hero.Distance(Game.CursorPos, true) < 40000) // 200 * 200
                    .OrderBy(h => h.Distance(Game.CursorPos, true)).FirstOrDefault();
                if (trys != null)
                {
                    NormalInsec.insecEnemy = HeroManager.Enemies
                      .FindAll(hero => hero.IsValidTarget() && hero.Distance(Game.CursorPos, true) < 40000) // 200 * 200
                      .OrderBy(h => h.Distance(Game.CursorPos, true)).FirstOrDefault();
                }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            Q.MinHitChance = MasterOfInsec.Combos.Combo.HitchanceCheck(menu.Item("seth").GetValue<Slider>().Value);
            if (Player.IsDead) return;
         //   KsIgnite();
            if(menu.Item("combokey").GetValue<KeyBind>().Active)
            {
                MasterOfInsec.Combos.Combo.Do();
            }
            if (menu.Item("wardjump").GetValue<KeyBind>().Active)
            {
                WardJump.Newjump();
            }
            if (menu.Item("jungleclearkey").GetValue<KeyBind>().Active)
            {
                MasterOfInsec.LaneClear.Do();
                MasterOfInsec.JungleClear.Do();
            }
            if (menu.Item("Harrash key").GetValue<KeyBind>().Active)
            {
                Combos.Harrash.Combo();
            }
            if (menu.Item("Starcombokey").GetValue<KeyBind>().Active)
            {
                Combos.StarCombo.Combo();
            }
            else
            {
                Combos.StarCombo.steps = "One";
            }
            if (menu.Item("InstaFlashRkey").GetValue<KeyBind>().Active)
            {
                MasterOfInsec.RFlashInsec.Combo(TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical));
            }
            if (menu.Item("inseckey").GetValue<KeyBind>().Active)
            {
                MasterOfInsec.NormalInsec.Combo();
            }
            else
            {
                MasterOfInsec.NormalInsec.ResetInsecStats();
               
            }
        }

        public static Obj_AI_Hero GetWInsecTarget()
        {
return  ObjectManager.Get<Obj_AI_Hero>()
                 .Where(x => x.IsEnemy)
                 .Where(x => !x.IsDead)
                 .Where(x => x.Distance(ObjectManager.Player.Position) <= W.Range)
                 .FirstOrDefault();
        }
        static string step = "WOne";
        static bool hit=false;
       
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
        private static void RenderCircles()
        {
           Render.Circle.DrawCircle( MasterOfInsec.NormalInsec.insecAlly.Position, 110, System.Drawing.Color.Blue, 5);
           Render.Circle.DrawCircle( MasterOfInsec.NormalInsec.insecEnemy.Position, 110, System.Drawing.Color.Red, 5);
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
            if (menu.Item("InstaFlashRkey").GetValue<KeyBind>().Active && menu.Item("DrawInsec").GetValue<bool>())
            {
                var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
                var wtsx = Drawing.WorldToScreen(InsecFinishPos(target));
                var wts = Drawing.WorldToScreen(target.Position);
                var wtssx = Drawing.WorldToScreen(target.Position);
                Drawing.DrawLine(wts[0], wts[1], wtsx[0], wtsx[1], 5f, System.Drawing.Color.Red);
                Render.Circle.DrawCircle(NormalInsec.Insecpos(target), 110, System.Drawing.Color.Blue, 5);
            }
                  if (menu.Item("inseckey").GetValue<KeyBind>().Active && menu.Item("DrawInsec").GetValue<bool>())
                  {
                        var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
                        if (Program.menu.Item("Mode").GetValue<StringList>().SelectedIndex == 0)
                        {
                            //insec to tower
                            if (WardJump.getNearTower(target) == null)
                            {
                            //    Drawing.DrawText(wtsp[0]-35, wtsp[1]+10, System.Drawing.Color.Yellow, "Not Enemy on range.");
                             Game.PrintChat("not enemy on range");
                            }
                            else
                            {
                                var wts = Drawing.WorldToScreen(target.Position);
                                var wtssxt = Drawing.WorldToScreen(WardJump.getNearTower(target).Position);
                                Drawing.DrawLine(wts[0], wts[1], wtssxt[0], wtssxt[1], 5f, System.Drawing.Color.Red);
                                Render.Circle.DrawCircle(WardJump.getNearTower(target).Position, 110, System.Drawing.Color.Green, 5);
                            //    Render.Circle.DrawCircle(WardJump.InsecposToAlly(target), 110, System.Drawing.Color.Green, 5);
                            }
                        }
                        if (Program.menu.Item("Mode").GetValue<StringList>().SelectedIndex == 1)
                        {
                            //insec to ally
                            RenderCircles();
                            var wts = Drawing.WorldToScreen(NormalInsec.insecAlly.Position);
                            var wtssxt = Drawing.WorldToScreen(NormalInsec.insecEnemy.Position);
                            Drawing.DrawLine(wtssxt[0], wtssxt[1], wts[0], wts[1], 5f, System.Drawing.Color.Red);
                            Render.Circle.DrawCircle(NormalInsec.insecAlly.Position, 110, System.Drawing.Color.Blue, 5);
                            Render.Circle.DrawCircle(WardJump.InsecposToAlly(NormalInsec.insecEnemy, NormalInsec.insecAlly), 110, System.Drawing.Color.Yellow, 5);
                        }
                        else if (Program.menu.Item("Mode").GetValue<StringList>().SelectedIndex == 2)
                        {
                            //insec to mouse
                            var wtsx = Drawing.WorldToScreen(InsecFinishPos(target));
                            var wts = Drawing.WorldToScreen(target.Position);
                            var wtssx = Drawing.WorldToScreen(target.Position);
                            Drawing.DrawLine(wts[0], wts[1], wtsx[0], wtsx[1], 5f, System.Drawing.Color.Red);
                            Render.Circle.DrawCircle(WardJump.Insecpos(target), 110, System.Drawing.Color.Blue, 5);
                        }
                  }
                  var wtsxx = Drawing.WorldToScreen(Player.Position);
          //        Drawing.DrawText(wtsxx[0] - 35, wtsxx[1] + 10, System.Drawing.Color.Yellow, "mode :" +);  
           //       Drawing.DrawText(wtsxx[0] - 35, wtsxx[1] + 10, System.Drawing.Color.Yellow, "step : " +passive);
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
