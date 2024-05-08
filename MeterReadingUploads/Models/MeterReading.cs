using MeterReadingUploads.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeterReadingApi.Models
{
    public class MeterReading : EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MeterReadingID { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; } = string.Empty;

        // Foreign Key
        public int AccountID { get; set; }
        public Account Account { get; set; }
    }
}
