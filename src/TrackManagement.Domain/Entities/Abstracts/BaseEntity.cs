using System;
using System.Collections.Generic;
using System.Text;

namespace TrackManagement.Domain.Entities.Abstracts;

public interface IBaseEntity;

public abstract class BaseEntity<TKey> : IBaseEntity
    where TKey : struct
{
    public TKey Id { get; set; }
}
