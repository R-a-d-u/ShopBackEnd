using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using ShopBackEnd.Migrations;

public class MigrationCode
{
    public static void RunMigrations(string connectionString)
    {
        EnsureDatabaseExists(connectionString);//genereaza un database daca nu exista in sql
        var serviceProvider = CreateServices(connectionString);
        using (var scope = serviceProvider.CreateScope())
        {
            UpdateDatabase(scope.ServiceProvider);
        }
    }

    private static IServiceProvider CreateServices(string connectionString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(RemoveValidColumnFromGoldHistory).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
    private static void EnsureDatabaseExists(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = builder.InitialCatalog;

        builder.InitialCatalog = "master";
        var masterConnectionString = builder.ToString();

        using (var connection = new SqlConnection(masterConnectionString))
        {
            connection.Open();

            var commandText = $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{databaseName}') CREATE DATABASE [{databaseName}]";
            using (var command = new SqlCommand(commandText, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}