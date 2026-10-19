using ExpenseTrackerCLI.Models;

namespace ExpenseTrackerCLI.Services;

public interface IExpenseService
{
    Dictionary<Guid, Expense> Expenses { get; }
    void Add(decimal amount, string? description, string? category, DateTime createdAt);
    void Update(Guid id, decimal? amount, string? description, string? category);
    void Delete(Guid id);
}