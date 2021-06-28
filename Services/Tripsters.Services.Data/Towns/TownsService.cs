namespace Tripsters.Services.Data.Towns
{
    using System.Linq;

    using Tripsters.Data.Common.Repositories;
    using Tripsters.Data.Models;
    using Tripsters.Web.ViewModels.Towns;

    public class TownsService : ITownsService
    {
        private readonly Tripsters.Data.Common.Repositories.IDeletableEntityRepository<Town> townRepository;

        public TownsService(IDeletableEntityRepository<Town> townRepository)
        {
            this.townRepository = townRepository;
        }

        public Town GetTownByName(string townName)
       => this.townRepository.All()
            .Where(t => t.Name == townName)
            .FirstOrDefault();
    }
}
