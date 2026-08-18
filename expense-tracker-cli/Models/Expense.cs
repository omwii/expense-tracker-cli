using System.Text.Json.Serialization;

namespace expense_tracker_cli.Models;

[Serializable]
public class Expense (int id, string description, int amount, DateTime date)
{
    public int Id { get; set; } = id;
    public string Description { get; set; } = description;
    public double Amount { get; set; } = amount;
    public DateTime Date { get; set; } = date;
    
    public Expense() : this(0, string.Empty, 0, DateTime.Now) { }
}