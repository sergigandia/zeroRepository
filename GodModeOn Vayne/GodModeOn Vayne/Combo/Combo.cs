using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodModeOn_Vayne.Combo
{
    static class Combo
    {
        public static void Do()
            {
            // uso de la q
                var Qcombo = Program.menu.Item("QC").GetValue<bool>();
                var Ecombo = Program.menu.Item("EC").GetValue<bool>();
                var Etarcombo = Program.menu.Item("ECT").GetValue<bool>();
                var Rcombo = Program.menu.Item("RC").GetValue<bool>();
                var Rmincombo = Program.menu.Item("CNr").GetValue<Slider>().Value;
           //     var Etarcombo = Program.menu.Item("RC").GetValue<bool>();
       //     TargetSelector.
                var target = TargetSelector.GetTarget(800, TargetSelector.DamageType.Physical);
            if(Qcombo)
            {
                if (target != null)
                {
                    Program.Q.Cast(Game.CursorPos, false);
                }
            }
            if (Ecombo)
            {
                if (target != null)
                {
                    if (!Etarcombo)
                    {
                        if (treesCondemn(target.Position))
                            Program.E.Cast(target);
                    }
                    else
                    {
                        if (target == TargetSelector.GetSelectedTarget())
                        {
                            if (treesCondemn(target.Position))
                                Program.E.Cast(target);
                        }
                    }
                }
            }
          if(Rcombo)
           {
              if (WillHitEnemys(Program.Player,800,Rmincombo))
              {
                  Program.R.Cast();
              }
           }
            //end uso de la q
            }
        public static float CondemnRange = 550f;
        public static float CondemnKnockback = 490f;
        private static bool treesCondemn(Vector3 position)
        {
            var pointList = new List<Vector3>();

            for (var j = CondemnKnockback; j >= 50; j -= 100)
            {
                var offset = (int)(2 * Math.PI * j / 100);

                for (var i = 0; i <= offset; i++)
                {
                    var angle = i * Math.PI * 2 / offset;
                    var point =
                        new Vector2(
                            (float)(position.X + j * Math.Cos(angle)),
                            (float)(position.Y - j * Math.Sin(angle))).To3D();

                    if (point.IsWall())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public static bool MyCondemn(Vector3 fromPosition,Obj_AI_Hero target )
        {
     /*      var line = new Geometry.Polygon.Line(target.Position, Program.Efinishpos(target));
            if (line.Points.Any(point => point.To3D().IsWall()))
           {
               return true;
            }*/
          var d = target.Position.Distance(Program.Efinishpos(target));
for(var i = 0; i < d; i+=10){
 var dist = i > d ? d : i;
 var point = target.Position.Extend(Program.Efinishpos(target), dist);
 if (point.IsWall()) return true;
}
return false;
        /*        if (Program.Efinishpos(target).IsWall())
                    return true;*/
        }
        public static bool WillHitEnemys(Obj_AI_Base zone, int Range, int min)
        {
            int i = 0;
            int mine = 0;
            foreach (Obj_AI_Hero b in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (b.IsEnemy && !b.IsDead && b.Distance(zone) < Range)
                {
                    i++;
                }
            }
            mine = i;
            if (i >= min)
                return true;
            else
                return false;
        }

    }
}
