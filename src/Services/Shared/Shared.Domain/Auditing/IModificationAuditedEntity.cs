using System;

namespace QuizEngineMicroservices.Shared.Domain.Auditing;

public interface IModificationAuditedEntity
{
    DateTime? LastModificationTime { get; set; }
    Guid? LastModifierId { get; set; }
} 