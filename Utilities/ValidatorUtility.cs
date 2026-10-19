namespace ExpenseTrackerCLI.Utilities;

public static class ValidatorUtility
{
    public static bool ValidateMonth(string month)
    {
        return month switch
        {
            "January" or 
                "February" or 
                "March" or 
                "April" or 
                "May" or 
                "June" or 
                "July" or 
                "August" or 
                "September" or 
                "October" or 
                "November" or 
                "December" => true,
            _ => false
        };
    }
}