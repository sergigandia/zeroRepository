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
    class Skills 
    {
        private Spell Q,SecondQ, W, E, R;
        private SpellSlot ignite;

        public Skills()
        {
            Q = new Spell(SpellSlot.Q, 475); // line
            SecondQ = new Spell(SpellSlot.Q, 1000);
            W = new Spell(SpellSlot.W, 400);  // wall
            E = new Spell(SpellSlot.E, 475); // line
            R = new Spell(SpellSlot.R,1200); // circle
            Q.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
            SecondQ.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);
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
           if (!HaveQ3)
           {
               if (Q.IsReady())
               {
                   Q.CastIfHitchanceEquals(target, HitChance.High);
                   return true;
               }
           }
           else
           {
               if (SecondQ.IsReady())
               {
                   SecondQ.CastIfHitchanceEquals(target, HitChance.High);
                   return true;
               }
           }
           return false;

       }
        public bool wCast(Obj_AI_Base  target)
       {
           if (target == null) return false;
      //wall not coded
           return false;
       }
        public bool eCast(Obj_AI_Base  target)
        {
                      if (target == null) return false;
                      if (E.IsReady() && E.IsInRange(target))
                      {
                          E.Cast(target);
                          return true;
                      }
             return false;
        }
        private bool canCastR(Obj_AI_Base target)
        {
            return target.HasBuffOfType(BuffType.Knockup) || target.HasBuffOfType(BuffType.Knockback);
        }
        public  bool rCast(Obj_AI_Base  target)
        {
           if (target == null) return false;
           if (R.IsReady() && canCastR(target))
           {
               R.Cast();
               return true;
           }
           return false;
        }
        public bool HaveQ3
        {
            get { return ObjectManager.Player.HasBuff("YasuoQ3W"); }
        }
        public bool IgniteCast(Obj_AI_Hero target)
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
