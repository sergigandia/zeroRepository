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
            menu = new Menu("Master Of Rogues","MasterOfRogues",true);
            name = "[Ryze]Master Of Rogues";
            version = "0.0.0.1";
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
                comboMenu.AddItem(new MenuItem("setcharges", "Set R charges")).SetValue(new Slider(4, 0, 5));
                comboMenu.AddItem(new MenuItem("QC", "Use Q in combo").SetValue(true));
                comboMenu.AddItem(new MenuItem("WC", "Use W in combo").SetValue(false));
                comboMenu.AddItem(new MenuItem("EC", "Use E in combo").SetValue(true));
                comboMenu.AddItem(new MenuItem("RC", "Use R in combo").SetValue(true));

                comboMenu.AddItem(new MenuItem("Ignite", "Use ignite for kill").SetValue(true));
                comboMenu.AddItem(new MenuItem("combokey", "Combo key").SetValue(new KeyBind(32, KeyBindType.Press)));
            }
            var HarrashMenu = new Menu("Harrash", "Harrash");
            {
                HarrashMenu.AddItem(new MenuItem("QH", "Use Q in Harrash").SetValue(true));
                HarrashMenu.AddItem(new MenuItem("WH", "Use W in Harrash").SetValue(true));
                HarrashMenu.AddItem(new MenuItem("EH", "Use E in Harrash").SetValue(true));
                HarrashMenu.AddItem(new MenuItem("Harrash key", "Harrash key").SetValue(new KeyBind('C', KeyBindType.Press)));
            }
            var LaneclearMenu = new Menu("Laneclear", "Laneclear");
            {
                LaneclearMenu.AddItem(new MenuItem("QL", "Use Q in Laneclear").SetValue(true));
                LaneclearMenu.AddItem(new MenuItem("WL", "Use W in Laneclear").SetValue(false));
                LaneclearMenu.AddItem(new MenuItem("EL", "Use E in Laneclear").SetValue(true));
                LaneclearMenu.AddItem(new MenuItem("RL", "Use R in Laneclear").SetValue(true));
                LaneclearMenu.AddItem(new MenuItem("laneclearkey", "LaneClear key").SetValue(new KeyBind('V', KeyBindType.Press)));
            }
            var JungleclearMenu = new Menu("Jungleclear", "Jungleclear");
            {
                JungleclearMenu.AddItem(new MenuItem("QJ", "Use Q in JungleClear").SetValue(true));
                JungleclearMenu.AddItem(new MenuItem("WJ", "Use W in JungleClear").SetValue(false));
                JungleclearMenu.AddItem(new MenuItem("EJ", "Use E in JungleClear").SetValue(true));
                JungleclearMenu.AddItem(new MenuItem("RJ", "Use R in JungleClear").SetValue(true));
                JungleclearMenu.AddItem(new MenuItem("jungleclearkey", "JungleClear key").SetValue(new KeyBind('V', KeyBindType.Press)));
            }
          /*  var ItemMenu = new Menu("Item Menu", "itemmenu");
            {
                ItemMenu.AddItem(new MenuItem("Zhonias", "zhonias").SetValue(true));
            //    ItemMenu.AddItem(new MenuItem("xxxx", "xxxx").SetValue(true));
            //    ItemMenu.AddItem(new MenuItem("xxxx", "xxxx").SetValue(true));
            }*/
            var DrawSettingsMenu = new Menu("Draw Settings", "Draw Settings");
            {
                DrawSettingsMenu.AddItem(new MenuItem("DrawUltimate", "DrawUltimate").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("DrawKilleableText", "Draw Killeable Text").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("Draw Q Range", "Draw Q Range").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("Draw W Range", "Draw W Range").SetValue(true));
                DrawSettingsMenu.AddItem(new MenuItem("Draw E Range", "Draw E Range").SetValue(true));
            }

            TargetSelector.AddToMenu(TargetSelectorMenu);
            menu.AddSubMenu(orbWalkerMenu);        //ORBWALKER
            menu.AddSubMenu(TargetSelectorMenu);   //TS
            menu.AddSubMenu(comboMenu);//COMBO
            menu.AddSubMenu(HarrashMenu);  //Harrash
          //  menu.AddSubMenu(ItemMenu);
            menu.AddSubMenu(LaneclearMenu);        //LANECLEAR
            menu.AddSubMenu(JungleclearMenu);      //JUNGLECLEAR
            menu.AddSubMenu(DrawSettingsMenu);     //DRAWS
            menu.AddToMainMenu();
        }

        public void draw(EventArgs args)
        {
            if (player.IsDead) return;
            if (menu.Item("Draw Q Range").GetValue<bool>())
                Render.Circle.DrawCircle(getPlayer().Position,900f, System.Drawing.Color.Blue, 2);
            if (menu.Item("Draw W Range").GetValue<bool>())
                Render.Circle.DrawCircle(getPlayer().Position,600f, System.Drawing.Color.Blue, 2);
            if (menu.Item("Draw E Range").GetValue<bool>())
                Render.Circle.DrawCircle(getPlayer().Position,600f, System.Drawing.Color.Blue, 2);     
        }
        
        public void load(EventArgs args)
        {
            Game.PrintChat("Estoy en el load");
            player = ObjectManager.Player;
            Utils.ShowNotification(getName() + " load good luck ;) " + getVersion(), System.Drawing.Color.White, 100);
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
            if (getMenu().Item("combokey").GetValue<KeyBind>().Active) base.combo(base.getTarget());
            if (getMenu().Item("jungleclearkey").GetValue<KeyBind>().Active) base.jungleClear();
            if (getMenu().Item("laneclearkey").GetValue<KeyBind>().Active) base.laneClear();
            if (getMenu().Item("Harrash key").GetValue<KeyBind>().Active) base.harrash(base.getTarget());                   
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.load(p);
            CustomEvents.Game.OnGameLoad += p.load;
        }


    }
}
