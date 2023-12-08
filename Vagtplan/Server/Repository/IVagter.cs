using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Musikfestival.Repositories
{


    public interface IVagter
    {
        public Vagter[] GetAllVagter();

        void AddTilFordeling(Vagter vagter);
    }
}