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
   static class WardJump
    {
       public static Vector3 posforward;
       public static float LastPlaced;
       private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
       public static InventorySlot getBestWardItem()
       {
           InventorySlot ward = Items.GetWardSlot();
           if (ward == default(InventorySlot)) return null;
           return ward;
       }


       public static bool jump()
       {
           #region ward ya existe
           if (Program.W.IsReady())
           {
               foreach (Obj_AI_Minion ward in ObjectManager.Get<Obj_AI_Minion>().Where(ward =>
                    ward.Name.ToLower().Contains("ward") && ward.Distance(Game.CursorPos) < 250))
               {
                   if (ward != null)
                   {
                       Program.W.CastOnUnit(ward);
                       Program.W.Cast();
                       return true;

                   }
               
               }

               foreach (
                   Obj_AI_Hero hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.Distance(Game.CursorPos) < 250 && !hero.IsDead))
               {
                   if (hero != null)
                   {
                       Program.W.CastOnUnit(hero);
                       Program.W.Cast();
                       return true;

                   }
             
               }

               foreach (Obj_AI_Minion minion in ObjectManager.Get<Obj_AI_Minion>().Where(minion =>
                   minion.Distance(Game.CursorPos) < 250))
               {
                   if (minion != null)
                   {
                       Program.W.CastOnUnit(minion);
                       Program.W.Cast();
                       return true;
                   }

               }
           }
           #endregion
           if (Program.W.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "BlindMonkWOne")
           {
               if (Environment.TickCount <= LastPlaced + 3000) return false;
               Vector3 cursorPos = Game.CursorPos;
               Vector3 myPos = Player.ServerPosition;

               Vector3 delta = cursorPos - myPos;
               delta.Normalize();

               Vector3 wardPosition = myPos + delta * (600 - 5);
               InventorySlot invSlot = getBestWardItem();
               Items.UseItem((int)invSlot.Id, wardPosition);
               LastPlaced = Environment.TickCount;
               return true;
           }
           return false;
       }

    }
}
