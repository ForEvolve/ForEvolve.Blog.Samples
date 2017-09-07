using ForEvolve.Blog.Samples.NinjaApi.Models;

namespace ForEvolve.Blog.Samples.NinjaApi
{
    public class NinjaNotFoundException : NinjaApiException
    {
        public NinjaNotFoundException(Ninja ninja)
            : base($"Ninja {ninja.Name} ({ninja.Key}) of clan {ninja.Clan.Name} was not found.")
        {
        }

        public NinjaNotFoundException(string clanName, string ninjaKey)
            : base($"Ninja {ninjaKey} of clan {clanName} was not found.")
        {
        }
    }
}
