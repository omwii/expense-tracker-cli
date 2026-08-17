using System.CommandLine;
using expense_tracker_cli.Interfaces;
using expense_tracker_cli.Services;

IExpenseService expenseService = new ExpenseService();

expenseService.LoadExpenses();

var addCommand = new Command("add", "Expense add command");
var deleteCommand = new Command("delete", "Expense delete command");
var updateCommand = new Command("update", "Expense update command");
var listCommand = new Command("list", "Expense list command");
var summaryCommand = new Command("summary", "Expense summary command");

Option<string> expenseDescriptionOption = new ("--description", "-d")
{
    Description = "Description of the expense"
};
Option<int> expenseAmountOption = new ("--amount", "-a")
{
    Description = "Amount of the expense"
};
Option<int> expenseIdOption = new("--id", "-i")
{
    Description = "ID of the expense"
};
Option<int> expenseMonthOption = new("--month", "-m")
{
    Description = "Month of the expense"
};

expenseAmountOption.Validators.Add(result =>
{
    if (result.GetValueOrDefault<int>() < 0)
    {
        result.AddError("Amount must be non-negative");
    }
});
expenseIdOption.Validators.Add(result =>
{
    if (result.GetValueOrDefault<int>() < 0)
    {
        result.AddError("ID must be non-negative");
    }
});
expenseMonthOption.Validators.Add(result =>
{
    if (result.GetValueOrDefault<int>() > 12 || result.GetValueOrDefault<int>() < 1)
    {
        result.AddError("Month must be between 1 and 12");
    }
});

addCommand.Add(expenseDescriptionOption);
addCommand.Add(expenseAmountOption);
deleteCommand.Add(expenseIdOption);
updateCommand.Add(expenseIdOption);
updateCommand.Add(expenseDescriptionOption);
summaryCommand.Add(expenseMonthOption);

addCommand.SetAction(result =>
{
    expenseService.AddExpense(result.GetRequiredValue(expenseDescriptionOption),
        result.GetRequiredValue(expenseAmountOption));
});
deleteCommand.SetAction(result =>
{
    expenseService.DeleteExpense(result.GetRequiredValue(expenseIdOption));
});
updateCommand.SetAction(result => 
{
    expenseService.UpdateExpense(result.GetRequiredValue(expenseIdOption),
        result.GetRequiredValue(expenseDescriptionOption));
});
listCommand.SetAction(_ =>
{
    expenseService.ListExpenses();
});
summaryCommand.SetAction(result =>
{
    expenseService.SummaryExpenses(result.GetValue(expenseMonthOption));
});