using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Musikfestival.Repositories
{
    

    public interface IBruger
    {
        //public void Update(Bruger bruger);

        public Bruger[] GetAllBrugere(); // Læs alle brugere
        Task<Bruger> AuthenticateUserAsync(string username, string password); // Godkend credentials
        public void Update(Bruger bruger); // Opdater Bruger med argumentet bruger
    }
}