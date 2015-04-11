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
        private static bool da;
        public static string InsecMode = "Normal";
        public static string Steps = "One";
        public static void updateInsec()
        {
            if (!Program.R.IsReady()) return;
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
                InsecQMode(target);


        }
        public static void InsecQMode(Obj_AI_Hero target)
        {
            if (target != null)
            {
                if (Steps == "One") //First hit q
                {
                    if (Program.Q.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "BlindMonkQOne")
                    {
                        Program.Q.CastIfHitchanceEquals(target, Program.HitchanceCheck(Program.menu.Item("seth").GetValue<Slider>().Value)); // Continue like that
                        Steps = "Two";
                    }
                }
                else if (Steps == "Two") // hit second q
                {
                    if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "blindmonkqtwo")
                    {
                        if (Program.Q.Cast())
                        {
                            if (WardJump.getBestWardItem() == null && Program.menu.Item("useflash").GetValue<bool>())               Steps = "Flash";
                            else                                                  Steps = "Three";
                               
                        }

                    }
                    else
                    {
                    }
                }
                else if (Steps == "Three") // hit w
                {
                    if (Program.Player.Distance(WardJump.getward(target)) <= 600 && Program.W.IsReady())
                    {
                        WardJump.InsecJump(WardJump.Insecpos(target).To2D());
                        Steps = "Four";
                    }
                }
                else if (Steps == "Four")  //go to the ward
                {
                    WardJump.InsecJump(WardJump.Insecpos(target).To2D());
                    if(!Program.Player.IsDashing())
                    Steps = "Five";
                }
                else if (Steps == "Flash") // hit w
                {
                  // Game.PrintChat("Flashing!");
                    if (WardJump.Insecpos(target).Distance(Program.Player.Position) < 400)
                    {
                        ObjectManager.Player.Spellbook.CastSpell(ObjectManager.Player.GetSpellSlot("SummonerFlash"), WardJump.Insecpos(target));
                        Steps = "Five";
                    }


                }
                else if (Steps == "Five") // and hit the kcik
                {
                        Program.R.CastOnUnit(target);// it dont hit anything

                }
                else
                {
                    Steps = "One";
                }
            }
        }
        public static Vector3  GetInsecPos(Obj_AI_Hero target)
        {
          if( Program.menu.Item("Mode").GetValue<StringList>().SelectedIndex==0)
          {
return WardJump.Insecpos(target);
          }
          else if( Program.menu.Item("Mode").GetValue<StringList>().SelectedIndex==1)
            {
                return WardJump.InsecposTower(target);
            }
          else if ( Program.menu.Item("Mode").GetValue<StringList>().SelectedIndex==2)
          {
              return WardJump.InsecposToAlly(target);
          }
              return WardJump.Insecpos(target);
        }
        public static void ResetInsecStats()
        {
            Steps = "One";
        }
    }
}

