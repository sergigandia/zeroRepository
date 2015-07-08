using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using SharpDX;
using System.Drawing;
using LeagueSharp.Common;

namespace MasterOfInsecRework
{
    class Polygon
    {
        public List<Vector2> points = new List<Vector2>();
        private Program p;
    

      
        public Polygon()
        {
            
        }

        public void load(Program p)
        {
            this.p = p;
        }


        public Polygon(List<Vector2> P)
        {
            points = P;
        }

        public void add(Vector2 vec)
        {
            points.Add(vec);
        }

        public int count()
        {
            return points.Count;
        }

        public Vector2 getProjOnPolygon(Vector2 vec)
        {
            Vector2 closest = new Vector2(-1000, -1000);
            Vector2 start = points[count() - 1];
            foreach (Vector2 vecPol in points)
            {
                Vector2 proj = projOnLine(start, vecPol, vec);
                closest = closestVec(proj, closest, vec);
                start = vecPol;
            }
            return closest;
        }

        public Vector2 closestVec(Vector2 vec1, Vector2 vec2, Vector2 to)
        {
            float dist1 = Vector2.DistanceSquared(vec1, to);//133
            float dist2 = Vector2.DistanceSquared(vec2, to);//12
            return (dist1 > dist2) ? vec2 : vec1;
        }      

        private Vector2 projOnLine(Vector2 v, Vector2 w, Vector2 p)
        {
            Vector2 nullVec = new Vector2(-1, -1);
            // Return minimum distance between line segment vw and point p
            float l2 = Vector2.DistanceSquared(v, w);  // i.e. |w-v|^2 -  avoid a sqrt
            if (l2 == 0.0)
                return nullVec;   // v == w case
            // Consider the line extending the segment, parameterized as v + t (w - v).
            // We find projection of point p onto the line. 
            // It falls where t = [(p-v) . (w-v)] / |w-v|^2
            float t = Vector2.Dot(p - v, w - v) / l2;
            if (t < 0.0)
                return nullVec;       // Beyond the 'v' end of the segment
            else if (t > 1.0)
                return nullVec;  // Beyond the 'w' end of the segment
            Vector2 projection = v + t * (w - v);  // Projection falls on the segment
            return projection;
        }

    }
}