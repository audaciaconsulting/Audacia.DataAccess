using System;
using System.Collections.Generic;

namespace Audacia.DataAccess.EntityFrameworkCore.Triggers
{
    public class TriggerTypeHash : IEquatable<TriggerTypeHash>
    {
        public TriggerType TriggerType { get; set; }
        public Type EntityType { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as TriggerTypeHash);
        }

        public bool Equals(TriggerTypeHash other)
        {
            return other != null &&
                   TriggerType == other.TriggerType &&
                   EqualityComparer<Type>.Default.Equals(EntityType, other.EntityType);
        }

        public override int GetHashCode()
        {
            var hashCode = -1203416947;
            hashCode = hashCode * -1521134295 + TriggerType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(EntityType);
            return hashCode;
        }

        public static bool operator ==(TriggerTypeHash hash1, TriggerTypeHash hash2)
        {
            return EqualityComparer<TriggerTypeHash>.Default.Equals(hash1, hash2);
        }

        public static bool operator !=(TriggerTypeHash hash1, TriggerTypeHash hash2)
        {
            return !(hash1 == hash2);
        }
    }
}