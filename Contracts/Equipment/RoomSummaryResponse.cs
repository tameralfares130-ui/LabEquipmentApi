namespace LabEquipmentApi.Contracts.Equipment;

public class RoomSummaryResponse
{
    public required string Room { get; set; }
    public int TotalCount { get; set; }
    public int AvailableCount { get; set; }
}
