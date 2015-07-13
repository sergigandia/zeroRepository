using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;


namespace MasterOfInsec
{
    static class JungleClear
    {
      public static void Do()
        {
            var minion = MinionManager.GetMinions(Program.Q.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();
            var useQ = Program.menu.Item("QJ").GetValue<bool>();
            var useW = Program.menu.Item("WJ").GetValue<bool>();
            var useE = Program.menu.Item("EJ").GetValue<bool>();
            var Passive = Program.menu.Item("jpassive").GetValue<bool>();
            if (minion != null)
            {
                if (useW&&Program.W.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() == "blindmonkwtwo")
                {
                    MasterOfInsec.Data.castSpell(Program.W, "jpassive");
                }
                else if (useQ&&Program.Q.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name.ToLower() == "blindmonkqtwo")
                {
                    MasterOfInsec.Data.castSpell(Program.Q, "jpassive");
                }
                else if (useE && Program.E.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Name.ToLower() == "blindmonketwo")
                {
                    MasterOfInsec.Data.castSpell(Program.E, "jpassive");
                }
                else if (useW && Program.W.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name.ToLower() == "blindmonkwone")
                {
                    MasterOfInsec.Data.castSpell(Program.W, "jpassive", Program.Player);
                }
                else if (useQ && Program.Q.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name.ToLower() == "blindmonkqone")
                {
                    MasterOfInsec.Data.castSpell(Program.Q, "jpassive", minion);
                }
                else if (useE && Program.E.IsReady() && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Name.ToLower() == "blindmonkeone")
                {
                    MasterOfInsec.Data.castSpell(Program.E, "jpassive");
                }
            }
        }

    }
}
