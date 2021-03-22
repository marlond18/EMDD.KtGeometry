using KtExtensions;

using System;
using System.Collections.Generic;

namespace EMDD.KtGeometry.Angles
{
    public abstract class Angle : IEquatable<Angle>
    {
        protected Angle(double value)
        {
            Value = value;
        }

        public double Value { get; }

        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj) ||
            (!(this is null) &&
            !(obj is null)
            && obj is Angle a
            && Equals(a));

        public bool Equals(Angle other) =>
            ReferenceEquals(this, other) ||
            (!(this is null)
            && (other?.ToDegrees().Value.NearEqual(Value, 5) ?? false));

        public override int GetHashCode() =>
            unchecked(HashCode.Combine(GetDerivedType, Value));

        public static bool operator ==(Angle left, Angle right) =>
            EqualityComparer<Angle>.Default.Equals(left, right);

        public static bool operator !=(Angle left, Angle right) =>
            !(left == right);

        internal Angle ToDegrees() => Degrees.Create(Value * 360.0 / OneCycle);

        public Angle NormalizeAngle() => Ctor(Value > OneCycle || Value < -OneCycle ? Value % OneCycle : Value);

        public Angle MakePositive() => Ctor(Value < 0 ? OneCycle + Value : Value);

        protected abstract Angle Ctor(double val);

        internal abstract double OneCycle { get; }

        internal abstract Type GetDerivedType { get; }

        private double NormalAngle => Value * Math.PI * 2 / OneCycle;

        public static double Cos(Angle angle) => angle is null ? 0 : Math.Cos(angle.NormalAngle);

        public static double Sin(Angle angle) => angle is null ? 0 : Math.Sin(angle.NormalAngle);

        public static double Tan(Angle angle) => angle is null ? 0 : Math.Tan(angle.NormalAngle);

        public static Angle Atan(double o, double a) => Degrees.Create(NumericExtensions.Atan3(o, a));

        public static Angle Acos(double a, double h) => h == 0 ? throw new DivideByZeroException() : Rads.Create(Math.Acos(a / h));

        public static Angle Asin(double o, double h) => h == 0 ? throw new DivideByZeroException() : Rads.Create(Math.Asin(o / h));

        private Angle Add(Angle angle) =>
            Ctor(Value + (angle.Value * OneCycle / angle.OneCycle));

        private Angle Negate() => Ctor(-Value);

        private Angle Multiply(double val) => Ctor(Value * val);

        private Angle Div(double val) => Ctor(Value / val);

        public static Angle operator +(Angle a, Angle b) => a.Add(b);

        public static Angle operator -(Angle a, Angle b) => a + -b;

        public static Angle operator -(Angle a) => a.Negate();

        public static Angle operator *(Angle a, double b) => a.Multiply(b);

        public static Angle operator *(double b, Angle a) => a.Multiply(b);

        public static Angle operator /(Angle a, double b) => a.Div(b);

        public static bool operator >(Angle a, Angle b) =>
            !ReferenceEquals(a, b)
            && (!(a is null) || b.Value < 0)
            && (!(b is null) || !(a is null))
            && a.ToDegrees().Value > b.ToDegrees().Value;

        public static bool operator <(Angle a, Angle b) => a.ToDegrees().Value < b.ToDegrees().Value;

        public bool IsZero => Value.NearZero();

        protected abstract string Sign { get; }

        public override string ToString() => Value.SmartToString() + Sign;

        public abstract Angle Complementary();
    }

    public class Nan : Angle
    {
        public Nan() : base(double.NaN)
        {
        }

        protected override string Sign => "";

        internal override double OneCycle => Value;

        internal override Type GetDerivedType => typeof(Nan);

        public override Angle Complementary() => new Nan();

        protected override Angle Ctor(double val) => new Nan();
    }

    public class Degrees : Angle
    {
        protected Degrees(double val) : base(val)
        {
        }

        public static Angle Create(double val)
        {
            if (double.IsNaN(val)) return new Nan();
            return new Degrees(val);
        }

        protected override string Sign => "°";

        internal override double OneCycle => 360;

        internal override Type GetDerivedType => typeof(Degrees);

        protected override Angle Ctor(double val) => new Degrees(val);

        public override Angle Complementary() => new Degrees(Value + 180);
    }

    public class Rads : Angle
    {
        protected Rads(double val) : base(val)
        {
        }

        public static Angle Create(double val)
        {
            if (double.IsNaN(val)) return new Nan();
            return new Rads(val);
        }

        internal override double OneCycle => Math.PI * 2;

        internal override Type GetDerivedType => typeof(Rads);

        protected override string Sign => "rads";

        protected override Angle Ctor(double val) => new Rads(val);

        public override Angle Complementary() => Degrees.Create(Value + Math.PI);
    }

    public class Grads : Angle
    {
        protected Grads(double val) : base(val)
        {
        }

        public static Angle Create(double val)
        {
            if (double.IsNaN(val)) return new Nan();
            return new Grads(val);
        }

        internal override double OneCycle => 400;

        internal override Type GetDerivedType => typeof(Grads);

        protected override string Sign => "grads";

        protected override Angle Ctor(double val) => new Grads(val);

        public override Angle Complementary() => Degrees.Create(Value + 200);
    }

    public class Cycle : Angle
    {
        protected Cycle(double val) : base(val)
        {
        }

        public static Angle Create(double val)
        {
            if (double.IsNaN(val)) return new Nan();
            return new Cycle(val);
        }

        internal override double OneCycle => 1;

        internal override Type GetDerivedType => typeof(Cycle);

        protected override string Sign => "cycles";

        protected override Angle Ctor(double val) => new Cycle(val);

        public override Angle Complementary() => Degrees.Create(Value + 0.5);
    }
}