using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ImpotsLST.Model;

namespace ImpotsLST
{
    /// <summary>
    /// Logique d'interaction pour AffichageListeEmploye.xaml
    /// </summary>
    /// 
    public partial class AffichageListeEmploye : Window
    {
        public AffichageListeEmploye(List<Employe> listEmploye)
        {
            InitializeComponent();
            TabEmploye.ItemsSource = listEmploye;
        }
    }
}
