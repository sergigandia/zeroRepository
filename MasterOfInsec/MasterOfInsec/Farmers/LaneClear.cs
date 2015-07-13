using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace MasterOfInsec
{
   static class LaneClear
    {
        public static void  Do()
        {

            var MinionN = MinionManager.GetMinions(Program.Q.Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth).FirstOrDefault();
            var useQ = Program.menu.Item("QL").GetValue<bool>();
            var useW = Program.menu.Item("WL").GetValue<bool>();
            var useE = Program.menu.Item("EL").GetValue<bool>();

            if (Program.Q.IsReady() && useQ)
            {
                if (MinionN.Distance(ObjectManager.Player.Position) <= Program.Q.Range)
                {
                    MasterOfInsec.Data.castSpell(Program.Q, "lpassive", MinionN);

                }
            }
            if (Program.E.IsInRange(MinionN)&&Program.W.IsReady() && useW)
            {
                MasterOfInsec.Data.castSpell(Program.W, "lpassive", Program.Player);
            }
            var MinionNe = MinionManager.GetMinions(Program.E.Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth);
            if (Program.E.IsReady() && useE)
            {
                if (MinionNe.Count >= Program.menu.Item("LMinE").GetValue<Slider>().Value)
                {
                    MasterOfInsec.Data.castSpell(Program.E, "lpassive");
                    if (Items.CanUseItem(3077) && Program.Player.Distance(MinionN.Position) < 350)
                        Items.UseItem(3077);
                    if (Items.CanUseItem(3074) && Program.Player.Distance(MinionN.Position) < 350)
                        Items.UseItem(3074);
                }
                
            }


        
        }

    }
}
