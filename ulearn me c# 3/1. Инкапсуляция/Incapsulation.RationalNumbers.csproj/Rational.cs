using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        public Rational(int numerator = 0, int denominator = 1)
        {
            var gcd = FindGcd(numerator, denominator);
            if (gcd == 0)
            {
                Numerator = 0;
                Denominator = 0;
                return;
            }
            
            var nextNumerator = numerator / gcd;
            var nextDenominator = denominator / gcd;
            
            Numerator = Math.Abs(numerator / gcd) * (nextDenominator < 0 ? -1 : 1) * (nextNumerator < 0 ? -1 : 1);
            Denominator = Math.Abs(nextDenominator);
        }
        
        public bool IsNan => Denominator == 0;

        public static Rational operator +(Rational a, Rational b)
        {
            var gcd = FindGcd(a.Denominator, b.Denominator);

            return new Rational((a.Numerator * b.Denominator + b.Numerator * a.Denominator) / gcd,
                a.Denominator * b.Denominator / gcd);
        } 
        
        public static Rational operator -(Rational a, Rational b)
        {
            return a + (-1 * b);
        }
        
        public static Rational operator *(Rational a, Rational b)
        {
            return new Rational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        } 
        
        public static Rational operator /(Rational a, Rational b)
        {
            if (b.IsNan) return new Rational
            {
                Numerator = 0,
                Denominator = 0,
            };

            return new Rational(a.Numerator * b.Denominator * (b.Numerator < 0 ? -1 : 1), a.Denominator * Math.Abs(b.Numerator));
        } 
        
        public static Rational operator *(Rational a, int f)
        {
            return new Rational(a.Numerator * f, a.Denominator);
        } 
        
        public static Rational operator *(int f, Rational a)
        {
            return new Rational(a.Numerator * f, a.Denominator);
        } 
        
        public static implicit operator double(Rational r) => r.IsNan ? double.NaN : (double)r.Numerator / r.Denominator;
        public static implicit operator Rational(int val) => new Rational
        {
            Numerator = val,
            Denominator = 1,
        };
        public static explicit operator int(Rational r)
        {
            if (r.IsNan || r.Numerator % r.Denominator != 0)
            {
                throw new Exception();
            }
            
            return r.Numerator / r.Denominator;
        }

        public static int FindGcd(int a, int b)
        {
            (a, b) = (Math.Max(a, b), Math.Min(a, b));

            while (b != 0)
            {
                (a, b) = (b, a % b);
            }

            return Math.Abs(a);
        }
    }
}
