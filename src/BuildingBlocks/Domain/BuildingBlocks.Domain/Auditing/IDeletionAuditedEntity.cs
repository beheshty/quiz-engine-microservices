using System;

namespace BuildingBlocks.Domain.Auditing;

public interface IDeletionAuditedEntity
{
    Guid? DeleterId { get; set; }
    DateTime? DeletionTime { get; set; }
} 