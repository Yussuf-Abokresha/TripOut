using Modules.Trips.Domain.Abstractions;

namespace Modules.Trips.Domain.Entities
{
    public class CsvSeedHistory : Entity
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public DateTime SeededAt { get; set; } = DateTime.UtcNow;
        public static CsvSeedHistory Create(string fileName)
        {
            return new CsvSeedHistory
            {
                FileName = fileName,
                SeededAt = DateTime.UtcNow
            };
        }
    }


}