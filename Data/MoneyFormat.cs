using System.Globalization;

namespace Pflegehaushaltsbuch.Data
{
    public static class MoneyFormat
    {
        public static CultureInfo GetCulture(string currencyCode)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.CurrencySymbol = GetCurrencySymbol(currencyCode);
            return culture;
        }

        private static string GetCurrencySymbol(string currencyCode)
        {
            switch ((currencyCode ?? string.Empty).Trim().ToUpperInvariant())
            {
                case "EUR":
                    return "\u20ac";
                case "USD":
                    return "$";
                case "GBP":
                    return "\u00a3";
                case "CHF":
                    return "CHF";
                case "TRY":
                    return "\u20ba";
                case "RUB":
                    return "\u20bd";
                default:
                    return string.IsNullOrWhiteSpace(currencyCode) ? "\u20ac" : currencyCode.Trim().ToUpperInvariant();
            }
        }
    }
}
