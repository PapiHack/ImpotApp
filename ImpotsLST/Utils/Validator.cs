using System.Text.RegularExpressions;

namespace ImpotsLST.Utils
{
    public class Validator
    {
        public bool IsJourValid(string value)
        {
            int valeur = -1;
            if (int.TryParse(value, out valeur))
            {
                return valeur > 0 ? true : false;
            }
            else
                return false;
        }


        public bool IsEnfantValid(string value)
        {
            int valeur = -1;
            if (int.TryParse(value, out valeur))
            {
                return valeur >= 0 ? true : false;
            }
            else
                return false;
        }

        public bool IsValidSalaire(string sal)
        {
            double salaire = 0;
            if (double.TryParse(sal, out salaire))
            {
                return (salaire >= 50000 && salaire < double.MaxValue) ? true : false;
            }
            else
                return false;
        }

        public bool IsConjointValid(string conjoint)
        {
            int convertedConjoint = -1;
            if (int.TryParse(conjoint, out convertedConjoint))
            {
                if (convertedConjoint == 0 || convertedConjoint == 1)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsValidName(string name)
        {
            Regex nameRegex = new Regex("^[a-zA-Zéèï ]+$");
            if (string.IsNullOrEmpty(name) || !nameRegex.IsMatch(name))
                return false;
            return true;
        }

        public bool IsValidMatricule(string matricule)
        {
            return string.IsNullOrEmpty(matricule) ? false : true;
        }
    }
}
