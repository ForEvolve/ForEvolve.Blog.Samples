using ForEvolve.Blog.Samples.NinjaApi.Models;

namespace ForEvolve.Blog.Samples.NinjaApi
{
    public class ClanNotFoundException : NinjaApiException
    {
        public ClanNotFoundException(Clan clan)
            : this(clan.Name)
        {
        }

        public ClanNotFoundException(string clanName)
            : base($"Clan {clanName} was not found.")
        {
        }
    }
}
