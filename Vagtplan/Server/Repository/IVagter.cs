using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Musikfestival.Repositories
{


    public interface IVagter
    {
        public Vagter[] GetAllVagter();
        public void UpdatePlan(Vagter vagter);
        public void CreatePlan(Vagter nyvagt);
        void AddVagt(Vagter vagter);
    }
}