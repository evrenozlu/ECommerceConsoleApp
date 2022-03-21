using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Base
{
    public class BaseDomainEvent : INotification
    {
        [Key]
        public int Id { get; set; } 
        public BaseDomainEvent()
        {
            EventId = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
        }

        public virtual Guid EventId { get; init; }
        public virtual DateTime CreatedOn { get; init; }
    }
}