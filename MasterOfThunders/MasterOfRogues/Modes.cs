using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
namespace MasterOfPlants
{
    class Modes
    {
        private Obj_AI_Hero target;
       private Program p;
      private  Skills skills;
        public void load(Program p)
        {
            skills = new Skills();
            this.p = p;
            target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Magical);
        }
        public Obj_AI_Hero getTarget()
        {
            target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Magical);
            return target;
        }
        //combos
        public int getPassiveBuff
        {
            get
            {
                var data = ObjectManager.Player.Buffs.FirstOrDefault(b => b.DisplayName == "RyzePassiveStack");
                return data != null ? data.Count : 0;
            }
        }
        public void laneClear()
        {
            var minion = MinionManager.GetMinions(skills.getQ().Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth).FirstOrDefault();
            var useQ = p.getMenu().Item("QL").GetValue<bool>();
            var useW = p.getMenu().Item("WL").GetValue<bool>();
            var useE = p.getMenu().Item("EL").GetValue<bool>();
            var useR= p.getMenu().Item("RL").GetValue<bool>();

            if (useQ && !useW && !useE) skills.qCast(minion);
            else if (!useQ && useW && !useE) skills.wCast(minion);
            else if (!useQ && !useW && useE) skills.eCast(minion);
            else if (!useQ && useW && useE)  
            {
                skills.wCast(minion);
                skills.eCast(minion);
            }
            
            else if (useQ && !useW && useE)
            {
                skills.qCast(minion);
                skills.eCast(minion);
            }
            else if (useQ && useW && !useE)
            {
                skills.qCast(minion);
                skills.wCast(minion);
            }
            else if (useQ && useW && useE)
            {
                skills.qCast(minion);
                skills.wCast(minion);
                skills.eCast(minion);

            }
            else
                return;
              if(useR)
            {
                if (p.getMenu().Item("setcharges").GetValue<Slider>().Value == getPassiveBuff)
                {
           //         Utils.ShowNotification(getPassiveBuff.ToString() ,System.Drawing.Color.Blue,100,true);
                    skills.rCast(minion);
                }
            }
        }
        public void jungleClear()
        {
            var minion = MinionManager.GetMinions(skills.getQ().Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();
            var useQ = p.getMenu().Item("QJ").GetValue<bool>();
            var useW = p.getMenu().Item("WJ").GetValue<bool>();
            var useE = p.getMenu().Item("EJ").GetValue<bool>();
            var useR = p.getMenu().Item("RJ").GetValue<bool>();
            if (useQ && !useW && !useE) skills.qCast(minion);
            else if (!useQ && useW && !useE) skills.wCast(minion);
            else if (!useQ && !useW && useE) skills.eCast(minion);
            else if (!useQ && useW && useE)
            {
                skills.wCast(minion);
                skills.eCast(minion);
            }

            else if (useQ && !useW && useE)
            {
                skills.qCast(minion);
                skills.eCast(minion);
            }
            else if (useQ && useW && !useE)
            {
                skills.qCast(minion);
                skills.wCast(minion);
            }
            else if (useQ && useW && useE)
            {
                skills.qCast(minion);
                skills.wCast(minion);
                skills.eCast(minion);
            }
            else
                return;
            if (useR)
            {
                if (p.getMenu().Item("setcharges").GetValue<Slider>().Value == getPassiveBuff)
                {
                 //   Utils.ShowNotification(getPassiveBuff.ToString(), System.Drawing.Color.Blue, 100, true);
                    skills.rCast(minion);
                }
            }
        }
        public void harrash(Obj_AI_Hero target)
        {
           //    skills.qCast(target);
            var useQ = p.getMenu().Item("QH").GetValue<bool>();
            var useW = p.getMenu().Item("WH").GetValue<bool>();
            var useE = p.getMenu().Item("EH").GetValue<bool>();
    //        int q = p.getMenu().Item("sethQ").GetValue<Slider>().Value;
            if (useQ && !useW && !useE)
            {
                skills.qCast(target);
            }
            else if (!useQ && useW && !useE)
                skills.wCast(target);
            else if (!useQ && !useW && useE)
                skills.eCast(target);
            else if (!useQ && useW && useE)
            {
                skills.wCast(target);
                skills.eCast(target);
            }
            else if (useQ && !useW && useE)
            {
                skills.qCast(target);
                skills.eCast(target);
            }
            else if (useQ && useW && !useE)
            {
                skills.qCast(target);
                skills.wCast(target);
            }
            else if (useQ && useW && useE) //   Q+W+Q+E+Q+R
            {
                skills.wCast(target);
                skills.eCast(target);
                skills.qCast(target);
            }
            else
            {
                return;
            }
        }
        public void combo(Obj_AI_Hero target)
        {
           var useQ = p.getMenu().Item("QC").GetValue<bool>();
            var useW = p.getMenu().Item("WC").GetValue<bool>();
            var useE = p.getMenu().Item("EC").GetValue<bool>();
            var useR = p.getMenu().Item("RC").GetValue<bool>();
            if (useQ && !useW && !useE) skills.qCast(target);
            else if (!useQ && useW && !useE) skills.wCast(target);
            else if (!useQ && !useW && useE) skills.eCast(target);
            else if (!useQ && useW && useE)
            {
                skills.wCast(target);
                skills.eCast(target);
            }
            else if (useQ && !useW && useE)
            {
                skills.qCast(target);
                skills.eCast(target);
            }
            else if (useQ && useW && !useE)
            {
                skills.qCast(target);
                skills.wCast(target);
            }
            else if (useQ && useW && useE) //   Q+W+Q+E+Q+R
            {
                skills.qCast(target);
                skills.wCast(target);
                skills.qCast(target);
                skills.eCast(target);
                skills.qCast(target);
        
            }
            else
                return;
            if (useR)
            {
                if (p.getMenu().Item("setcharges").GetValue<Slider>().Value == getPassiveBuff)
                {
              //      Utils.ShowNotification(getPassiveBuff.ToString(), System.Drawing.Color.Blue, 100, true);
                    skills.rCast(target);
                }
            }
        }

    }
}
