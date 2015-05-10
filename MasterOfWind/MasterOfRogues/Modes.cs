using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
namespace MasterOfWind
{
    class Modes
    {
        public Obj_AI_Base selectedminion;
        public Obj_AI_Base selectedminions;
        private Obj_AI_Hero target;
       private Program p;
      private  Skills skills;
        public Skills getSkills()
      {
          return skills;
      }
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
        public void laneClear()
        {

            var minion = MinionManager.GetMinions(skills.getE().Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth).FindAll(m => !m.HasBuff("YasuoDashWrapper")).FirstOrDefault();
            var useQ = p.getMenu().Item("QL").GetValue<bool>();
            var useE = p.getMenu().Item("EL").GetValue<bool>();
            if (useQ &&  !useE) skills.qCast(minion);
            else if (!useQ &&  useE) skills.eCast(minion);
            else if (!useQ && useE)  
            {
                skills.eCast(minion);
            }
            
            else if (useQ &&  useE)
            {
                skills.qCast(minion);
                skills.eCast(minion);
            }
            else if (useQ &&  !useE)
            {
                skills.qCast(minion);
            }
            else if (useQ &&  useE)
            {
                skills.qCast(minion);
                skills.eCast(minion);
            }
            else
                return;
            selectedminion = minion;
        }
        public void jungleClear()
        {

            var minion = MinionManager.GetMinions(skills.getE().Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FindAll(m => !m.HasBuff("YasuoDashWrapper")).FirstOrDefault();
            var useQ = p.getMenu().Item("QJ").GetValue<bool>();
            var useE = p.getMenu().Item("EJ").GetValue<bool>();
            if (useQ && !useE) skills.qCast(minion);
            else if (!useQ &&  useE) skills.eCast(minion);
            else if (!useQ &&  useE)
            {
                skills.eCast(minion);
            }

            else if (useQ &&  useE)
            {
                skills.qCast(minion);
                skills.eCast(minion);
            }
            else if (useQ &&  !useE)
            {
                skills.qCast(minion);
            }
            else if (useQ &&  useE)
            {
                skills.qCast(minion);
                skills.eCast(minion);
            }
            else
                return;
            selectedminion = minion;
        }
        public void harrash(Obj_AI_Hero target)
        {
           //    skills.qCast(target);
            var useQ = p.getMenu().Item("QH").GetValue<bool>();
            var useE = p.getMenu().Item("EH").GetValue<bool>();
    //        int q = p.getMenu().Item("sethQ").GetValue<Slider>().Value;
            if (useQ &&  !useE)
            {
                skills.qCast(target);
            }
            else if (!useQ && useE)
            {
                skills.eCast(target);
            }
            else if (useQ &&  useE)
            {
                skills.qCast(target);
                skills.eCast(target);
            }
            else if (useQ && !useE)
            {
                skills.qCast(target);
            }
            else if (useQ &&  useE) //   Q+W+Q+E+Q+R
            {
                skills.eCast(target);
                skills.qCast(target);
            }
            else
            {
                return;
            }
        }
        public void flee(Obj_AI_Base target)
        {
            //use w for flee?
            //load second q for flee?
          p.getPlayer().IssueOrder(GameObjectOrder.MoveTo, p.getPlayer().Position.Extend(Game.CursorPos, 150));
            eFlee();
        }
        public void eFlee()
        {
            Obj_AI_Base minion = ObjectManager.Get<Obj_AI_Base>().Where(x => x.IsMinion && skills.getE().IsInRange(x) && !x.HasBuff("YasuoDashWrapper")).MinOrDefault(x => x.Distance(Game.CursorPos));
            if (minion.Distance(Game.CursorPos) < ObjectManager.Player.Distance(Game.CursorPos))
           {
                skills.eCast(minion);
                selectedminions = minion;
           }
       }
        public void eLogic(Obj_AI_Hero Target)
        {
            if(!skills.getE().IsInRange(Target))
            {
                Obj_AI_Base minion = ObjectManager.Get<Obj_AI_Base>().Where(x => x.IsMinion && skills.getE().IsInRange(x) && !x.HasBuff("YasuoDashWrapper")).MinOrDefault(x => x.Distance(Target));
                if (minion.Distance(target) < ObjectManager.Player.Distance(target))
                {
                    skills.eCast(minion);
                    selectedminions = minion;
                }
            }
            else
            {
                skills.eCast(Target);
                selectedminions = Target;
            }
        }
             
        public void combo(Obj_AI_Hero target)
        {
           var useQ = p.getMenu().Item("QC").GetValue<bool>();
            var useE = p.getMenu().Item("EC").GetValue<bool>();
            var useR = p.getMenu().Item("RC").GetValue<bool>();
            if (useQ &&  !useE) skills.qCast(target);
            else if (!useQ && useE) eLogic(target);
            else if (!useQ &&  useE)
            {
                eLogic(target);
            }
            else if (useQ && useE)
            {
                skills.qCast(target);
                eLogic(target);
            }
            else if (useQ && !useE)
            {
                skills.qCast(target);
            }
            else if (useQ &&  useE) //   Q+W+Q+E+Q+R
            {
                skills.qCast(target);
                eLogic(target);
            }
            else
                return;
            if (useR)
                skills.rCast(target);
        }

    }
}
