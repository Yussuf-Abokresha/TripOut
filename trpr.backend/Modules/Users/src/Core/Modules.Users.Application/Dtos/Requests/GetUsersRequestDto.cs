namespace Modules.Users.Application.Dtos.Requests
{
    public class GetUsersRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
    }
}
