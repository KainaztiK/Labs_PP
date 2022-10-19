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
        //public Guid Id { get; set; }
        //[Required(ErrorMessage = "Client ID is a required field.")]
        //[MaxLength(60, ErrorMessage = "Maximum length for the Client FIO is 60 characters.")]
        //public string Client_FIO { get; set; }
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Client name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Number Client is a required field.")]
        public string Number_Client { get; set; }
        [Required(ErrorMessage = "Position is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Address is 100 characters.")]
        public string Address { get; set; }
    }
}
