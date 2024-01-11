using Rpnw.Domain.Enum;
using Rpnw.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Infrastructure.Repository.Model
{
    [Table("AuditEntity")]
    public class AuditEntity : ModelEntity
    {
        [Column("Timestamp")]
        public string Timestamp { get; set; }

        [Column("Message")]
        public string Message { get; set; }
        
    }
}
