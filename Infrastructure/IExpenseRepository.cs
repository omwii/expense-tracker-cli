using ExpenseTrackerCLI.Models;

namespace ExpenseTrackerCLI.Infrastructure;

public interface IExpenseRepository
{ 
    IEnumerable<Expense> GetAll();
    bool TryGet(Guid id, out Expense expense);
    void Write(Expense expense);
    void Delete(Guid id);
}