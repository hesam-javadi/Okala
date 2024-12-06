using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okala.Domain.DTOs;
using Okala.Domain.Request;
using Okala.Domain.Response;

namespace Okala.Application.Interfaces.IServices
{
    public interface ICryptoService
    {
        Task<DataResponse<CryptoQuoteResponse>> GetCryptoQuoteAsync(CryptoQuoteRequest request);
    }
}
