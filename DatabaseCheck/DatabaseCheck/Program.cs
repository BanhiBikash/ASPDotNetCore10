using Microsoft.Data.SqlClient;

var csb = new SqlConnectionStringBuilder
{
    DataSource = "amazonserver12353.database.windows.net,1433",
    InitialCatalog = "AmazonDatabase",
    UserID = "CloudSA8609abb9",
    Password = "Qwerty123@",
    Encrypt = true,
    TrustServerCertificate = false
};

Console.WriteLine(csb.ConnectionString);
string DefaultConnection = "Server=tcp:amazonserver12353.database.windows.net,1433;Initial Catalog=AmazonDatabase;Persist Security Info=False;User ID=CloudSA8609abb9;Password={Qwerty123@};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

using var connection = new SqlConnection(DefaultConnection);

connection.Open();

Console.WriteLine("Connected");
Console.ReadLine();