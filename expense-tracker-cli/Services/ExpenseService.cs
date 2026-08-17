using System.Globalization;
using System.Text.Json;
using expense_tracker_cli.Interfaces;
using expense_tracker_cli.Models;

namespace expense_tracker_cli.Services;

public class ExpenseService : IExpenseService
{
    private const string FileName = "expenses.json";
    private readonly Dictionary<int, Expense> _expenses = new();
    private readonly JsonSerializerOptions _serializerOptions = new() { WriteIndented = true };

    public void LoadExpenses()
    {
        if (File.Exists(FileName))
        {
            var file = File.ReadAllText(FileName);
            var json = JsonSerializer.Deserialize<List<Expense>>(file, _serializerOptions);
            if (json != null)
            {
                _expenses.Clear();
                foreach (var expense in json)
                {
                    _expenses.Add(expense.Id, expense);
                }
            }
            else
                throw new FileLoadException();
        }
        else
            File.Create(FileName);
    }
    
    public void AddExpense(string description, int amount)
    {
        var id = _expenses.Values.Select(x => x.Id).Max() + 1;
        var expense = new Expense(id, description, amount);
        _expenses.Add(id, expense);
        
        UpdateFile();
    }

    public void UpdateExpense(int id, string description)
    {
        if (_expenses.TryGetValue(id, out var expense))
        {
            expense.Description = description;
            UpdateFile();
        }
        else
            throw new KeyNotFoundException();
    }

    public void DeleteExpense(int id)
    {
        if (_expenses.Remove(id, out _))
            UpdateFile();
        else
            throw new KeyNotFoundException();
    }

    public void ListExpenses()
    {

    }

    public void SummaryExpenses(int month = 0)
    {
        switch (month)
        {
            case 0:
            {
                var expenses = _expenses.Values;
                Console.WriteLine($"Total expenses: {expenses.Sum(x => x.Amount)}");
                break;
            }
            case >= 1 and <= 12:
            {
                var expenses = _expenses.Values.Where(expense => expense.Date.Month == month);
                Console.WriteLine($"Total expenses in {month}: {expenses.Sum(x => x.Amount)}");
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(month));
        }
    }

    private void UpdateFile()
    {
        var json = JsonSerializer.Serialize(_expenses.Values.ToList(), _serializerOptions);
        File.WriteAllText(FileName, json);
    }
}