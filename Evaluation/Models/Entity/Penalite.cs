using Evaluation.Models.Cnx;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Evaluation.Models.Entity
{
    public class Penalite
    {
        private Guid _id;
        private Guid _etapeId;
        private Etape _etape;
        private Guid _equipeId;
        private Equipe _equipe;
        private TimeSpan _tempsPenalite;
        private int _etat;

        [Key]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }
        [ForeignKey("Etape")]
        public Guid EtapeId
        {
            get { return _etapeId; }
            set { _etapeId = value; }
        }
        public Etape Etape
        {
            get { return _etape; }
            set { _etape = value; }
        }
        [ForeignKey("Equipe")]
        public Guid EquipeId
        {
            get { return _equipeId; }
            set { _equipeId = value; }
        }
        public Equipe Equipe
        {
            get { return _equipe; }
            set { _equipe = value; }
        }

        public TimeSpan TempsPenalite
        {
            get { return _tempsPenalite; }
            set { _tempsPenalite = value; }
        }

        public int Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }

        public override string? ToString()
        {
            string stringValue = "{";
            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                stringValue += property.Name + "=" + property.GetValue(this) + ";";
            }
            stringValue += "}";

            return stringValue;
        }

        // ------------------- FUNCTIONS -------------------

        public async Task<int> AddNewPenalite(PsqlContext context, string etapeId, string equipeId, TimeSpan tempsPenalite)
        {
            int writen = 0;
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Etape etape = context.Etape
                        .Include(e => e.Coureurs)
                        .First(e => e.Id == Guid.Parse(etapeId));

                    List<Coureur> coureursEquipeAffectees = etape.Coureurs
                        .Where(c => c.EquipeId == Guid.Parse(equipeId)).ToList();
                    foreach (Coureur coureurAffectee in coureursEquipeAffectees)
                    {
                        Resultat? resultatCoureurThisEtape = await context.Resultat
                            .FirstOrDefaultAsync(r => r.EtapeId == etape.Id && r.CoureurId == coureurAffectee.Id);
                        if(resultatCoureurThisEtape != null)
                        {
                            resultatCoureurThisEtape.DateArrivee = resultatCoureurThisEtape.DateArrivee.Add(tempsPenalite);
                            context.Resultat.Update(resultatCoureurThisEtape);
                        }
                    }

                    Penalite newPenalite = new Penalite
                    {
                        EtapeId = etape.Id,
                        EquipeId = Guid.Parse(equipeId),
                        TempsPenalite = tempsPenalite
                    };

                    await context.Penalite.AddAsync(newPenalite);
                    writen = await context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    Console.Error.WriteLine(e.StackTrace);
                    throw e;
                }

                return writen;
            }
        }

        public async Task<int> DeletePenalite(PsqlContext context, string idPenalite)
        {
            int writen = 0;
            using(var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Penalite? penaliteToDelete = await context.Penalite
                        .Include(p => p.Etape)
                            .ThenInclude(e => e.Coureurs)
                        .FirstOrDefaultAsync(p => p.Id == Guid.Parse(idPenalite));

                    if(penaliteToDelete != null)
                    {
                        List<Coureur> coureursEquipeAffectees = penaliteToDelete.Etape.Coureurs
                        .Where(c => c.EquipeId == penaliteToDelete.EquipeId).ToList();

                        foreach (Coureur coureurAffectee in coureursEquipeAffectees)
                        {
                            Resultat? resultatCoureurThisEtape = await context.Resultat
                            .FirstOrDefaultAsync(r => r.EtapeId == penaliteToDelete.EtapeId && r.CoureurId == coureurAffectee.Id);
                            
                            if (resultatCoureurThisEtape != null)
                            {
                                resultatCoureurThisEtape.DateArrivee = resultatCoureurThisEtape.DateArrivee.Subtract(penaliteToDelete.TempsPenalite);
                                context.Resultat.Update(resultatCoureurThisEtape);
                            }

                            
                        }

                        penaliteToDelete.Etat = -1;
                        context.Penalite.Update(penaliteToDelete);
                        writen = await context.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                }
                catch(Exception e)
                {
                    await transaction.RollbackAsync();
                    Console.Error.WriteLine(e.StackTrace);
                    throw e;
                }
            }
            return writen;
        }
    }
}
