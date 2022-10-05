using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Client
    {
        [Column("ClientId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Client ID is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Client FIO is 60 characters.")]
        public string Client_FIO { get; set; }
    }
}
