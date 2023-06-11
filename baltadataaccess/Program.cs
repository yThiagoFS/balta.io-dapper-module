using System.Data;
using System.Data.SqlClient;
using Microsoft.Data;
using Microsoft.Data.SqlClient;

const string connectionString = "Data Source=DESKTOP-DIFT32I\\SQLEXPRESS;Initial Catalog=Balta;Integrated Security=True; TrustServerCertificate=True";

using(var connection = new SqlConnection(connectionString))
{
    connection.Open();
    using (var command = new SqlCommand())
    {
        command.Connection = connection;
        command.CommandType = System.Data.CommandType.Text;
        command.CommandText = "SELECT Id, Title FROM Category";

        var reader = command.ExecuteReader();

        while(reader.Read()){
            ReadSingleRow((IDataRecord) reader);
            /*
                Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)}");
            */
        }

        reader.Close();
    }

}

static void ReadSingleRow(IDataRecord dRecord){
    Console.WriteLine(String.Format("{0}, {1}", dRecord[0], dRecord[1]));
}