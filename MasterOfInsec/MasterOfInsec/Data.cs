using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
namespace MasterOfInsec
{
    static class Data
    {
        public static void castSpell(Spell p, String menu)
        {
            var Passive = Program.menu.Item(menu).GetValue<bool>();
            if (Passive == false)
            {

                p.Cast();
            }
            else
            {
                if (!Program.Passive())
                {
                    p.Cast();
                }
            }
        }
        public static void castSpell(Spell p, String menu, Obj_AI_Base target)
        {
            var Passive = Program.menu.Item(menu).GetValue<bool>();
            if (Passive == false)
            {
                p.Cast(target);
            }
            else
            {
                if (!Program.Passive())
                {
                    p.Cast(target);
                }
            }
        }
    }
}
