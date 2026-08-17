namespace expense_tracker_cli.Interfaces;

public interface IExpenseService
{
    void LoadExpenses();
    void AddExpense(string description, int amount);
    void UpdateExpense(int id, string description);
    void DeleteExpense(int id);
    void ListExpenses();
    void SummaryExpenses(int month = 0);
}