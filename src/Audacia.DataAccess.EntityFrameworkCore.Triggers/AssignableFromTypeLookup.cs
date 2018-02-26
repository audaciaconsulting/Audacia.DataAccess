using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public static class DictionaryExtensions
    {
        //TODO: Move to Audacia.Core or a new Audacia.Reflection
        public static IEnumerable<TValue> GetValuesForTypeChain<TValue>(this IDictionary<Type, TValue> dictionary, Type key)
        {

        }
    }
}