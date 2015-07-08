using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace MasterOfInsecRework
{
    class Skills
    {
        private static Spell Q, W, E, R, qHarrash;
        private SpellSlot ignite;
        private SpellSlot smite;
        Orbwalking.Orbwalker x;

        //intalize spell
        public Skills()
        {
            //   Ignite = new Spell(SpellSlot.Summoner1, 1100);
            Q = new Spell(SpellSlot.Q, 1100);
            qHarrash = new Spell(SpellSlot.Q, 700);
            W = new Spell(SpellSlot.W, 700);
            E = new Spell(SpellSlot.E, 430);
            R = new Spell(SpellSlot.R, 375);
           

            smite = ObjectManager.Player.GetSpellSlot("SummonerDot");
            ignite = ObjectManager.Player.GetSpellSlot("summonersmite");
            qHarrash.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
            Q.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
            R.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
        }

        public Spell getQ()
        {
            return Q;
        }
        public Spell getW()
        {
            return W;
        }
        public Spell getE()
        {
            return E;
        }
        public Spell getR()
        {
            return R;
        }
        public SpellSlot getIgnite()
        {
            return ignite;
        }

        public HitChance hitchanceCheck(int i)
        {
            switch (i)
            {
                case 1:
                    return HitChance.Low;
                case 2:
                    return HitChance.Medium;
                case 3:
                    return HitChance.High;
                case 4:
                    return HitChance.VeryHigh;
            }
            return HitChance.Low;
        }

        //El control de la pasiva debe de ir aqui dentro, osea cuando castees las skills se debe controlar la pasiva tmb.
           
        public bool qCast(Obj_AI_Base target, int hitChance)
        {
            if (target == null) return false;
            if (Q.IsReady() && Q.CanCast(target) && Q.IsInRange(target) &&
                ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name.ToLower() == "blindmonkqone")
            {
                Q.CastIfHitchanceEquals(target, hitchanceCheck(hitChance)); // Continue like that
                return true;
            }          
            return false;
        }
        public bool qCast2(Obj_AI_Base target)
        {
            if (target != null)             
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name.ToLower() == "blindmonkqtwo")
            {
                
                Q.Cast();
               // Utils.ShowNotification("despues del if", System.Drawing.Color.White, 100);
                return true;

            }
            return false;
        }
            
        public bool wCast(Obj_AI_Base target)
        {

            if (target == null) return false;

            if (W.IsReady())
            {
                W.CastOnUnit(target);
                return true;

            }
            return false;
        }
   

        public bool eCast(Obj_AI_Base target)
        {
            if (E.IsReady() && E.CanCast(target) && E.IsInRange(target))
            {
                E.Cast();
                return true;
            }
            return false;
        }

      

        public bool rCast(Obj_AI_Base target)
        {
            if (target == null) return false;
            if (R.IsReady() && R.IsInRange(target))
            {
                R.Cast(target);
                return true;
            }
            return false;
        }

        public bool igniteCast(Obj_AI_Base target)
        {
            if (ignite.IsReady() && target.Health - ObjectManager.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite) <= 0)
            {
                ObjectManager.Player.Spellbook.CastSpell(ignite, target);
                return true;
            }
            return false;
        }
    }
}
