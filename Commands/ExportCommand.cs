using System.CommandLine;
using ExpenseTrackerCLI.Infrastructure;
using ExpenseTrackerCLI.Services;

namespace ExpenseTrackerCLI.Commands;

public class ExportCommand(IExpenseService expenseService, IExpenseExporter expenseExporter) : ICommand
{
    public Command Build()
    {
        var command = new Command("export", "Export all expenses into CSV file");
        
        var outputArgument = new Argument<FileInfo>("output");
        
        command.Add(outputArgument);
        
        command.SetAction(result =>
        {
            var path = result.GetRequiredValue(outputArgument);
            
            expenseExporter.Export(expenseService.Expenses.Values, path);
            
            Console.WriteLine($"Expenses exported to {path.FullName}");
        });
        
        return command;
    }
}