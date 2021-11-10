namespace BusinessLogic.Infrastructure
{
    using System;

    public class Maybe<TValue>
    {
        private bool _hasValue;
        private TValue _value;

        public Maybe()
        {
            _hasValue = false;
        }
        public Maybe(TValue value)
        {
            _hasValue = true;
            _value = value;
        }
        public Maybe<TReturn> Select<TReturn>(Func<TValue, TReturn> selector)
        {
            if (_hasValue)
            {
                return selector(_value);
            }
            return new Maybe<TReturn>();
        }
        public Maybe<TReturn> Select<TReturn>(Func<TValue, Maybe<TReturn>> selector)
        {
            if (_hasValue)
            {
                return selector(_value);
            }
            return new Maybe<TReturn>();
        }
        public static implicit operator Maybe<TValue>(TValue value) => new Maybe<TValue>(value);
    }
}