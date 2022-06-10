using System.Data;
using Npgsql;
using rateit.Interfaces;

namespace rateit.Services;

public class SqlManager : ISqlManager
{
    private readonly NpgsqlConnection _connection = new(System.IO.File.ReadAllTextAsync("SqlConnection").Result);

    public async Task<List<Dictionary<string, dynamic>>> Reader(string query)
    {
        try
        {
            await _connection.OpenAsync();
            NpgsqlCommand command = new NpgsqlCommand(query, _connection);

            var reader = await command.ExecuteReaderAsync();

            List<Dictionary<string, dynamic>> result = new List<Dictionary<string, dynamic>>();
            while (await reader.ReadAsync())
            {
                Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var filedName = reader.GetName(i);
                    var value = reader.GetValue(i);

                    data.Add(filedName, value);
                }

                result.Add(data);
            }

            await _connection.CloseAsync();

            return result;
        }
        finally
        {
            if (_connection.State != ConnectionState.Closed)
                await _connection.CloseAsync();
        }
    }

    public async Task Execute(string query)
    {
        var command = new NpgsqlCommand(query, _connection);

        try
        {
            await _connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        finally
        {
            if (_connection.State != ConnectionState.Closed)
                await _connection.CloseAsync();
        }
    }

    public async Task<bool> IsValueExist(string query)
    {
        try
        {
            await _connection.OpenAsync();
            NpgsqlCommand command = new NpgsqlCommand(query, _connection);

            var reader = await command.ExecuteReaderAsync();

            bool result = reader.HasRows;

            await _connection.CloseAsync();
            return result;
        }
        finally
        {
            if (_connection.State != ConnectionState.Closed)
                await _connection.CloseAsync();
        }
    }
}