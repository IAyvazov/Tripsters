namespace Tripsters.Services.Data.Towns
{
    using Tripsters.Data.Models;

    public interface ITownsService
    {
        Town GetTownByName(string townName);
    }
}
