using System;
using ImpotsLST.Utils;

namespace ImpotsLST.Model
{
    public class Employe
    {
        public string Matricule { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public double SalaireBrut { get; set; }
        public int NbreJours { get; set; }
        public int Enfants { get; set; }
        public CodeConjoint Conjoint { get; set; }

        public Employe(string matricule, string nom, string prenom, double salaire, 
                int nbreJours, int enfants, CodeConjoint conjoint)
        {
            Matricule = matricule;
            Nom = nom;
            Prenom = prenom;
            SalaireBrut = salaire;
            NbreJours = nbreJours;
            Enfants = enfants;
            Conjoint = conjoint;
        }
    }
}
