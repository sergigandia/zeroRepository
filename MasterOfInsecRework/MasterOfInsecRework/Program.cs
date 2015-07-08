using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System.Drawing;

namespace MasterOfInsecRework
{
    class Program : Modes //, Polygon, Map
    {


        private Menu menu;
        private String name;
        private String version;
        private Obj_AI_Hero player;
        private Obj_AI_Hero allytarget;
        public  Map map;    
        private Orbwalking.Orbwalker orb;
        private int charges = 0;
        private Obj_AI_Hero trys;

        public Program()
        {
            menu = new Menu("Master Of Insec", "MasterOfInsec", true);
            name = "[Lee Sin]Master Of Insec";
            version = "2.0.0.0";
    
        }
        public Orbwalking.Orbwalker Orbwalker { get; internal set; }
        public Orbwalking.OrbwalkingMode OrbwalkerMode
        {
            get { return Orbwalker.ActiveMode; }
        }

        public Obj_AI_Hero getPlayer()
        {

            return player;
        }
        public Menu getMenu()
        {
            return menu;
        }
        public Map getMap()
        {
            return map;
        }
        public String getName()
        {
            return name;
        }
        public String getVersion()
        {
            return version;
        }

        public void loadMenu()
        {

            // menu = new Menu("Master Of Insec", "MasterOfInsec", true);
            var orbWalkerMenu = new Menu("Orbwalker", "Orbwalker");
            orb = new Orbwalking.Orbwalker(orbWalkerMenu);
            var TargetSelectorMenu = new Menu("TargetSelector", "TargetSelector");
            var QMenu = new Menu("QMenu", "Q Menu");
            {
                QMenu.AddItem(new MenuItem("seth", "Q Hitchance")).SetValue(new Slider(3, 1, 4));
                QMenu.AddItem(new MenuItem("comboQ", "Use Q in combo").SetValue(true));
                QMenu.AddItem(new MenuItem("RQ", "Priorize Q after R").SetValue(true));
            }
            var WMenu = new Menu("WMenu", "W Menu");
            {
                WMenu.AddItem(new MenuItem("comboW", "Use W in combo").SetValue(false));
                WMenu.AddItem(new MenuItem("Set W life %", "Set % W").SetValue(new Slider(20, 0, 100)));
            }
            var EMenu = new Menu("EMenu", "E Menu");
            {
                EMenu.AddItem(new MenuItem("comboE", "Use E in combo").SetValue(true));
            }
            var RMenu = new Menu("RMenu", "R Menu");
            {
                RMenu.AddItem(new MenuItem("comboR", "Use R to finish the enemy").SetValue(true));
            }
            var normalComboMenu = new Menu("NormalCombo", "Standard Combo");
            {
                normalComboMenu.AddSubMenu(QMenu);
                normalComboMenu.AddSubMenu(WMenu);
                normalComboMenu.AddSubMenu(EMenu);
            }
            var starComboMenu = new Menu("StarCombo", "Star Combo");
            {
                starComboMenu.AddItem(new MenuItem("Starcombokey", "StarCombo key").SetValue(new KeyBind('X', KeyBindType.Press)));
                starComboMenu.AddItem(new MenuItem("starcombo", "StarCombo: E1 + Q1 + R + delay + Q2"));
            }
             var customComboMenu = new Menu("CustomCombo", "Custom Combo");
            {
                customComboMenu.AddSubMenu(QMenu);
                customComboMenu.AddSubMenu(WMenu);
                customComboMenu.AddSubMenu(EMenu);
            }
            var comboMenu = new Menu("Combo", "Combo");
            {
                comboMenu.AddItem(new MenuItem("IgniteR", "Use R+Ignite for kill").SetValue(true));
                comboMenu.AddItem(new MenuItem("Ignite", "Use ignite for kill").SetValue(true));
                comboMenu.AddItem(new MenuItem("combokey", "Combo key").SetValue(new KeyBind(32, KeyBindType.Press)));
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
                InsecSettingsMenu.AddItem(new MenuItem("Mode", "Mode").SetValue(new StringList(new[] { "Insec to Tower", "Insec to Ally", "Insec to Mouse" }, 1)));
                InsecSettingsMenu.AddItem(new MenuItem("inseckey", "Insec key").SetValue(new KeyBind('T', KeyBindType.Press)));
                InsecSettingsMenu.AddItem(new MenuItem("useflash", "Use flash if not ward").SetValue(true));
                InsecSettingsMenu.AddItem(new MenuItem("OrbwalkInsec", "orbwalk on insec mode").SetValue(true));
                InsecSettingsMenu.AddItem(new MenuItem("InstaFlashRkey", "InstaFlash R key").SetValue(new KeyBind('G', KeyBindType.Press)));
                InsecSettingsMenu.AddItem(new MenuItem("useflash", "Use flash if not ward").SetValue(true));
                InsecSettingsMenu.AddItem(new MenuItem("infoRetarders", "How to use: hold the key until the insec finish!"));
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

        public void draw(EventArgs args)
        {
            
            if (player.IsDead) return;
            if (menu.Item("Draw Q Range").GetValue<bool>())
                Render.Circle.DrawCircle(getPlayer().Position, base.getSkills().getQ().Range, System.Drawing.Color.Purple, 2);
            if (menu.Item("Draw W Range").GetValue<bool>())
                Render.Circle.DrawCircle(getPlayer().Position, base.getSkills().getW().Range, System.Drawing.Color.Purple, 2);
            if (menu.Item("Draw E Range").GetValue<bool>())
                Render.Circle.DrawCircle(getPlayer().Position, base.getSkills().getE().Range, System.Drawing.Color.Purple, 2);
            if (menu.Item("Draw R Range").GetValue<bool>())
                Render.Circle.DrawCircle(getPlayer().Position, base.getSkills().getR().Range, System.Drawing.Color.Purple, 2);

        }

        public void load(EventArgs args)
        {
            
            player = ObjectManager.Player;
            // if (player.ChampionName != "Amumu") return;
            Utils.ShowNotification(getName() + " load good luck ;) " + getVersion(), System.Drawing.Color.White, 100);
            loadMenu();
            
            Drawing.OnDraw += draw;
            Game.OnUpdate += update;
            Game.OnWndProc += gameOnOnWndProc; // --> Seleccion de champs

            map = new Map();    
            
         
            
      //      Obj_AI_Base.OnProcessSpellCast += Oncast; --> PAra que es esto?
         //   Orbwalking.AfterAttack += afterAttack; --> para que es esto?
            
        }
        private void gameOnOnWndProc(WndEventArgs args)
        {
            if (args.Msg != (uint)WindowsMessages.WM_LBUTTONDOWN)
            {
                return;
            }
            trys = HeroManager.Allies
                     .FindAll(hero => hero.Distance(Game.CursorPos, true) < 40000) // 200 * 200
                     .OrderBy(h => h.Distance(Game.CursorPos, true)).FirstOrDefault();
            if (trys != null)
            {
                base.setInsecAlly(HeroManager.Allies
                 .FindAll(hero => hero.Distance(Game.CursorPos, true) < 40000) // 200 * 200
                 .OrderBy(h => h.Distance(Game.CursorPos, true)).FirstOrDefault());
            }
            trys = HeroManager.Enemies
                .FindAll(hero => hero.IsValidTarget() && hero.Distance(Game.CursorPos, true) < 40000) // 200 * 200
                .OrderBy(h => h.Distance(Game.CursorPos, true)).FirstOrDefault();
            if (trys != null)
            {
                base.setInsecEnemy(HeroManager.Enemies
                  .FindAll(hero => hero.IsValidTarget() && hero.Distance(Game.CursorPos, true) < 40000) // 200 * 200
                  .OrderBy(h => h.Distance(Game.CursorPos, true)).FirstOrDefault()); 
            }
        }
        public void update(EventArgs args)
        {
            if (getPlayer().IsDead) return;
            updateModes();
        }
        public void updateModes()
        {
            if (menu.Item("combokey").GetValue<KeyBind>().Active)         base.combo(base.getTarget());
            if (menu.Item("wardjump").GetValue<KeyBind>().Active)         base.jump();
            if (menu.Item("jungleclearkey").GetValue<KeyBind>().Active)   base.jungleClear();
            if (menu.Item("Harrash key").GetValue<KeyBind>().Active)      base.harrash(base.getTarget());
        //    if (menu.Item("Starcombokey").GetValue<KeyBind>().Active)     base.starCombo(base.getTarget());            
        //    else         steps = "One";            
            if (menu.Item("InstaFlashRkey").GetValue<KeyBind>().Active)  base.updateInsecFlash();            
            if (menu.Item("inseckey").GetValue<KeyBind>().Active)        base.updateInsec();          
            else
            {
               // MasterOfInsec.Insec.ResetInsecStats(); // --> Pône estado 1
                base.setOldPositionbool(true);
            }
        }
        static void Main(string[] args)
        {
            Program p = new Program();
            p.load(p);
           //pasar a piolygon p
            CustomEvents.Game.OnGameLoad += p.load;
        }


    }
}