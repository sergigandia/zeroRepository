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
        private Skills skills;
        private Program p;
        public Modes()
        {
        }
        public void load(Program p)
        {
            skills = new Skills();
            this.p =p;
            target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Magical);
        }
        public Obj_AI_Hero getTarget()
        {
            target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Magical);
            return target;
        }
        //combos
        //Harrash Q+W
        //Harrash poca movilidad Q+W+W
        //  E+W+Q+W
        //  E+W+Q+W+R
        //  R+Q+W+E+W
        public void laneClear()
        {
            var minion = MinionManager.GetMinions(skills.getQ().Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth).FirstOrDefault();
            skills.rCast(minion);
            skills.eCast(minion);
            if (skills.getE().IsReady())
                skills.wCast(minion);
            skills.qCast(minion);
            if (skills.getQ().IsReady())
                skills.wCast(minion);
        }
        public void jungleClear()
        {
            var minion = MinionManager.GetMinions(skills.getQ().Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();
          //  var useQ = p.getMenu().Item("QJ").GetValue<bool>();
          //  var useW =p.getMenu().Item("WJ").GetValue<bool>();
          //  var useE =p.getMenu().Item("EJ").GetValue<bool>();
            skills.rCast(minion);
            skills.eCast(minion);
            if (skills.getE().IsReady())
                skills.wCast(minion);
            skills.qCast(minion);
            if (skills.getQ().IsReady())
                skills.wCast(minion);
        }
        public void harrash(Obj_AI_Hero target)
        {
            skills.qCast(target);
            if (skills.getQ().IsReady())
            skills.wCast(target);
        }
        public void Flee(Obj_AI_Hero target)
        {
            p.getPlayer().IssueOrder(GameObjectOrder.MoveTo, p.getPlayer().Position.Extend(Game.CursorPos, 150));
            skills.eCast(target);
        }
        public void OnlyR(Obj_AI_Hero target)
        {
           p.getPlayer().IssueOrder(GameObjectOrder.MoveTo, p.getPlayer().Position.Extend(Game.CursorPos, 150));
            skills.rCast(target);
        }
        public void combo(Obj_AI_Hero target)
        {
            skills.eCast(target);
            if (skills.getE().IsReady())
            skills.wCast(target);
            skills.qCast(target);
            if(skills.getQ().IsReady())
            skills.wCast(target);
        }
        public void rCombo(Obj_AI_Hero target)
        {
            p.getPlayer().IssueOrder(GameObjectOrder.MoveTo, p.getPlayer().Position.Extend(Game.CursorPos, 150));
            skills.rCast(target);
            skills.eCast(target);
            if (skills.getE().IsReady())
                skills.wCast(target);
            skills.qCast(target);
            if (skills.getQ().IsReady())
                skills.wCast(target);
        }

    }
}
