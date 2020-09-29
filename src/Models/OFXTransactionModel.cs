using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace src.Models
{
    [Table("OFXTransactionModels")]
    public class OFXTransactionModel
    {
        [Key]
        [IgnoreDataMemberAttribute]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set; }
        public string TransactiontType { get; set; }
        public DateTime PostedDate { get; set; }
        public string TransactionAmount { get; set; }
        public string Description { get; set; }
    }
}
