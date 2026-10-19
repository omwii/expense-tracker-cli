using System.CommandLine;
using ExpenseTrackerCLI.Models;
using ExpenseTrackerCLI.Services;

namespace ExpenseTrackerCLI.Commands;

public class AddCommand(IExpenseService expenseService, TrackerConfig trackerConfig) : ICommand
{
    public Command Build()
    {
        var command = new Command("add", "Add a new expense");

        var amountArgument = new Argument<decimal>("amount");
        var descriptionOption = new Option<string>("--description", "-d");
        var categoryOption = new Option<string>("--category", "-c");

        amountArgument.Validators.Add(result =>
        {
            var amount = result.GetValueOrDefault<decimal>();
            if (amount <= 0)
                result.AddError("Must be greater than zero");
        });
        descriptionOption.Validators.Add(result =>
        {
            var description = result.GetValueOrDefault<string>();
            if (description.Length > 256)
                result.AddError("Description is too long");
        });
        categoryOption.Validators.Add(result =>
        {
            var category = result.GetValueOrDefault<string>();
            if (category.Length > 32)
                result.AddError("Category is too long");
        });
        
        command.Add(amountArgument);
        command.Add(descriptionOption);
        command.Add(categoryOption);
        
        command.SetAction(result =>
        {
            var amount = result.GetRequiredValue(amountArgument);
            var description = result.GetValue(descriptionOption);
            var category = result.GetValue(categoryOption);
            var creationAt = DateTime.Now;
            
            expenseService.Add(amount, description, category, creationAt);
            
            Console.WriteLine($"Successfully added expense:" +
                              $"\n- Amount: {amount}" +
                              $"\n- Category: {category}" +
                              $"\n- Description: {description}" +
                              $"\n- Date: {creationAt.ToLongDateString()}" +
                              $"\n- Time: {creationAt.ToShortTimeString()}");
            
            if (expenseService.Expenses
                    .Where(pair => pair.Value.CreatedAt.Month == DateTime.Now.Month)
                    .Sum(pair => pair.Value.Amount) > trackerConfig.Budget)
                Console.WriteLine("Warning! Monthly budget is exceeded");
        });
        
        return command;
    }
}