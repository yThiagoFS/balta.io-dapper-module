using Microsoft.Data.SqlClient;
using Dapper;
using modulo2dapper.Models;

const string connectionString = "Data Source=DESKTOP-DIFT32I\\SQLEXPRESS;Initial Catalog=Balta;Integrated Security=True; TrustServerCertificate=True";

var category = new Category();
category.Title = "Amazon AWS";
category.Url = "amazon";
category.Description = "Categoria destinada a serviços do AWS";
category.Order = 8;
category.Summary = "AWS Cloud";
category.Featured = true;


using(var connection = new SqlConnection(connectionString))
{
    // ListCategories(connection);
    // InsertCategory(connection);
    // DeleteStudentStoredProcedure(connection);
    // ReadProcedure(connection);
    // UsingExecuteScalar(connection, category);
    // ReadView(connection);
    // OneToOne(connection);
    // OneToMany(connection);
    QueryMultiple(connection);
}   


static void ListCategories(SqlConnection connection){

    var query = "SELECT * from Category";

    var categories = connection.Query<Category>(query);

    foreach(var item in categories)
    {
        foreach(var prop in item.GetType().GetProperties()){
            Console.WriteLine("");
            Console.WriteLine($" {prop.Name} =  {prop.GetValue(item)} -");
            Console.WriteLine("");
        }
    }

}

static void InsertCategory(SqlConnection connection, Category category){

    var insertSql = @$"INSERT INTO Category VALUES(
         @Id
        ,@Title
        ,@Url
        ,@Summary
        ,@Order
        ,@Description
        ,@Featured)";

    var rows = connection.Execute(insertSql, new 
            {
                Id = category.Id,
                Title = category.Title,
                Url = category.Url,
                Description = category.Description,
                Order = category.Order,
                Summary = category.Summary,
                Featured = category.Featured
            });

    Console.WriteLine($"Rows: {rows}");
}

static void DeleteStudentStoredProcedure(SqlConnection connection)
{   
    var result = connection.Execute("spDeleteStudent", new { StudentId = "5AFACB02-82EA-47C4-91EC-0B2EB98CFA33" }, commandType: System.Data.CommandType.StoredProcedure );

    Console.WriteLine($"Amount of rows affected: {result}");
}

static void ReadProcedure(SqlConnection  connection)
{
    var procedure = "spGetCoursesByCategory";
    var pars = new { CategoryId = "09CE0B7B-CFCA-497B-92C0-3290AD9D5142" };
    var dynObj = connection.Query(procedure, pars,commandType: System.Data.CommandType.StoredProcedure );

    foreach(var item in dynObj){
        Console.WriteLine($"{item.Id}");
    }
}

static void UsingExecuteScalar(SqlConnection connection, Category category)
{
    var insertSql = @$"INSERT INTO Category OUTPUT inserted.Id VALUES(
         NEWID()
        ,@Title
        ,@Url
        ,@Summary
        ,@Order
        ,@Description
        ,@Featured)";

    var idGenerated = connection.ExecuteScalar<Guid>(insertSql, new 
            {
                Title = category.Title,
                Url = category.Url,
                Description = category.Description,
                Order = category.Order,
                Summary = category.Summary,
                Featured = category.Featured
            });

    Console.WriteLine($"Id: {idGenerated}");
}

static void ReadView(SqlConnection connection)
{
    var query = "SELECT * FROM vwCourses";

    var courses = connection.Query(query);

    foreach(var item in courses)
    {
        Console.WriteLine($"{item.Id}, {item.Title}");
    }
}

static void OneToOne(SqlConnection connection)
{
    var query = @"SELECT 
                    *
                FROM 
                    CareerItem CI
                INNER JOIN 
                    Course CO ON CI.CourseId = CO.Id  ";

    var items = connection.Query<CareerItem, Course, CareerItem>(
        query,
        (carrerItem, course) => { 
            carrerItem.Course = course;
            return carrerItem;
        }, splitOn: "Id");

    foreach(var item in items)
    {
        Console.WriteLine($"{item.Title} = Curso: {item.Course.Title}");
    }
}

static void OneToMany(SqlConnection connection)
{
     var query = @"SELECT
                        CAR.Id
                        ,CAR.Title
                        ,CI.CareerId
                        ,CI.Title
                    FROM 
                        Career CAR
                    INNER JOIN 
                        CareerItem CI ON CI.CareerId = CAR.Id
                    ORDER BY 
                        CAR.Title ";

    var careers = new List<Career>();
                                //Query<Item, populado por:, resultado final>
     var items = connection.Query<Career, CareerItem, Career>(
        query,
        (career, item) =>
        {
            var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();

            if(car == null)
            {
                car = career;
                car.Items.Add(item);
                careers.Add(car);
            }
            else
            {
                car.Items.Add(item);
            }

            return career;
        }, splitOn: "CareerId");

        foreach(var career in careers)
        {
            Console.WriteLine($"{career.Title}");
            foreach(var item in career.Items)
            {
                Console.WriteLine($"{item.Title}");
            }
        }
}

static void QueryMultiple(SqlConnection connection)
{
    var query = "SELECT * FROM Category; SELECT * FROM Course";

    using(var multi = connection.QueryMultiple(query))
    {
        var categories = multi.Read<Category>();
        var courses = multi.Read<Course>();

        foreach(var item in categories)
        {
            Console.WriteLine(item.Title);
        }

        foreach(var item in courses)
        {
            Console.WriteLine(item.Title);
        }
    }
}

