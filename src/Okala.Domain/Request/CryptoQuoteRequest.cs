using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okala.Domain.Request
{
    public class CryptoQuoteRequest
    {
        [Required(ErrorMessage = "{0} is required!")]
        [RegularExpression("^[A-Za-z0-9]{2,10}$",
            ErrorMessage = "The cryptocurrency symbol is not in a correct format.")]
        public string CryptoSymbol { get; set; } = null!;
    }
}
