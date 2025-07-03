namespace BuildingBlocks.Domain;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}