using Data;

namespace Api.IoC
{
  public static class MapServices
  {
    public static void Map(IServiceCollection services)
    {
      services.AddScoped(typeof(IDocumentRepository<>), typeof(DocumentRepository<>));
      services.AddScoped(typeof(Services.IDataService), typeof(Services.DataService));
      services.AddScoped(typeof(Services.IRequestValidationService), typeof(Services.RequestValidationService));

      services.AddAutoMapper(typeof(AutoMapping), typeof(AutoMapping));
    }
  }
}
