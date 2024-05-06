using System.ComponentModel.DataAnnotations;

namespace Shared.Core.Domain.Entities
{
    public abstract class EntityBase : EntityBase<int>, IEntity<int>
    {

    }

    public abstract class EntityBase<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        [Key]
        public virtual TPrimaryKey Id { get; set; }
    }
}