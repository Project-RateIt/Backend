using System.Data;
using System.Globalization;
using Npgsql;
using rateit.Interfaces;

namespace rateit.Services;

public class SqlManager : ISqlManager
{
    string _cstring = new(System.IO.File.ReadAllTextAsync("SqlConnection").Result);

    
    
    //public async Task<List<Dictionary<string, dynamic>>> Reader(string query)
    //{
    //    try
    //    {
    //        if (_connection.State != ConnectionState.Open)
    //        {
    //            await _connection.OpenAsync();
    //        }
    //        
    //        NpgsqlCommand command = new NpgsqlCommand(query, _connection);
//
    //        var reader = await command.ExecuteReaderAsync();
//
    //        List<Dictionary<string, dynamic>> result = new List<Dictionary<string, dynamic>>();
    //        while (await reader.ReadAsync())
    //        {
    //            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
//
    //            for (int i = 0; i < reader.FieldCount; i++)
    //            {
    //                var filedName = reader.GetName(i);
    //                var value = reader.GetValue(i);
//
    //                data.Add(filedName, value);
    //            }
//
    //            result.Add(data);
    //        }
//
    //        await _connection.CloseAsync();
//
    //        return result;
    //    }
    //    finally
    //    {
    //        if (_connection.State != ConnectionState.Closed)
    //            await _connection.CloseAsync();
    //        NpgsqlConnection.ClearPool(_connection);
    //    }
    //}
    
    public async Task<List<Dictionary<string, dynamic>>> Reader(string sql)
    {
        NpgsqlConnection connection = new NpgsqlConnection(_cstring);
        List<Dictionary<string, dynamic>> result = new();

        try
        {
            NpgsqlCommand command = new(sql, connection);

            await connection.OpenAsync();

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Dictionary<string, dynamic> data = new();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    string currentName = reader.GetName(i);
                    dynamic currentValue = reader.GetValue(i);

                    data.Add(currentName, currentValue);
                }

                result.Add(data);
            }

            await connection.CloseAsync();

            if (connection.State != ConnectionState.Closed)
            {
                Console.WriteLine("err sql " + connection.State + " " + connection.ConnectionString);
            }

                
        }
        catch (Exception e)
        {
            Console.WriteLine("ex sql " + e.Message);
            throw;
        }
        finally
        {
            await connection.DisposeAsync();
        }
            
        return result;
    }    
    
    public async Task<bool> IsValueExist(string sql)
    {
        NpgsqlConnection connection = new NpgsqlConnection(_cstring);
        List<Dictionary<string, dynamic>> result = new();
        bool haveRows;
        try
        {
            NpgsqlCommand command = new(sql, connection);

            await connection.OpenAsync();

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();

            haveRows = reader.HasRows;
            
            await connection.CloseAsync();

            if (connection.State != ConnectionState.Closed)
            {
                Console.WriteLine("err sql " + connection.State + " " + connection.ConnectionString);
            }

                
        }
        catch (Exception e)
        {
            Console.WriteLine("ex sql " + e.Message);
            throw;
        }
        finally
        {
            await connection.DisposeAsync();
        }
            
        return haveRows;
    }

    //public async Task Execute(string query)
    //{
    //    var command = new NpgsqlCommand(query, _connection);
//
    //    try
    //    {
    //        if (_connection.State != ConnectionState.Open)
    //        {
    //            await _connection.OpenAsync();
    //        }
//
    //        await command.ExecuteNonQueryAsync();
//
    //        await _connection.CloseAsync();
    //    }
    //    finally
    //    {
    //        if (_connection.State != ConnectionState.Closed)
    //            await _connection.CloseAsync();
    //        NpgsqlConnection.ClearPool(_connection);
    //    }
    //}
    public async Task Execute(string sql)
    {
        NpgsqlConnection connection = new NpgsqlConnection(_cstring);

        try
        {
            NpgsqlCommand command = new (sql, connection);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
                

            if (connection.State != ConnectionState.Closed)
            {
                Console.WriteLine("err sql " + connection.State + " " + connection.ConnectionString);

            }
        }
        catch (Exception e)
        {
            Console.WriteLine ("ex sql " + e.Message);
        }
        finally
        {
            await connection.DisposeAsync();
        }
    }
    //public async Task<bool> IsValueExist(string query)
    //{
    //    try
    //    {
    //        if (_connection.State != ConnectionState.Open)
    //        {
    //            await _connection.OpenAsync();
    //        }
//
    //        NpgsqlCommand command = new NpgsqlCommand(query, _connection);
//
    //        var reader = await command.ExecuteReaderAsync();
//
    //        bool result = reader.HasRows;
//
    //        await _connection.CloseAsync();
    //        return result;
    //    }
    //    finally
    //    {
    //        if (_connection.State != ConnectionState.Closed)
    //            await _connection.CloseAsync();
    //        NpgsqlConnection.ClearPool(_connection);
    //    }
    //}
}