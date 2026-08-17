namespace expense_tracker_cli.Models;

[Serializable]
public class Expense (int id, string description, int amount)
{
    public int Id { get; set; } = id;
    public string Description { get; set; } = description;
    public double Amount { get; set; } = amount;
    public DateTime Date { get; set; } = DateTime.Now;
}