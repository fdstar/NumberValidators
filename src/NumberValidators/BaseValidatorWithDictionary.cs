using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators
{
    /// <summary>
    /// 基于数据字典的验证基类
    /// </summary>
    /// <typeparam name="TResult">验证结果类型</typeparam>
    /// <typeparam name="TKey">字典Key</typeparam>
    /// <typeparam name="TValue">字典Value</typeparam>
    public abstract class BaseValidatorWithDictionary<TResult, TKey, TValue> : BaseValidator<TResult>
        where TResult : ValidationResult, new()
    {
        private IValidationDictionary<TKey, TValue> _dictionary;
        /// <summary>
        /// 默认字典类
        /// </summary>
        protected abstract IValidationDictionary<TKey, TValue> DefaultDictionary { get; }
        /// <summary>
        /// 获取字典类
        /// </summary>
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
