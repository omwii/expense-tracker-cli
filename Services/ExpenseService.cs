using ExpenseTrackerCLI.Infrastructure;
using ExpenseTrackerCLI.Models;

namespace ExpenseTrackerCLI.Services;

public class ExpenseService(IExpenseRepository repository) : IExpenseService
{
    public Dictionary<Guid, Expense> Expenses { get; } = repository.GetAll().ToDictionary(x => x.Id);

    public void Add(decimal amount, string? description, string? category, DateTime createdAt)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var expense = new Expense(Guid.NewGuid(), amount, description, category, createdAt);

        try
        {
            Expenses.Add(expense.Id, expense);
            repository.Write(expense);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            throw;
        }
    }

    public void Update(Guid id, decimal? amount, string? description, string? category)
    {
        if (!Expenses.TryGetValue(id, out var expense)) throw new KeyNotFoundException();
        
        expense.Amount = amount ??  expense.Amount;
        expense.Description = description ??  expense.Description;
        expense.Category = category ??  expense.Category;
        expense.ModifiedAt = DateTime.Now;
        
        repository.Write(expense);
    }

    public void Delete(Guid id)
    {
        Expenses.Remove(id);
        repository.Delete(id);
    }
}