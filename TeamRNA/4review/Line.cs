using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace TeamRNA.ForReview
{
    internal class Line
    {
        public double K { get; private set; }
        public double B { get; private set; }

        public override string ToString()
        {
            return string.Format("y={0}x{1}{2}", K, Math.Sign(B) >= 0 ? "+" : "-", Math.Abs(B));
        }

        public Line(double k, double b)
        {
            K = k;
            B = b;
        }

        internal static double K0(Vector p2, Vector p1)
        {
            if (p2.X - p1.X == 0) return 0;
            return Math.Round((p2.Y - p1.Y) / (p2.X - p1.X), 1);
        }
        internal static double B0(Vector p2, Vector p1)
        {
            return Math.Round(p2.Y - K0(p2, p1) * p2.X, 1);
        }
        internal static Line One(Vector p2, Vector p1)
        {
            return new Line(K0(p2, p1), B0(p2, p1));
        }
        internal static Line One(IPosition p2, IPosition p1)
        {
            return One(p2.Position, p1.Position);
        }
        internal static Line Normal(Line line, Vector point)
        {
            if (line.K == 0) return new Line(0, 0);
            var kn = Math.Round(-1f / line.K, 1);
            var bn = Math.Round(point.Y - kn * point.X, 1);
            return new Line(kn, bn);
        }
        internal static Line Normal(Line line, IPosition point)
        {
            return Normal(line, point.Position);
        }
        internal static Vector Cross(Line line1, Line line2)
        {
            if (line2.K - line1.K == 0) return new Vector(0, line1.B);
            var x = (line1.B - line2.B) / (line2.K - line1.K);
            var y = line1.K * x + line1.B;
            return new Vector(x, y);
        }
    }
}
