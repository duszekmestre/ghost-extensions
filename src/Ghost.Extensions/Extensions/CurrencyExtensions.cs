using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;

namespace Ghost.Extensions.Extensions
{
    public static class CurrencyExtensions
    {
        private static ConcurrentDictionary<string, string> currencySymbolDict = new ConcurrentDictionary<string, string>();

        public static string ToCurrencySymbol(this string isoCode)
        {
            if (string.IsNullOrWhiteSpace(isoCode))
            {
                return string.Empty;
            }

            if (currencySymbolDict.TryGetValue(isoCode, out string currencySymbol) == false)
            {
                var culture = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x =>
                {
                    try
                    {
                        return new RegionInfo(x.Name);
                    }
                    catch
                    {
                        return null;
                    }
                }).FirstOrDefault(x => x?.ISOCurrencySymbol == isoCode);

                currencySymbol = culture?.CurrencySymbol ?? isoCode;
                currencySymbolDict.TryAdd(isoCode, currencySymbol);
            }

            return currencySymbol;
        }
    }
}
