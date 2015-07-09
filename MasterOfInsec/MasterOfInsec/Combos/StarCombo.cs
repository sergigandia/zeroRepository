using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
namespace MasterOfInsec.Combos
{
   static class StarCombo
    {
       public static string steps = "FDDFF";
       static bool star;

       public static void CastQ()
       {
           Program.Q.Cast();
           steps = "One";
       }
       private static void UseWardJump(Obj_AI_Base target)
       {
           var ws = Program.menu.Item("swardjump").GetValue<bool>();
           if(ws && Program.Player.Distance(target)>50)
           {
               WardJump.JumpTo(target.Position);
           }
       }
       public static void Combo()
       {
           Program.Player.IssueOrder(GameObjectOrder.MoveTo, Program.Player.Position.Extend(Game.CursorPos, 150));
           var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
           if (star == false)
           {
               if (Program.Q.IsReady() && Program.W.IsReady() && Program.R.IsReady() && Program.Player.Mana >= 150)
                  star = true;
           }
           if (star == false) return;
           if (steps == "Ward") //First hit q
           {
                   UseWardJump(target);
                   steps = "One";
           }
           else if (steps == "One") //First hit q
           {
               if (Program.E.IsInRange(target, Program.E.Range) && Program.E.CanCast(target))
               {

                   Program.E.Cast();
                   steps = "Two";
               }

           }
           else if (steps == "Two") // hit second q
           {
               if (Program.Q.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "BlindMonkQOne")
               {
                   Program.Q.CastIfHitchanceEquals(target, Combos.Combo.HitchanceCheck(Program.menu.Item("seth").GetValue<Slider>().Value)); // Continue like that
                   steps = "Three";
               }
           }
           else if (steps == "Three") // hit second q
           {
               //
               if (Program.R.CanCast(target))
               {
                   Program.R.CastOnUnit(target);
                   steps = "Four";
               }
           }
           else if (steps == "Four") // hit second q
           {
               if (Program.Q.CanCast(target))
               {

                   Utility.DelayAction.Add(1250, () => Program.Q.Cast());
                   if (Program.menu.Item("swardjump").GetValue<bool>())
                   {
                       steps = "WardJump";
                   }
                   else
                   {
                       steps = "One";
                   }
                   star = false;
               }
           }
           else
           {
               if (Program.menu.Item("swardjump").GetValue<bool>())
               {
                   steps = "WardJump";
               }
               else { 
               steps = "One";
            }
               star = false;
           }

       }
    }
}
