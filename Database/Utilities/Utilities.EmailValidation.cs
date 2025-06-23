namespace Database.Utilities.EmailValidation;

using System.Text.RegularExpressions;

public class UtilitiesEmailValidation
{
    public static bool Execute(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) { return false; }

        try
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException) { return false; }
    }
}