using System;
using System.Collections.Generic;
using System.Windows;
using ImpotsLST.Model;
using ImpotsLST.Utils;

namespace ImpotsLST
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string Matricule;
        private string Nom;
        private string Prenom;
        private string Salaire;
        private string NbreJours;
        private string Conjoint;
        private string Enfants;
        private Validator Validateur;
        private SortedList<string, Employe> listEmploye;
        private List<Employe> DataSource;
        private DBConnection databaseConnection;

        public MainWindow()
        {
            InitializeComponent();
            Validateur = new Validator();
            listEmploye = new SortedList<string, Employe>();
            CurrentDate.Text = "Date d'aujourd'hui : " + GetFormatedDate();
            DataSource = new List<Employe>();
            databaseConnection = new DBConnection("root", "", "dbemploye", "localhost");
        }

        private void ValiderButton_Click(object sender, RoutedEventArgs e)
        {
             Matricule = matricule.Text;
             Nom = nom.Text;
             Prenom = prenom.Text;
             Salaire = salaire.Text;
             NbreJours = jours.Text;
             Conjoint = conjoint.Text;
             Enfants = enfant.Text;

            if(double.TryParse(Salaire, out double sal))
            {
                if (sal > 1000000000)
                {
                    MessageBox.Show("Le montant du salaire n'est par pris en charge par ce programme !");
                    return;
                }

            }

            string reponse = ValidateEmployeInformation();
            if (reponse == "ok")
            {
                CodeConjoint conjoint = Conjoint == "0" ? CodeConjoint.Celibataire : CodeConjoint.ConjointNonSalarie;
                Employe employe = new Employe(Matricule,
                                              Nom,
                                              Prenom,
                                              double.Parse(Salaire),
                                              int.Parse(NbreJours),
                                              int.Parse(Enfants),
                                              conjoint);

                CalculImpot calculImpot = new CalculImpot(employe);
                if (!calculImpot.ValidateSalaire(calculImpot.GetBrutFiscalAnnuel() / 12))
                {
                    MessageBox.Show("Donnez un montant de salaire supérieur au SMIG et ne dépassant pas la normal !");
                    return;
                }
                if (!IsEmployeExist(employe))
                {
                    listEmploye.Add(employe.Matricule, employe);
                    //databaseConnection.Insert(employe);
                    DataSource.Add(employe);
                    calculImpot = new CalculImpot(employe);
                    brutFiscalAnnuel.Content = "Brut Fiscal Annuel : " + calculImpot.GetBrutFiscalAnnuel();
                    abattement.Content = "Abbattement : " + calculImpot.GetAbattement();
                    brutFiscalAbattement.Content = "Brut Fiscal après Abattement : " + calculImpot.GetBrutFiscalApresAbattement();
                    nbreParts.Content = "Nombre de parts : " + calculImpot.GetNbreParts();
                    reduction.Content = "Réduction : " + calculImpot.GetReduction();
                    IRPP.Content = "IRPP avant réduction : " + calculImpot.GetIRPPAvantReduction();
                    impot.Text = " " + calculImpot.GetImpot();
                    salaireNet.Text = "Salaire Net : " + calculImpot.GetSalaireNet();
                }
                else
                    MessageBox.Show("Ce matricule existe déjà !");
            }
            else
                MessageBox.Show(reponse);
            //MessageBox.Show("Vous avez clické sur le boutton Valider ! ");
        }

        private void AnnulerButton_Click(object sender, RoutedEventArgs e)
        {
            matricule.Clear(); nom.Clear();
            prenom.Clear(); salaire.Clear();
            jours.Clear(); conjoint.Clear();
            enfant.Clear();
            brutFiscalAnnuel.Content = "Brut Fiscal Annuel";
            brutFiscalAbattement.Content = "Brut Fiscal après Abattement";
            IRPP.Content = "IRPP avant réduction";
            abattement.Content = "Abattement";
            nbreParts.Content = "Nombre de parts";
            reduction.Content = "Réduction";
            impot.Text = ""; salaireNet.Text = "Salaire Net :";
        }

        private void ListeEmployeButton_Click(object sender, RoutedEventArgs e)
        {
            new AffichageListeEmploye(DataSource).Show();
            //new AffichageListeEmploye(databaseConnection.Select()).Show();
        }

        private void QuitterButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment quitter le programme ?", "Quitter", MessageBoxButton.YesNo);
            if (result.ToString() == "Yes")
                this.Close();
        }

        private string GetFormatedDate()
        {
            string day = "";
            string month = "";

            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday: day = "lundi";
                    break;
                case DayOfWeek.Tuesday: day = "mardi";
                    break;
                case DayOfWeek.Wednesday: day = "mercredi";
                    break;
                case DayOfWeek.Thursday: day = "jeudi";
                    break;
                case DayOfWeek.Friday: day = "vendredi";
                    break;
                case DayOfWeek.Saturday: day = "samedi";
                    break;
                case DayOfWeek.Sunday: day = "dimanche";
                    break;
            }

            switch(DateTime.Now.Month)
            {
                case 1: month = "janvier";
                    break;
                case 2: month = "février";
                    break;
                case 3: month = "mars";
                    break;
                case 4: month = "avril";
                    break;
                case 5: month = "mai";
                    break;
                case 6: month = "juin";
                    break;
                case 7: month = "juillet";
                    break;
                case 8: month = "août";
                    break;
                case 9: month = "septembre";
                    break;
                case 10: month = "octobre";
                    break;
                case 11: month = "novembre";
                    break;
                case 12: month = "décembre";
                    break;
            }
            string formatedDate = day + " " + DateTime.Now.Day + " " + month + " " + DateTime.Now.Year;
            return formatedDate;
        }

        private string ValidateEmployeInformation()
        {
            string msg;

            if(Validateur.IsValidMatricule(Matricule))
            {
                if (Validateur.IsValidName(Nom))
                {
                    if (Validateur.IsValidName(Prenom))
                    {
                        if (Validateur.IsJourValid(NbreJours))
                        {
                            if (Validateur.IsEnfantValid(Enfants))
                            {
                                if (Validateur.IsValidSalaire(Salaire))
                                {
                                    if (Validateur.IsConjointValid(Conjoint))
                                        msg = "ok";
                                    else
                                        msg = "Le champ \"Conjoint\" doit prendre la valeur 0 ou 1 !";
                                }
                                else
                                    msg = "Le salaire doit être supérieur ou égal au SMIG (50000) !";
                            }
                            else
                                msg = "Veuillez saisir un nombre d'enfant valide !";
                        }
                        else
                            msg = "Veuillez saisir un nombre de jours valide !";
                    }
                    else
                        msg = "Veuillez renseigner le champ \"Prenom\" ou donnez un prénom valide !";
                }
                else
                    msg = "Veuillez renseigner le champ \"Nom\" ou donnez un nom valide !";
            }
            else
                msg = "Veuillez renseigner le champ \"Matricule\" !";

            return msg;
        }

        private bool IsEmployeExist(Employe emp)
        {
            return listEmploye.ContainsKey(emp.Matricule);
        }
    }
}
