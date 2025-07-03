using System;

namespace BuildingBlocks.Domain.Auditing;

public interface IModificationAuditedEntity
{
    DateTime? LastModificationTime { get; set; }
    Guid? LastModifierId { get; set; }
} 