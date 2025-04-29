using System;

namespace QuizEngineMicroservices.Shared.Domain.Auditing;

public interface IDeletionAuditedEntity
{
    Guid? DeleterId { get; set; }
    DateTime? DeletionTime { get; set; }
} 