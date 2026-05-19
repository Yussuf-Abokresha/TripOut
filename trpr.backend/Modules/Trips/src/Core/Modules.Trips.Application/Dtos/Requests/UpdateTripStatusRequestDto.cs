namespace Modules.Trips.Application.Dtos.Requests
{
    public class UpdateTripStatusRequestDto
    {
        public Guid Id { get; set; }
        public bool IsApproved { get; set; } = true;
        public string? RejectionReason { get; set; }
    }
}
