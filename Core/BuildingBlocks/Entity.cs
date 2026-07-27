using System;
using System.Collections.Generic;

namespace BuildingBlocks
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
        where TId : notnull
    {
        public TId Id { get; }

        protected Entity(TId id) => Id = id;

        public bool Equals(Entity<TId>? other)
        {
            if (other is null || GetType() != other.GetType())
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override bool Equals(object? obj) => Equals(obj as Entity<TId>);

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !(left == right);
    }
}
