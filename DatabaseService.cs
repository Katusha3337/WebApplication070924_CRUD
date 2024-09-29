using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Create(string tableName, Dictionary<string, object> data)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var columns = string.Join(", ", data.Keys);
        var values = string.Join(", ", data.Keys.Select(k => "@" + k));
        var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
        using var command = new SqlCommand(sql, connection);
        foreach (var kvp in data)
        {
            command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
        }
        command.ExecuteNonQuery();
    }

    public IEnumerable<Dictionary<string, object>> Read(string tableName, string condition = "1=1")
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var sql = $"SELECT * FROM {tableName} WHERE {condition}";
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        var results = new List<Dictionary<string, object>>();
        while (reader.Read())
        {
            var row = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row[reader.GetName(i)] = reader.GetValue(i);
            }
            results.Add(row);
        }
        return results;
    }

    public void Update(string tableName, Dictionary<string, object> data, string condition)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var setClause = string.Join(", ", data.Keys.Select(k => $"{k} = @{k}"));
        var sql = $"UPDATE {tableName} SET {setClause} WHERE {condition}";
        using var command = new SqlCommand(sql, connection);
        foreach (var kvp in data)
        {
            command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
        }
        command.ExecuteNonQuery();
    }

    public void Delete(string tableName, string condition)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var sql = $"DELETE FROM {tableName} WHERE {condition}";
        using var command = new SqlCommand(sql, connection);
        command.ExecuteNonQuery();
    }
}