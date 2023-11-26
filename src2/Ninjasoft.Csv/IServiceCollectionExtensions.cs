using Microsoft.Extensions.DependencyInjection;

namespace Ninjasoft.Csv
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCsv(this IServiceCollection services)
        {
            return services.AddScoped<ICsvReader, CsvReader>()
                .AddScoped<ICsvCreator, ICsvCreator>();
        }
    }
}
