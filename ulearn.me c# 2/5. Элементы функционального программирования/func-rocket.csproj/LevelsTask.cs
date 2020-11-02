using System;
using System.Collections.Generic;

namespace func_rocket
{
    public class LevelsTask
    {
        static readonly Physics standardPhysics = new Physics();

        public static IEnumerable<Level> CreateLevels()
        {
            Func<Rocket> getRocket = () => new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);

            Func<string, Vector, Gravity, Level> getLevel =
                (name, target, gravity) => new Level(name,
                    getRocket(),
                    target,
                    gravity,
                    standardPhysics
                );

            Func<Vector, Vector, double> getDistance = (Vector r, Vector t) =>
                Math.Sqrt(Math.Pow(t.X - r.X, 2) + Math.Pow(t.Y - r.Y, 2));

            Func<double, double, double> getHoleForce = (double k, double distance) =>
                k * distance / (distance * distance + 1);
            
            Func<double, Vector, bool, Gravity> getHoleGravity = (double k, Vector vector, bool isWhite) =>
                (size, v) =>
                    ((isWhite ? 1 : -1) * (v - new Vector(vector.X, vector.Y))).Normalize() *
                    getHoleForce(k, getDistance(v, new Vector(vector.X, vector.Y)));
            
            var whiteHoleGravity = getHoleGravity(140, new Vector(600, 200), true);
            var blackHoleGravity = getHoleGravity(300, new Vector(400, 350), false);

            yield return getLevel(
                "Zero",
                new Vector(600, 200),
                (size, v) => Vector.Zero
            );
            yield return getLevel(
                "Heavy",
                new Vector(600, 200),
                (size, v) => new Vector(0, 0.9)
            );
            yield return getLevel(
                "Up",
                new Vector(700, 500),
                (size, v) => new Vector(0, -300 / (size.Height - v.Y + 300.0))
            );
            yield return getLevel(
                "WhiteHole",
                new Vector(600, 200),
                whiteHoleGravity
            );
            yield return getLevel(
                "BlackHole",
                new Vector(600, 200),
                blackHoleGravity
            );
            yield return getLevel(
                "BlackAndWhite",
                new Vector(600, 200),
                (size, v) => (blackHoleGravity(size, v) + whiteHoleGravity(size, v)) / 2
            );
        }
    }
}