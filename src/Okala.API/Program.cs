using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Okala.API.Attributes;
using Okala.API.Middlewares;
using Okala.Application.Interfaces.IRepositories;
using Okala.Application.Interfaces.IServices;
using Okala.Application.Services;
using Okala.Domain.Redis;
using Okala.Domain.Settings;
using Okala.Infrastructure.Repositories;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(o =>
{
    o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    o.Filters.Add(new ValidationFilterAttribute());
});
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddHttpClient();

// Services
builder.Services.AddScoped<ICryptoService, CryptoService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// redis
var redisConfiguration = builder.Configuration.GetSection("Redis")["ConnectionString"]!;
try
{
    var redis = ConnectionMultiplexer.Connect(redisConfiguration);
    builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
    builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
}
catch
{
    // ignored
}

// Infrastructure services
builder.Services.AddScoped<ICryptoPriceRepository, CoinMarketCapCryptoPriceRepository>();
builder.Services.AddScoped<IExchangeRateRepository, ERExchangeRateRepository>();

// Add seq and serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetSection("Seq:ServerUrl").Value!,
        apiKey: builder.Configuration.GetSection("Seq:ApiKey").Value!));

// Check appsetting currencies
string[] supportedCurrencies =
[
    "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF",
    "BMD", "BND", "BOB", "BRL", "BSD", "BTC", "BTN", "BWP", "BYR", "BZD", "CAD", "CDF", "CHF", "CLF", "CLP", "CNY",
    "COP", "CRC", "CUC", "CUP", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EEK", "EGP", "ERN", "ETB", "EUR", "FJD",
    "FKP", "GBP", "GEL", "GGP", "GHS", "GIP", "GMD", "GNF", "GTQ", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF", "IDR",
    "ILS", "IMP", "INR", "IQD", "IRR", "ISK", "JEP", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW",
    "KWD", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LTL", "LVL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK",
    "MNT", "MOP", "MRO", "MUR", "MVR", "MWK", "MXN", "MYR", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "OMR",
    "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RUB", "RWF", "SAR", "SBD", "SCR", "SDG",
    "SEK", "SGD", "SHP", "SLL", "SOS", "SRD", "STD", "SVC", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY",
    "TTD", "TWD", "TZS", "UAH", "UGX", "USD", "UYU", "UZS", "VEF", "VND", "VUV", "WST", "XAF", "XAG", "XAU", "XCD",
    "XDR", "XOF", "XPF", "YER", "ZAR", "ZMK", "ZMW", "ZWL"
];
var appsettingCurrencies = builder.Configuration.GetSection("Currencies").Get<List<string>>()!;
var unsupportedCurrencies = appsettingCurrencies.Where(ac => !supportedCurrencies.Contains(ac)).ToList();
if (unsupportedCurrencies.Any())
{
    throw new Exception(
        $"Please edit appsetting.json, because the {string.Join(", ", unsupportedCurrencies)} currencies are/is not supported.");
}


// settings
builder.Services.Configure<CurrencySetting>(option =>
{
    option.Currencies = builder.Configuration.GetSection("Currencies").Get<List<string>>()!;
});

builder.Services.Configure<ApiKeySetting>(builder.Configuration.GetSection("ApiKeys"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
