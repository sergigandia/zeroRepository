using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace MasterOfJumps
{
    // Master of Jumps
    enum ChampName
    {
        LeeSin = 0,
        Jax = 1,
        Katarina = 2
    }
    class Program
    {
        //KATARINA LEE JAX 
        public static Menu menu;
        public static ChampName name;
        public static Spell QJax, WLee, EKata;
        public static Obj_AI_Hero Player;
        public static Vector3 wardPosition;
        public static bool jumped;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad(EventArgs args)
        {
             Player = ObjectManager.Player;
             if (Player.ChampionName == "LeeSin")
            {
                WLee = new Spell(SpellSlot.W, 700);
                name = ChampName.LeeSin;
            }

            else if (Player.ChampionName == "Jax")
            {
                QJax = new Spell(SpellSlot.Q, 700);
                name = ChampName.Jax;
            }
            else if (Player.ChampionName == "Katarina")
            {
                EKata = new Spell(SpellSlot.E, 700);
                name = ChampName.Katarina;
            }
            Menu();
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (menu.Item("Drawings").IsActive())
            {
                if (ChampName.Jax == name)
                {
                    Drawing.DrawCircle(Player.Position, QJax.Range, System.Drawing.Color.Green);
                }
                else if (ChampName.LeeSin == name)
                {
                    Drawing.DrawCircle(Player.Position, WLee.Range, System.Drawing.Color.Green);
                }
                else if (ChampName.Katarina == name)
                {
                    Drawing.DrawCircle(Player.Position, EKata.Range, System.Drawing.Color.Green);
                }
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (menu.Item("wardjump").GetValue<KeyBind>().Active)
            {
                if (ChampName.Jax == name)
                {
                    WardJumpJax();
                }
                else if (ChampName.LeeSin == name)
                {
                    WardJumpLee();
                }
                else if (ChampName.Katarina == name)
                {
                    WardJumpKata();
                }
            }
        }

        public static void Menu()
        {
            menu = new Menu("Master Of Jump", "MasterOfJump", true);
            menu.AddItem(new MenuItem("Drawings", "draws").SetValue(true));
            menu.AddItem(new MenuItem("wardjump", "WardJump key").SetValue(new KeyBind('Z', KeyBindType.Press)));
            menu.AddToMainMenu();
        }
        public static void WardJumpJax()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, 150));

            if (QJax.IsReady())
            {
                wardPosition = Game.CursorPos;
                Obj_AI_Minion Wards;
                if (Game.CursorPos.Distance(Program.Player.Position) <= 700)
                {
                    Wards = ObjectManager.Get<Obj_AI_Minion>().Where(ward => ward.Distance(Game.CursorPos) < 150 && !ward.IsDead).FirstOrDefault();
                }
                else
                {
                    Vector3 cursorPos = Game.CursorPos;
                    Vector3 myPos = Player.ServerPosition;
                    Vector3 delta = cursorPos - myPos;
                    delta.Normalize();
                    wardPosition = myPos + delta * (600 - 5);
                    Wards = ObjectManager.Get<Obj_AI_Minion>().Where(ward => ward.Distance(wardPosition) < 150 && !ward.IsDead).FirstOrDefault();
                }
                if (Wards == null)
                {
                    if (!wardPosition.IsWall())
                    {
                        InventorySlot invSlot = Items.GetWardSlot();
                        Items.UseItem((int)invSlot.Id, wardPosition);
                        jumped = true;
                    }
                }

                else
                    if (QJax.CastOnUnit(Wards))
                    {
                        jumped = false;
                    }
            }
        }
        public static void WardJumpLee()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, 150));

            if (WLee.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "BlindMonkWOne")
            {
                wardPosition = Game.CursorPos;
                Obj_AI_Minion Wards;
                if (Game.CursorPos.Distance(Program.Player.Position) <= 700)
                {
                    Wards = ObjectManager.Get<Obj_AI_Minion>().Where(ward => ward.Distance(Game.CursorPos) < 150 && !ward.IsDead).FirstOrDefault();
                }
                else
                {
                    Vector3 cursorPos = Game.CursorPos;
                    Vector3 myPos = Player.ServerPosition;
                    Vector3 delta = cursorPos - myPos;
                    delta.Normalize();
                    wardPosition = myPos + delta * (600 - 5);
                    Wards = ObjectManager.Get<Obj_AI_Minion>().Where(ward => ward.Distance(wardPosition) < 150 && !ward.IsDead).FirstOrDefault();
                }
                if (Wards == null)
                {
                    if (!wardPosition.IsWall())
                    {
                        InventorySlot invSlot = Items.GetWardSlot();
                        Items.UseItem((int)invSlot.Id, wardPosition);
                        jumped = true;
                    }
                }

                else
                    if (Program.WLee.CastOnUnit(Wards))
                    {
                        jumped = false;
                    }
            }

        }
        public static void WardJumpKata()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, 150));
            if (EKata.IsReady())
            {
                wardPosition = Game.CursorPos;
                Obj_AI_Minion Wards;
                if (Game.CursorPos.Distance(Program.Player.Position) <= 700)
                {
                    Wards = ObjectManager.Get<Obj_AI_Minion>().Where(ward => ward.Distance(Game.CursorPos) < 150 && !ward.IsDead).FirstOrDefault();
                }
                else
                {
                    Vector3 cursorPos = Game.CursorPos;
                    Vector3 myPos = Player.ServerPosition;
                    Vector3 delta = cursorPos - myPos;
                    delta.Normalize();
                    wardPosition = myPos + delta * (600 - 5);
                    Wards = ObjectManager.Get<Obj_AI_Minion>().Where(ward => ward.Distance(wardPosition) < 150 && !ward.IsDead).FirstOrDefault();
                }
                if (Wards == null)
                {
                    if (!wardPosition.IsWall())
                    {
                        InventorySlot invSlot = Items.GetWardSlot();
                        Items.UseItem((int)invSlot.Id, wardPosition);
                        jumped = true;
                    }
                }

                else
                    if (EKata.CastOnUnit(Wards))
                    {
                        jumped = false;
                    }
            }
        }
    }
}
