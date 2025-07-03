using System;

namespace BuildingBlocks.Domain.Auditing;

public interface ICreationAuditedEntity 
{
    DateTime CreationTime { get; set; }
    Guid? CreatorId { get; set; }
} 