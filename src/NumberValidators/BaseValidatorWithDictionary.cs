using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators
{
    public abstract class BaseValidatorWithDictionary<TResult, TKey, TValue> : BaseValidator<TResult>
        where TResult : ValidationResult, new()
    {
        private IValidationDictionary<TKey, TValue> _dictionary;
        protected abstract IValidationDictionary<TKey, TValue> DefaultDictionary { get; }
        public IValidationDictionary<TKey, TValue> Dictionary
        {
            get
            {
                return this._dictionary ?? DefaultDictionary;
            }
            set
            {
                this._dictionary = value;
            }
        }
    }
}
