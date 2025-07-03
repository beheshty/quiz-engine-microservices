namespace BuildingBlocks.Domain;

public interface IHasConcurrencyStamp
{
    public byte[] ConcurrencyStamp { get; set; }

}