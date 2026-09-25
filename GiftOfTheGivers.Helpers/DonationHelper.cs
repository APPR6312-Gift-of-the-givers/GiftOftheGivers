namespace GiftOfTheGivers.Helpers
{
    public static class DonationHelper
    {
        public static string FormatAmount(decimal amount)
        {
            return $"R {amount:N2}";
        }
    }
}
