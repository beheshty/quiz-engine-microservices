using System;

namespace QuizEngineMicroservices.Shared.Domain.Auditing;

public interface ICreationAuditedEntity 
{
    DateTime CreationTime { get; set; }
    Guid? CreatorId { get; set; }
} 