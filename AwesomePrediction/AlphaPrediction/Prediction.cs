using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomePrediction
{
   public static class Awesome
    {
        enum Direction
        {
            right = 0,
            left = 1,
            forward = 2
        }
        private struct Distance
        {
            public Distance(Vector3 pos1, Vector3 pos2, float dist)
            {
                this.pos1 = pos1;
                this.pos2 = pos2;
                this.dist = dist;
            }
            public Vector3 pos1;
            public Vector3 pos2;
            public float dist;
        }
        private struct Circle
        {
            public Circle(Vector3 pos, float range)
            {
                this.pos = pos;
                this.range = range;
            }

           public Vector3 pos;
            public float range;
        }

        public static bool SpellAoePrediction(Spell spell)
        {
            var targets = ObjectManager.Get<Obj_AI_Hero>().Where(t => t.IsEnemy && t.Distance(ObjectManager.Player) < spell.Range);
            var positions = new List<List<Vector3>>();
            foreach (Obj_AI_Base t in targets)
            {
               var posn = new List<Vector3>();

                foreach (Obj_AI_Base te in targets)
                {
                    var dist = t.Distance(ObjectManager.Player);
                    var t1pos = LeagueSharp.Common.Prediction.GetPrediction(t, dist / spell.Speed).UnitPosition;
                    var t2pos = LeagueSharp.Common.Prediction.GetPrediction(te, dist / spell.Speed).UnitPosition;
                    if (t1pos.Distance(t2pos) < spell.Width)
                    {
                        posn.Add(t1pos);
                    }

                }
                positions.Add(posn);
            }

           var posfint = positions.Max(t => t.Count);
            var posin = positions.Where(t => t.Count == posfint).FirstOrDefault();
            if (posfint >= 2)
            {
                var flist = new List<float>();
               var firstdis = new Distance(new Vector3(), new Vector3(), 0);
                for (var i = 0; i < posin.Count; i++)
                {
                    for (var x = i + 1; x <= posin.Count; x++)
                    {
                        var d = posin[i].Distance(posin[x]);
                        if (d > firstdis.dist)
                        {
                            firstdis = new Distance(posin[i], posin[x], d);
                        }
                    }
                }
                //  firstdis
                Vector3 pos;
                pos.X = (firstdis.pos1.X + firstdis.pos2.X) / 2;
                pos.Y = (firstdis.pos1.Y + firstdis.pos2.Y) / 2;
                pos.Z = (firstdis.pos1.Z + firstdis.pos2.Z) / 2;
                spell.Cast(pos);
                Render.Circle.DrawCircle(pos, 10, System.Drawing.Color.Red, 2);
                return true;
            }
            //     Render.Circle.DrawCircle(line_finish, 10, System.Drawing.Color.Red, 2);
            return false;

        }

        public static bool SpellPrediction(Spell spell, Obj_AI_Hero target,bool collisionable)
        {
  //
            var dist = ObjectManager.Player.Distance(target.Position);
            var pos1 = LeagueSharp.Common.Prediction.GetPrediction(target, dist / spell.Speed).UnitPosition-40;
            var dister = target.Position.Extend(target.GetWaypoints()[1].To3D(), target.GetWaypoints()[1].To3D().Distance(target.Position) + 50);
          //  var pos1 = LeagueSharp.Common.Prediction.GetPrediction(target, dist / spell.Speed).UnitPosition - 40;
            var wts = Drawing.WorldToScreen(target.Position);
            var wtsx = target.GetWaypoints()[1];
            Drawing.DrawLine(wts[0], wts[1], wtsx[0], wtsx[1], 2f, System.Drawing.Color.Red);
            var e = pos1.Distance(target.GetWaypoints()[1].To3D());

            pos1.Extend(target.GetWaypoints()[1].To3D(), -e);
            Render.Circle.DrawCircle(dister, 10, System.Drawing.Color.GreenYellow, 2);
            Render.Circle.DrawCircle(pos1, 10, System.Drawing.Color.BlueViolet, 2);
//
            var point = PointsAroundTheTarget(target.Position, target.BoundingRadius + 50).FirstOrDefault(t => t.IsWall());
            if (point.X != 0 && point.Y != 0 && point.Z != 0)
            {
                if (MinionCollideLine(ObjectManager.Player.Position, ExtendWallpos(target, point), spell,collisionable)) return false;

                Render.Circle.DrawCircle(ExtendWallpos(target, point), 10, System.Drawing.Color.Brown, 2);
                spell.Cast(ExtendWallpos(target, point));
                return true;
            }
            else
            {
                var range = spell.Range;
                if (target.Position.Distance(ObjectManager.Player.Position) < target.GetWaypoints()[1].Distance(ObjectManager.Player.Position))
                {
                    range -= 100;
                }

                if (!spell.IsInRange(target, range)) return false;
                
                /*if (target.IsFacing(ObjectManager.Player) && target.Position.Distance(ObjectManager.Player.Position) > target.GetWaypoints()[1].Distance(ObjectManager.Player.Position))
                {
                  if (MinionCollideLine(ObjectManager.Player.Position, target.Position, spell,collisionable)) return false;

                    {
                        Game.PrintChat("Casteando por inface");
                       spell.Cast(target.Position);

                        return true;
                    }

                }*/
                // code of dashes
                if (target.IsDashing())
                {
                    float timeforArrive=(target.Position.Distance(target.GetDashInfo().EndPos.To3D()))/target.GetDashInfo().Speed;
                    float grabtime =( ObjectManager.Player.Position.Distance(target.GetDashInfo().EndPos.To3D())
                                     / spell.Speed)+spell.Delay;
                    if (timeforArrive<grabtime)
                    {
                        spell.Cast(target.GetDashInfo().EndPos);
                        return true;
                    }
                }
               /* if (target.IsImmovable) // check for cc guys
                {
                    if (MinionCollideLine(ObjectManager.Player.Position, target.Position, spell,collisionable)) return false;
                    spell.Cast(target.Position);
                    return true;
                }*/

              //  if(target.IsChannelingImportantSpell())
                if (target.IsWindingUp && !target.IsMelee())
                {
                    if (MinionCollideLine(ObjectManager.Player.Position, target.Position, spell,collisionable)) return false;
                    spell.Cast(target.Position);
                    return true;
                }

                if (target.Position.Distance(ObjectManager.Player.Position) <= 300)
                {
                    CastToDirection( target, spell,collisionable);
                }
                else
                {
                    var oldPos = target.GetWaypoints()[0].To3D();
                   var h = false;
                    Utility.DelayAction.Add(Game.Ping + 1000, () => h = Next(target, oldPos, spell,collisionable));
                    return h;
                }
            }
            return false;
        }

        private static bool PlayerIsStop(Vector3 pos, Obj_AI_Base target,Spell spell,bool collisionable)
        {
            if (pos == target.Position)
            {
                if (MinionCollideLine(ObjectManager.Player.Position, target.Position, spell,collisionable)) return false;
                spell.Cast(target.Position);
                return true;
            }
            return false;
        }

        private static bool Next(Obj_AI_Base target, Vector3 oldpos, Spell spell, bool collisionable)
        {
            if (oldpos != target.GetWaypoints()[1].To3D())
            {
          //      Utility.DelayAction.Add(1500, () => PlayerIsStop(target.Position, target, spell));
                Utility.DelayAction.Add(1000, () => Next(target, target.GetWaypoints()[1].To3D(), spell, collisionable));
                return false;
            }
            else
            {
                if (CastToDirection(target, spell,collisionable))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


       private static Vector3 ExtendWallpos(Obj_AI_Base ts, Vector3 point)
        {
            if (ts == null) throw new ArgumentNullException("ts");
            return point.Extend(ts.Position, point.Distance(ts.Position) +25);
        }

        private static bool CastToDirection( Obj_AI_Base target, Spell spell,bool collisionable)
        {
         // punto trasero a la posicion q va estar el personage cuando se lance el gancho
            var dist = ObjectManager.Player.Distance(target.Position);
            var pos1 = LeagueSharp.Common.Prediction.GetPrediction(target, dist / spell.Speed).UnitPosition-40;
            var e = pos1.Distance(target.GetWaypoints()[1].To3D());

            pos1.Extend(target.GetWaypoints()[1].To3D(), +e);
            Render.Circle.DrawCircle(pos1, 10, System.Drawing.Color.BlueViolet, 2);
            var wts = Drawing.WorldToScreen(target.Position);
            var wtsx = target.GetWaypoints()[1];
            Drawing.DrawLine(wts[0], wts[1], wtsx[0], wtsx[1], 2f, System.Drawing.Color.Red);
            if (MinionCollideLine(ObjectManager.Player.Position, pos1,spell,collisionable)) return false;
            spell.Cast(pos1);
            return true;
        }
     private static bool MinionCollideLine(Vector3 lineStart, Vector3 lineFinish, Spell spell,bool check)
     {
         if (check == false) return false;
               var minion =
                                MinionManager.GetMinions(spell.Range, MinionTypes.All, MinionTeam.NotAlly, MinionOrderTypes.Health);
          //  Render.Circle.DrawCircle(ObjectManager.Player.Position, 100, System.Drawing.Color.Brown, 2);
            //   Render.Circle.DrawCircle(line_finish, 10, System.Drawing.Color.Red, 2);
            var d = lineStart.Distance(lineFinish);
            //    dis = (int)(dis /spell.Width);
            var circles = new List<Circle>();
            for (var i = 0; i < d; i += 10)
            {
                var dist = i > d ? d : i;
                var point = lineStart.Extend(lineFinish, +dist);
                circles.Add(new Circle(point, spell.Width));
            }
            foreach (var c in circles)
          {
              foreach (var m in minion)
              {
                  if (Geometry.CircleCircleIntersection(c.pos.To2D(), m.Position.To2D(), c.range, 100).Count()!=0)
                  {
                      return true;
                  }

              }
          }
            //  foreach(Obj_AI_Base m in minion)
            //  {
            //     m.
            //  }
            // Geometry.cu
            return false;
        }
        private static bool WillHitEnemys(Obj_AI_Base zone, int range, int min)
        {
            var i = ObjectManager.Get<Obj_AI_Hero>().Count(b => b.IsEnemy && !b.IsDead && b.Distance(zone) < range);
            return i >= min;
        }
        private static IEnumerable<Vector3> PointsAroundTheTarget(Vector3 pos, float dist, float prec = 15, float prec2 = 5)
        {
            if (!pos.IsValid())
            {
                return new List<Vector3>();
            }
            var list = new List<Vector3>();
            if (dist > 500)
            {
                prec = 20;
                prec2 = 6;
            }
            if (dist > 805)
            {
                prec = 35;
                prec2 = 8;
            }
            var angle = 360 / prec * Math.PI / 180.0f;
            var step = dist / prec2;
            for (int i = 0; i < prec; i++)
            {
                for (int j = 0; j < prec2; j++)
                {
                    list.Add(
                        new Vector3(
                            pos.X + (float)(Math.Cos(angle * i) * (j * step)),
                            pos.Y + (float)(Math.Sin(angle * i) * (j * step)), pos.Z));
                }
            }

            return list;
        }
    }
}
