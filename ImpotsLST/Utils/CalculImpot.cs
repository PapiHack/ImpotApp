using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImpotsLST.Model;
using ImpotsLST.Utils;

namespace ImpotsLST.Utils
{
    public class CalculImpot
    {
        public Employe employe { get; set; }
        public double BrutFiscalAnnuel { get; set; }
        public double BrutFiscalApresAbattement { get; set; }
        public double IRPPAvantReduction { get; set; }
        public double Impot { get; set; }
        public double Abattement { get; set; }
        public float NbreParts { get; set; }
        public double Reduction { get; set; }
        public double SalaireNet { get; set; }

        public CalculImpot(Employe e)
        {
            employe = e;
        }

        public float GetNbreParts()
        {
            return NbreParts = (employe.Conjoint
                                == CodeConjoint.Celibataire) ? (float)(1 + (employe.Enfants * 0.5))  : (float)(1.5 + (employe.Enfants * 0.5));
        }


        public double GetBrutFiscalAnnuel()
        {
            BrutFiscalAnnuel = Math.Round((employe.SalaireBrut * 360) / employe.NbreJours);
            return BrutFiscalAnnuel;
        }

        public double GetAbattement()
        {
            Abattement = Math.Round((BrutFiscalAnnuel * 30) / 100);
            Abattement = Abattement > 900000 ? 900000 : Abattement;
            return Abattement;
        }

        public double GetReduction()
        {
            if (NbreParts == 1)
                Reduction = 0;
            else if (NbreParts < 4.5)
                Reduction = 100000 + (((NbreParts - 1.5) / 0.5) * 100000);
            else
                Reduction = 800000;
            return Reduction;
        }

        public double GetBrutFiscalApresAbattement()
        {
            BrutFiscalApresAbattement = BrutFiscalAnnuel - Abattement;
            return BrutFiscalApresAbattement;
        }

        public double GetIRPPAvantReduction()
        {
            List<IRPPInterval> iRPPIntervals = new List<IRPPInterval>();
            iRPPIntervals.Add(new IRPPInterval(0, 630000, 0));
            iRPPIntervals.Add(new IRPPInterval(630001, 1500000, 20));
            iRPPIntervals.Add(new IRPPInterval(1500001, 4000000, 30));
            iRPPIntervals.Add(new IRPPInterval(4000001, 8000000, 35));
            iRPPIntervals.Add(new IRPPInterval(8000001, 13500000, 37));
            iRPPIntervals.Add(new IRPPInterval(13500001, 1000000000, 40));

            IRPPAvantReduction = 0;

            foreach (IRPPInterval iRPPInterval in iRPPIntervals)
            {
                //IRPPAvantReduction += (BrutFiscalApresAbattement > iRPPInterval.Max) ? ((iRPPInterval.Max - iRPPInterval.Min) * iRPPInterval.Pourcentage / 100) : ((BrutFiscalApresAbattement - iRPPInterval.Min) * iRPPInterval.Pourcentage / 100);
                if(BrutFiscalApresAbattement > iRPPInterval.Max)
                {
                    IRPPAvantReduction += ((iRPPInterval.Max - iRPPInterval.Min + 1) * iRPPInterval.Pourcentage / 100);
                }
                else
                {
                    IRPPAvantReduction += ((BrutFiscalApresAbattement - iRPPInterval.Min) * iRPPInterval.Pourcentage / 100);
                    break;
                }
            }

            return IRPPAvantReduction = Math.Round(IRPPAvantReduction);
        }

        public double GetImpot()
        {
            Impot = IRPPAvantReduction - Reduction;
            return Impot;
        }

        public double GetSalaireNet()
        {
            SalaireNet = BrutFiscalAnnuel - Impot;
            return SalaireNet;
        }

        public bool ValidateSalaire(double salaire)
        {
            return new Validator().IsValidSalaire(salaire.ToString());
        }
    }
}
