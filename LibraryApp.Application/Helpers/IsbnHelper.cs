namespace LibraryApp.Application.Helpers;

public static class IsbnHelper
{
    private static Random random = new Random();

    public static string GenerateIsbn13()
    {
        string prefix = "978";

        string body = "";
        for (int i = 0; i < 9; i++)
        {
            body += random.Next(0, 10);
        }

        string isbn12 = prefix + body;

        int checkDigit = CalculateCheckDigit(isbn12);

        return isbn12 + checkDigit;
    }

    private static int CalculateCheckDigit(string isbn12)
    {
        int sum = 0;

        for (int i = 0; i < 12; i++)
        {
            int digit = int.Parse(isbn12[i].ToString());
            sum += (i % 2 == 0) ? digit : digit * 3;
        }

        return (10 - (sum % 10)) % 10;
    }
}