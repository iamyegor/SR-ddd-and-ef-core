using App;
using Microsoft.Extensions.Configuration;

string result3 = Execute(x => x.DisenrollStudent(1, 2));
string result = Execute(x => x.CheckStudentFavoriteCourse(1, 2));
string result2 = Execute(x => x.EnrollStudent(1, 2, Grade.A));

string Execute(Func<StudentController, string> func)
{
    string connectionString = GetConnectionString();

    using (var context = new SchoolContext(connectionString, true))
    {
        var controller = new StudentController(context);
        return func(controller);
    }
}

string GetConnectionString()
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return configuration["ConnectionString"]!;
}
