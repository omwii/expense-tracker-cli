using System.CommandLine;
using ExpenseTrackerCLI.Models;

namespace ExpenseTrackerCLI.Commands;

public class SetBudgetCommand(TrackerConfig trackerConfig) : ICommand
{
    public Command Build()
    {
        var command = new Command("set-budget", "Sets the monthly budget for expenses");

        var budgetArgument = new Argument<int>("budget");
        
        budgetArgument.Validators.Add(result =>
        {
            if (result.GetValueOrDefault<int>() <= 0)
                result.AddError("budget must be greater than zero");
        });
        
        command.SetAction(result =>
        {
            trackerConfig.Budget = result.GetRequiredValue(budgetArgument);
            
            Console.WriteLine($"Budget successfully set to {trackerConfig.Budget}!");
        });
        
        return command;
    }
}