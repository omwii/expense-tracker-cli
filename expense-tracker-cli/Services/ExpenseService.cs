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
        if (!File.Exists(FileName)) return;
        var file = File.ReadAllText(FileName);
        var json = JsonSerializer.Deserialize<List<Expense>>(file, _serializerOptions);
        if (json != null)
        {
            _expenses.Clear();
            foreach (var expense in json)
                _expenses.Add(expense.Id, expense);
        }
        else
            throw new FileLoadException();
    }
    
    public void AddExpense(string description, int amount)
    {
        var id = 1;
        if (_expenses.Count > 0)
            id = _expenses.Values.Select(x => x.Id).Max() + 1;
        var expense = new Expense(id, description, amount, DateTime.Now);
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
        // Find longest column elements for dynamic padding
        var idColumnElements = _expenses.Values.Select(x => x.Id.ToString(CultureInfo.InvariantCulture)).Append("Id");
        var longestIdLength = idColumnElements.Max(x => x.Length);
        var descriptionColumnElements = _expenses.Values.Select(x => x.Description).Append("Description");
        var longestDescriptionLength = descriptionColumnElements.Max(x => x.Length);
        var amountColumnElements = _expenses.Values.Select(x => x.Amount.ToString(CultureInfo.InvariantCulture))
            .Append("Amount");
        var longestAmountLength = amountColumnElements.Max(x => x.Length);

        // Add table header with dynamic padding
        var s = $"{"Id".PadRight(longestIdLength)} " +
                $"{"Description".PadRight(longestDescriptionLength)} " +
                $"{"Amount".PadRight(longestAmountLength)} " +
                $"{"Date",-9}\n";

        // Add table elements with dynamic padding
        s = _expenses.Values.Select(expense =>
                $"{expense.Id.ToString(CultureInfo.InvariantCulture).PadRight(longestIdLength)} " +
                $"{expense.Description.PadRight(longestDescriptionLength)} " +
                $"{expense.Amount.ToString(CultureInfo.InvariantCulture).PadRight(longestAmountLength)} " +
                $"{expense.Date,-9:yy-MM-dd}\n")
            .Aggregate(s, (current, line) => current + line);

        Console.WriteLine(s);
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