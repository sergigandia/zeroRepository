using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace MasterOfInsec
{
    public static class Insec
    {
        public static int SecondQTime = new int();
        private static bool da;
        private static bool beforeall=true;
        public static string InsecMode = "Normal";
        public static string Steps = "One";
        public static Obj_AI_Hero insecAlly;
        public static Obj_AI_Hero insecEnemy;
        public static void updateInsec()
        {
            if(Program.menu.Item("OrbwalkInsec").GetValue<bool>())
            Program.Player.IssueOrder(GameObjectOrder.MoveTo, Program.Player.Position.Extend(Game.CursorPos, 150));
            if (!Program.R.IsReady()) return;
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            InsecQMode(target);
        }
        public static void updateInsecFlash()
        {
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            InsecFlashR(target);
        }
        public static string fiveornot()
        {
            return !Program.Player.IsDashing() ? Steps = "Five" : Steps;
        }

        public static void InsecQMode(Obj_AI_Hero target)
        {
            if(beforeall==false)
            {
                if (Program.Q.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "BlindMonkQOne")
                {
                    Steps = "One";
                }
                else if (Program.Q.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "blindmonkqtwo")
                {
                    Steps = "Two";
                }
                else
                {
                    Steps = "Three";
                }
                beforeall = true;
            }

            if (target.IsValidTarget(Program.Q.Range))
            {
                if (Steps == "One" ) //First hit q
                {
                 if(   Program.W.IsInRange(target.Position+50))
                 {
                     Steps = "Three";
                 }
                 else{
                    if (Program.Q.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "BlindMonkQOne")
                    {
                        if(Program.Q.CastIfHitchanceEquals(target, Program.HitchanceCheck(Program.menu.Item("seth").GetValue<Slider>().Value))) 
                        Steps = "Two";
                    }
                 }
                }
                else if (Steps == "Two") // hit second q
                {
                    if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "blindmonkqtwo")
                    {
                        SecondQTime = Convert.ToInt32(Math.Round(Game.Ping + Program.Q.Instance.SData.SpellTotalTime, MidpointRounding.AwayFromZero));
                        if (Program.Q.Cast())
                        {
                            if (!WardJump.getBestWardItem().IsValidSlot() && Program.menu.Item("useflash").GetValue<bool>())
                            {
                                
                                //Steps = "TrickR";
                                Steps = "Flash";
                            }
                            else
                            {
                                Steps = "Three";
                            }
                        }

                    }
                    else {
                     //   Steps = "One";
                    }
                }
                else if (Steps == "Three") // put ward
                {
                    if (Program.Player.Distance(WardJump.getward(target)) <= 600 && Program.W.IsReady())
                    {
                        Utility.DelayAction.Add(SecondQTime, () => WardJump.InsecJump(GetInsecPos(target).To2D()));
                        Steps = "Four";
                    }
                }
                else if (Steps == "Four")  //go to the ward
                {
                    Utility.DelayAction.Add(Game.Ping + 100, () => WardJump.InsecJump(GetInsecPos(target).To2D()));
                    Utility.DelayAction.Add(Game.Ping + 110, () => fiveornot());
                }
                else if (Steps == "Flash") // hit w
                {
                    if (WardJump.Insecpos(target).Distance(Program.Player.Position) < 400)
                    {
                        ObjectManager.Player.Spellbook.CastSpell(ObjectManager.Player.GetSpellSlot("SummonerFlash"), GetInsecPos(target));
                        Steps = "Five";
                    }
                }
                    else if(Steps == "TrickR") //truco de flash
                {
                    if (WardJump.Insecpos(target).Distance(Program.Player.Position) < 375)
                    {
                        if (Program.R.CastOnUnit(target))
                        {
                            Steps = "TrickFlash";
                        }
                    }
                }
                else if (Steps == "TrickFlash") //truco de flash
                {
                    if (WardJump.Insecpos(target).Distance(Program.Player.Position) < 375)
                    {
                        Utility.DelayAction.Add(Game.Ping + 125, () => ObjectManager.Player.Spellbook.CastSpell(ObjectManager.Player.GetSpellSlot("SummonerFlash"), GetInsecPos(target)));
                        Steps = "One";
                    }
                }
                else if (Steps == "Five" &&  !Program.W.Cast()) // and hit the kick
                {
                    Utility.DelayAction.Add(Convert.ToInt32(Math.Round(Game.Ping + Program.W.Instance.SData.SpellTotalTime, MidpointRounding.AwayFromZero)), () => Program.R.CastOnUnit(target));// it dont hit anything
                    Steps = "One";
                }
                else {
                  /*  if (Program.Q.IsReady())
                    {
                     //   Steps = "One";
                    }
                    else if(Program.W.IsReady())
                    {
                      //  Steps = "Three";
                    }
                    else
                    {*/
                        Steps = "One";
                 //   }
                }
            }
        }

        public static Vector3 GetInsecPos(Obj_AI_Hero target)
        {
            if (Program.menu.Item("Mode").GetValue<StringList>().SelectedIndex == 0)
            {
                return WardJump.InsecposTower(target); // insec torre
              //  Game.PrintChat("");
            }
            else if (Program.menu.Item("Mode").GetValue<StringList>().SelectedIndex == 1)
            {
                return WardJump.InsecposToAlly(insecEnemy,insecAlly); //insec ally  
            }
            else if (Program.menu.Item("Mode").GetValue<StringList>().SelectedIndex == 2)
            {
                return WardJump.Insecpos(target); // insec normal
            }

            return WardJump.Insecpos(target);
        }
        public static void InsecFlashR(Obj_AI_Hero target)
        {
           Program.Player.IssueOrder(GameObjectOrder.MoveTo, Program.Player.Position.Extend(Game.CursorPos, 150));
            if(MasterOfInsec.Program.R.IsReady())
            if (WardJump.Insecpos(target).Distance(Program.Player.Position) < 375)
            {
                Program.R.CastOnUnit(target);
                Utility.DelayAction.Add(Game.Ping + 125, () => ObjectManager.Player.Spellbook.CastSpell(ObjectManager.Player.GetSpellSlot("SummonerFlash"), WardJump.Insecpos(target)));
            }

        }
        public static void ResetInsecStats()
        {
       //     beforeall = false;
            Steps = "One";
        }
    }
}