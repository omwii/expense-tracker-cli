using System.CommandLine;
using ExpenseTrackerCLI.Services;
using ExpenseTrackerCLI.Utilities;

namespace ExpenseTrackerCLI.Commands;

public class ListCommand(IExpenseService expenseService) : ICommand
{
    public Command Build()
    {
        var command = new Command("list");
        
        var monthOption = new Option<string>("--month", "-m");
        
        monthOption.Validators.Add(result =>
        {
            var month = result.GetValueOrDefault<string>();
            if (!ValidatorUtility.ValidateMonth(month))
                result.AddError("The month is invalid.");
        });
        
        command.Add(monthOption);
        
        command.SetAction(result =>
        {
            var expenses = expenseService.Expenses;

            var month = result.GetValue(monthOption);

            if (month != null)
            {
                foreach (var (_, expense) in expenses)
                {
                    if (expense.CreatedAt.Month == ParseUtility.GetMonthNumber(month))
                    {
                        Console.WriteLine($"Expense {expense.Id}:" +
                                          $"\nAmount: {expense.Amount}" +
                                          $"\nCategory: {expense.Category}" +
                                          $"\nDescription: {expense.Description}" +
                                          $"\nCreation date: {expense.CreatedAt}" +
                                          $"\nLast modify date: {expense.ModifiedAt}\n\n");
                    }
                }
            }
            else
            {
                foreach (var (_, expense) in expenses)
                {
                    Console.WriteLine($"Expense {expense.Id}:" +
                                      $"\nAmount: {expense.Amount}" +
                                      $"\nCategory: {expense.Category}" +
                                      $"\nDescription: {expense.Description}" +
                                      $"\nCreation date: {expense.CreatedAt}" +
                                      $"\nLast modify date: {expense.ModifiedAt}\n\n");
                }
            }
            
            Console.WriteLine("Listed all expenses!");
        });
        
        return command;
    }
}