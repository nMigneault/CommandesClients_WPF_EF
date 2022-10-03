using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Natacha_Projet_802
{
    /// <summary>
    /// Logique d'interaction pour ListeClients.xaml
    /// </summary>
    public partial class ListeClients : Window
    {
        ObservableCollection<Clients> clients;

        public ListeClients()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListViewClients.ItemsSource = clients;
        }

        //	Ajouter un constructeur avec paramètre(IEnumerable listClients) à la classe « ListeClients » ;
        //	ce paramètre est le résultat de la sélection du double clic sur une commande ; 
        public ListeClients(IEnumerable<Clients> listClients)
        {
            InitializeComponent();
            //IEnumerable<Clients> lstClients = listClients;
            //clients = new ObservableCollection<Clients>(lstClients);
            ListViewClients.ItemsSource = listClients.ToList();
        }
    }
}
