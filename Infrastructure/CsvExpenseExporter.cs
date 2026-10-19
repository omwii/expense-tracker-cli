using System.Collections;
using System.Globalization;
using CsvHelper;
using ExpenseTrackerCLI.Models;

namespace ExpenseTrackerCLI.Infrastructure;

public class CsvExpenseExporter : IExpenseExporter
{
    public void Export(IEnumerable<Expense> expenses, FileInfo outputFileInfo)
    {
        using var writer = new StreamWriter(outputFileInfo.FullName);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords((IEnumerable)expenses);
    }
}