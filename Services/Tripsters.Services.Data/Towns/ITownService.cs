using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tripsters.Data.Models;

namespace Tripsters.Services.Data.Towns
{
    public interface ITownService
    {
        Town GetTown(string townId);

        Town GetTownByName(string name);

        Task<Town> AddTown(Town town);
    }
}
