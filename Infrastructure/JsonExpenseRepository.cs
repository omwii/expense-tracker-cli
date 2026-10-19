using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ExpenseTrackerCLI.Models;

namespace ExpenseTrackerCLI.Infrastructure;

public class JsonExpenseRepository : IExpenseRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    public IEnumerable<Expense> GetAll()
    {
        EnsureDirectoryExists();

        var expensesPaths = Directory.EnumerateFiles("Expenses");
        return [.. expensesPaths
            .Select(File.ReadAllText)
            .Select(file => JsonSerializer.Deserialize<Expense>(file))];
    }

    public bool TryGet(Guid id, out Expense expense)
    {
        EnsureDirectoryExists();

        var path = Path.Combine("Expenses", $"{id.ToString()}.json");
        
        expense = File.Exists(path)
            ? JsonSerializer.Deserialize<Expense>(File.ReadAllText(path),
                SerializerOptions)
            : default;
        
        return File.Exists(path);
    }

    public void Write(Expense expense)
    {
        EnsureDirectoryExists();

        var path = Path.Combine("Expenses", $"{expense.Id.ToString()}.json");

        var json = JsonSerializer.Serialize(expense, SerializerOptions);

        File.WriteAllText(path, json);
    }

    public void Delete(Guid id)
    {
        EnsureDirectoryExists();

        var path = Path.Combine("Expenses", $"{id.ToString()}.json");
        
        if (File.Exists(path))
            File.Delete(path);
    }

    private void EnsureDirectoryExists()
    {
        if (!Directory.Exists("Expenses"))
            Directory.CreateDirectory("Expenses");
    }
}