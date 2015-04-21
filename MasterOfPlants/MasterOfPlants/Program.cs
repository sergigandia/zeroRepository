using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System.Drawing;
namespace MasterOfPlants
{
    class Program : Modes
    {
        private Menu menu;
        private String name;
        private String version;
        private Obj_AI_Hero player;
       
        public Program()
        {
            menu = new Menu("Master Of Plants","MasterOfPlants",true);
            name = "[Zyra]Master Of Plants";
            version = "0.1.0.0";
        }

        public Obj_AI_Hero getPlayer()
        {
            return player;
        }
        public Menu getMenu()
        {
            return menu;
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
            var orbWalkerMenu = new Menu("Orbwalker", "Orbwalker");
            Orbwalking.Orbwalker orb = new Orbwalking.Orbwalker(orbWalkerMenu);
            var TargetSelectorMenu = new Menu("TargetSelector", "TargetSelector");
            var comboMenu = new Menu("Combo", "Combo");
            {
                comboMenu.AddItem(new MenuItem("seth", "Q Hitchance")).SetValue(new Slider(3, 1, 4));
                comboMenu.AddItem(new MenuItem("comboQ", "Use Q in combo").SetValue(true));
                comboMenu.AddItem(new MenuItem("comboW", "Use W in combo").SetValue(false));
                comboMenu.AddItem(new MenuItem("comboE", "Use E in combo").SetValue(true));
                comboMenu.AddItem(new MenuItem("comboR", "Use R to finish the enemy").SetValue(true));
                comboMenu.AddItem(new MenuItem("Ignite", "Use ignite for kill").SetValue(true));
                comboMenu.AddItem(new MenuItem("combokey", "Combo key").SetValue(new KeyBind(32, KeyBindType.Press)));
            }
            var comboRMenu = new Menu("RCombokey", "RCombo"); // R+E+W+Q+W
            {
                comboRMenu.AddItem(new MenuItem("QH", "Use Q in Harrash").SetValue(true));
                comboRMenu.AddItem(new MenuItem("WH", "Use W for go out").SetValue(false));
                comboRMenu.AddItem(new MenuItem("rcombokey", "RCombo key").SetValue(new KeyBind('X', KeyBindType.Press)));
            }
            var HarrashMenu = new Menu("Harrash", "Harrash");
            {
                HarrashMenu.AddItem(new MenuItem("QH", "Use Q in Harrash").SetValue(true));
                HarrashMenu.AddItem(new MenuItem("WH", "Use W for go out").SetValue(false));
                HarrashMenu.AddItem(new MenuItem("Harrash key", "Harrash key").SetValue(new KeyBind('C', KeyBindType.Press)));
            }
            var UltimateSettingsMenu = new Menu("Ultimate Settings", "Ultimate Settings");
            {
                UltimateSettingsMenu.AddItem(new MenuItem("seth", "Q Hitchance")).SetValue(new Slider(1, 1, 5));
                UltimateSettingsMenu.AddItem(new MenuItem("Ultimate Key", "Ultimate key").SetValue(new KeyBind('T', KeyBindType.Press)));
            //    UltimateSettingsMenu.AddItem(new MenuItem("useflash", "Use flash").SetValue(true));
            }
            var FleeMenu = new Menu("Flee", "Flee");
            {
                UltimateSettingsMenu.AddItem(new MenuItem("fleekey", "Flee key").SetValue(new KeyBind('Z', KeyBindType.Press)));
                UltimateSettingsMenu.AddItem(new MenuItem("flee", "Only use e for flee"));
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
                ItemMenu.AddItem(new MenuItem("Zhonias", "zhonias").SetValue(true));
                ItemMenu.AddItem(new MenuItem("xxxx", "xxxx").SetValue(true));
                ItemMenu.AddItem(new MenuItem("xxxx", "xxxx").SetValue(true));
            }
            var DrawSettingsMenu = new Menu("Draw Settings", "Draw Settings");
            {
                DrawSettingsMenu.AddItem(new MenuItem("DrawUltimate", "DrawUltimate").SetValue(true));
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
            menu.AddSubMenu(comboRMenu);
            menu.AddSubMenu(HarrashMenu);  //Harrash
            menu.AddSubMenu(UltimateSettingsMenu);  //Ultimate
            menu.AddSubMenu(ItemMenu);
            menu.AddSubMenu(LaneclearMenu);        //LANECLEAR
            menu.AddSubMenu(JungleclearMenu);      //JUNGLECLEAR
            menu.AddSubMenu(DrawSettingsMenu);     //DRAWS
            menu.AddToMainMenu();
        }

        public void draw(EventArgs args)
        {
            if (player.IsDead) return;
            if (menu.Item("Draw Q Range").GetValue<bool>())
                Render.Circle.DrawCircle(getPlayer().Position,800f, System.Drawing.Color.Blue, 2);
            if (menu.Item("Draw W Range").GetValue<bool>())
                Render.Circle.DrawCircle(getPlayer().Position,850f, System.Drawing.Color.Blue, 2);
            if (menu.Item("Draw E Range").GetValue<bool>())
                Render.Circle.DrawCircle(getPlayer().Position,1100f, System.Drawing.Color.Blue, 2);
            if (menu.Item("Draw R Range").GetValue<bool>())
                Render.Circle.DrawCircle(getPlayer().Position,700f, System.Drawing.Color.Blue,2);           
        }
        
        public void load(EventArgs args)
        {
            player = ObjectManager.Player;
            Game.PrintChat(getName()+" load good luck ;) " + getVersion());
            loadMenu();
            Drawing.OnDraw += draw;
            Game.OnUpdate += update;
        }

        public void update(EventArgs args)
        {
            if (getPlayer().IsDead) return;
            updateModes();
        }

        public void updateModes()
        {
            if (getMenu().Item("combokey").GetValue<KeyBind>().Active)  base.combo(base.getTarget());
            if (getMenu().Item("rcombokey").GetValue<KeyBind>().Active) base.rCombo(base.getTarget());
            if (getMenu().Item("fleekey").GetValue<KeyBind>().Active) base.Flee(base.getTarget());
            if (getMenu().Item("Ultimate Key").GetValue<KeyBind>().Active) base.OnlyR(base.getTarget());
            if (getMenu().Item("jungleclearkey").GetValue<KeyBind>().Active) base.jungleClear();
            if (getMenu().Item("Harrash key").GetValue<KeyBind>().Active) base.harrash(base.getTarget());                   
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.load(p); // que el program es heredado
            CustomEvents.Game.OnGameLoad += p.load;
        }


    }
}
