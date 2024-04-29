using Microsoft.Extensions.Configuration;
using App;

string connectionString = GetConnectionString();

using (var context = new ApplicationContext(connectionString, true))
{
    //Student student = context.Students.Find(1L);

    Student? student = context.Students.SingleOrDefault(x => x.Id == 1);
}

string GetConnectionString()
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return configuration["ConnectionString"]!;
}
