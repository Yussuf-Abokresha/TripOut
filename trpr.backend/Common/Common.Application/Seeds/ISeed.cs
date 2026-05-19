namespace Common.Application.Seeds
{
    public interface ISeed
    {
        public Task SeedAsync();
    }
}