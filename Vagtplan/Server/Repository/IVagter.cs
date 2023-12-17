using Musikfestival.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Musikfestival.Repositories
{

    public interface IVagter
    {
        public Vagter[] GetAllVagter(); //Bruger læser alle vagter

        public MineVagter[] GetMineVagter(); //Bruger læser sine egne vagter
        public void UpdatePlan(Vagter vagter); //Koordinator opdaterer en vagt
        public void CreatePlan(Vagter nyvagt); //Koordinator oprette en ny vagt
        void TakeVagt(Vagter vagter); //Bruger tager en vagt
        Task DeleteVagt(int vagtId); //Koordinator sletter en vagt
    }
}