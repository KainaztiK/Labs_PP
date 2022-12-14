using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public abstract class ClientForManipulationDto
    {
        [Required(ErrorMessage = "Client name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Number_Client  is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Number_Client is 30 characters.")]
        public string Number_Client { get; set; }
        [Required(ErrorMessage = "Company address is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Address is 100 characters.")]
        public string Address { get; set; }
    }
}
