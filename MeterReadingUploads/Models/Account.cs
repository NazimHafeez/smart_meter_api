using MeterReadingApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MeterReadingUploads.Models
{
    public class Account: EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Navigation property
        public ICollection<MeterReading> MeterReadings { get; set; }
    }
}
