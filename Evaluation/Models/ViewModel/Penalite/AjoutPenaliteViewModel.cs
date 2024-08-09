using Evaluation.Models.Entity;

namespace Evaluation.Models.ViewModel
{
    public class AjoutPenaliteViewModel
    {
        public string EtapeId { get; set; } = "";
        public string EquipeId { get; set; } = "";
        public TimeOnly TempsPenalite { get; set; }
        public string TempsPenaliteString { get; set; } = "";

        public List<Etape> Etapes { get; set; } = new List<Etape>();       
        public List<Equipe> Equipes { get; set; } = new List<Equipe>();

    }
}
