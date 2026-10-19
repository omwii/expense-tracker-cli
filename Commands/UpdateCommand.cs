using System.CommandLine;
using ExpenseTrackerCLI.Services;

namespace ExpenseTrackerCLI.Commands;

public class UpdateCommand(IExpenseService expenseService) : ICommand
{
    public Command Build()
    {
        var command = new Command("update", "Updates an existing expense");

        var idArgument = new Argument<Guid>("id");
        var amountOption = new Option<decimal>("--amount", "-a");
        var descriptionOption = new Option<string>("--description", "-d");
        var categoryOption = new Option<string>("--category", "-c");

        amountOption.Validators.Add(result =>
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
        
        command.Add(idArgument);
        command.Add(amountOption);
        command.Add(descriptionOption);
        command.Add(categoryOption);
        
        command.SetAction(result =>
        {
            var id = result.GetRequiredValue(idArgument);
            var amount = result.GetValue(amountOption);
            var description = result.GetValue(descriptionOption);
            var category = result.GetValue(categoryOption);
            
            expenseService.Update(id, amount, description, category);
            
            Console.WriteLine("Successfully updated expense!");
        });

        return command;
    }
}