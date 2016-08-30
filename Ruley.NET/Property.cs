using System;

namespace Ruley.NET
{
    public class Property<T>
    {
        private object _value;
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _getter = new TemplatedPropertyGetter(_ctx, value);
            }
        }

        private TemplatedPropertyGetter _getter;
        private Context _ctx;

        public Property()
        {
        }

        public Property(Context ctx)
        {
            _ctx = ctx;
        }

        public Property(Context ctx, object value)
        {
            Value = value;

            if (value is string && typeof(T) == typeof(long))
            {
                long l;
                if (long.TryParse((string)value, out l))
                {
                    Value = l;
                }
            }

            if (value is string && typeof(T) == typeof(TimeSpan))
            {
                Value = TimeSpanDeserializer.Deserialize((string)value);
            }

            _getter = new TemplatedPropertyGetter(ctx, Value);
        }

        //public static implicit operator string(Property<T> d)
        //{
        //    throw new Exception("Implicit cast not allowed, use Get() method");
        //}

        //public static implicit operator Property<T>(string d)
        //{
           

        //    return new Property<T>(_ctx, d);
        //}

        //public static implicit operator long(Property<T> d)
        //{
        //    throw new Exception("Invalid cast");
        //}

        //public static implicit operator Property<T>(long d)
        //{
        //    return new Property<T>(_ctx, d);
        //}

        //public static implicit operator int(Property<T> d)
        //{
        //    throw new Exception("Invalid cast");
        //}

        //public static implicit operator Property<T>(int d)
        //{
        //    return new Property<T>(_ctx, d);
        //}

        //public static implicit operator double(Property<T> d)
        //{
        //    throw new Exception("Invalid cast");
        //}

        //public static implicit operator Property<T>(double d)
        //{
        //    return new Property<T>(_ctx, d);
        //}

        //public static implicit operator bool(Property<T> d)
        //{
        //    return false;
        //}

        //public static implicit operator Property<T>(bool d)
        //{
        //    return new Property<T>(_ctx, d);
        //}

        //public static implicit operator Event(Property<T> d)
        //{
        //    throw new Exception("Invalid cast");
        //}

        //public static implicit operator Property<T>(Event d)
        //{
        //    return new Property<T>(_ctx, d);
        //}

        public T Get(Event @event)
        {
            return (T)_getter.GetValue(Value, @event);
        }
    }
}