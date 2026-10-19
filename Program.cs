using System.CommandLine;
using ExpenseTrackerCLI.Commands;
using ExpenseTrackerCLI.Infrastructure;
using ExpenseTrackerCLI.Models;
using ExpenseTrackerCLI.Services;
using Microsoft.Extensions.DependencyInjection;

// Builder
var services = new ServiceCollection();

// Services
services.AddSingleton<TrackerConfig>();
services.AddSingleton<IExpenseService, ExpenseService>();

// Infrastructure
services.AddTransient<IExpenseRepository, JsonExpenseRepository>();
services.AddTransient<IExpenseExporter, CsvExpenseExporter>();

// Commands
services.AddTransient<ICommand, AddCommand>();
services.AddTransient<ICommand, DeleteCommand>();
services.AddTransient<ICommand, ExportCommand>();
services.AddTransient<ICommand, ListCommand>();
services.AddTransient<ICommand, SetBudgetCommand>();
services.AddTransient<ICommand, UpdateCommand>();

// Build
var provider = services.BuildServiceProvider();

// Initialize
var rootCommand = new RootCommand("CLI tool for tracking your expenses");
foreach (var command in provider.GetServices<ICommand>())
    rootCommand.Add(command.Build());

rootCommand.Parse(args).Invoke();