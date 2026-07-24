
public enum CountryTiers { Tier1, Tier2, Tier3 }

public static class CountryTier 
{
    public static CountryTiers GetCountry(string countryCode)
    {
        return countryCode switch
        {
            "US" or "KR" or "JP" or "AU" or "CA" or "NZ" or "GB" or "DE" or "FR" or "TW" => CountryTiers.Tier1,
            _ => CountryTiers.Tier3
        };
    }
}
