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
    class Skills : Program
    {
        private Spell Q, W, E, R;
        private SpellSlot ignite;

        public Skills()
        {
            Q = new Spell(SpellSlot.Q, 800); // circle
            W = new Spell(SpellSlot.W, 850);  // circle
            E = new Spell(SpellSlot.E, 1100); // line
            R = new Spell(SpellSlot.R, 700); // circle
            Q.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, false, SkillshotType.SkillshotCircle);
            W.SetSkillshot(W.Instance.SData.SpellCastTime, W.Instance.SData.LineWidth, W.Instance.SData.MissileSpeed,false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(E.Instance.SData.SpellCastTime, E.Instance.SData.LineWidth, E.Instance.SData.MissileSpeed, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(R.Instance.SData.SpellCastTime, R.Instance.SData.LineWidth, R.Instance.SData.MissileSpeed, false, SkillshotType.SkillshotCircle);
            ignite = ObjectManager.Player.GetSpellSlot("SummonerDot");
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
       public HitChance HitchanceCheck(int i)
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
       public bool qCast(Obj_AI_Base target)
       {
           if (target == null) return false;
           if (Q.IsReady())
           {
               Q.CastIfHitchanceEquals(target, HitChance.High);
               return true;
           }
           return false;

       }
       public bool wCast(Obj_AI_Base target)
       {
                      if (target == null) return false;
           if (W.IsReady())
           {
               W.CastIfHitchanceEquals(target, HitChance.High);
               return true;
           }
           return false;
       }
       public bool eCast(Obj_AI_Base target)
        {
                      if (target == null) return false;
           if (E.IsReady())
           {
               E.CastIfHitchanceEquals(target, HitChance.High);
               return true;
           }
           return false;
        }
       public bool rCast(Obj_AI_Base target)
        {
               if (target == null) return false;
           if (R.IsReady())
           {
               R.CastIfHitchanceEquals(target, HitChance.High);
               return true;
           }
           return false;
        }
       public bool IgniteCast(Obj_AI_Base target)
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
