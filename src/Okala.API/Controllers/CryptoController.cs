using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Okala.Domain.Response;
using System.Security.Claims;
using Okala.Application.Interfaces.IServices;
using Okala.Domain.DTOs;
using Okala.Domain.Request;

namespace Okala.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public class CryptoController(ICryptoService cryptoService) : ControllerBase
    {
        // GET: api/Crypto/GetCryptoQuote
        [HttpGet("GetCryptoQuote")]
        [ProducesResponseType(typeof(DataResponse<CryptoQuoteResponse>), 200)]
        public async Task<IActionResult> GetCryptoQuote([FromQuery] CryptoQuoteRequest request)
        {
            return Ok(await cryptoService.GetCryptoQuoteAsync(request));
        }
    }
}
