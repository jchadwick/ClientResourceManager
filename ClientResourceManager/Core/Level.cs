using System;

namespace ClientResourceManager
{
    public class Level : IComparable<Level>
    {
        public static readonly Level Global = new Level(100);
        public static readonly Level MidLevel = new Level(75);
        public static readonly Level Component = new Level(50);
        public static readonly Level Loose = new Level(0);

        public int Value { get; private set; }


        public Level(int value)
        {
            Value = value;
        }


        public int CompareTo(Level other)
        {
            if(other == null)
                return Value.CompareTo(0);

            return other.Value.CompareTo(Value);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Level other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static implicit operator int?(Level value)
        {
            if (value == null)
                return null;

            return value.Value;
        }

        public static implicit operator Level(int value)
        {
            return new Level(value);
        }

    }
}