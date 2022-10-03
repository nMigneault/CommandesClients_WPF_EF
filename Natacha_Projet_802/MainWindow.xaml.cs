using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Natacha_Projet_802
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (masterEntities entityContext = new masterEntities())
            {             
                listViewEmployes.ItemsSource = entityContext.Employes.ToList();
                cbxProduits.ItemsSource = entityContext.Produits.ToList();
                cbxCategories.ItemsSource = entityContext.Categories.ToList();   
            }
        }

        // faire apparaitre les infos de la BD dans les champs à remplir
        private void listViewEmployes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (masterEntities entityContext = new masterEntities())
            {
                var selectedItem = listViewEmployes.SelectedItem as Employes;

                if (selectedItem == null)
                {
                    return;
                }
                // Chercher l'employé dans la liste des employés
                Employes employeSelected = entityContext.Employes.FirstOrDefault(x => x.EmployeID == selectedItem.EmployeID);

                if (employeSelected != null)
                {
                    txtNom.Text = employeSelected.Nom;
                    txtPrenom.Text = employeSelected.Prenom;
                    txtTitre.Text = employeSelected.Titre;
                    txtDateNaissance.SelectedDate = employeSelected.DateDeNaissance;
                    txtDateEmbauche.SelectedDate = employeSelected.DateEmbauche;
                    txtAdresse.Text = employeSelected.Adresse;
                    txtProvince.Text = employeSelected.Province;
                    txtCodePostal.Text = employeSelected.CodePostal;
                    txtTelephone.Text = employeSelected.Telephone;
                    txtExtension.Text = employeSelected.Extension;
                }
            }
        }

        // Le double clique sur un employé de la liste des employés (ListViewEmployes) affichera toutes les commandes faites par celui-ci ;
        private void lstViewEmployes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            using (masterEntities entityContext = new masterEntities())
            {
                var selectedItem = listViewEmployes.SelectedItem as Employes;
                IEnumerable<Commandes> listeCommande = entityContext.Commandes.Where(x => x.EmployeID == selectedItem.EmployeID).ToList();
                listViewCommandes.ItemsSource = listeCommande.ToList();
            }
        }

        // Le double clique sur une commande de la liste des commandes affichera une nouvelle fenêtre qui contiendra la liste des clients associés à la commande ;
        private void lstViewCommandes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            using (masterEntities entityContext = new masterEntities())
            {
                Commandes selectedItem = listViewCommandes.SelectedItem as Commandes;
                IEnumerable<Clients> listeCommandeClient = entityContext.Clients.Where(x => x.ClientID == selectedItem.ClientID).ToList();
                ListeClients listeClients = new ListeClients(listeCommandeClient);
                listeClients.ShowDialog();
            }
        }

        // effacer le contenu des champs dans le UI
        private void btnEffacer_Click(object sender, RoutedEventArgs e)
        {
            txtNom.Text = "";
            txtPrenom.Text = "";
            txtTitre.Text = "";
            txtDateNaissance.SelectedDate = null;
            txtDateEmbauche.SelectedDate = null;
            txtAdresse.Text = "";
            txtProvince.Text = "";
            txtCodePostal.Text = "";
            txtPays.Text = "";
            txtTelephone.Text = "";
            txtExtension.Text = "";
            txtNotes.Document.Blocks.Clear();
        }

        private void btnSauvegarder_Click(object sender, RoutedEventArgs e)
        {
            using (masterEntities entityContext = new masterEntities())
            {
                var maxEmployeID = entityContext.Employes.Max(x => x.EmployeID);
                DateTime dateEmbauche = DateTime.Now;
                DateTime dateNaissance = DateTime.Now;
                string richText = "";

                if (txtNom.Text == "" || txtPrenom.Text == "")
                {
                    MessageBox.Show("Vous devez saisir le nom et prénom de l'employé.", "Attention");
                    return;
                }

                if (txtDateEmbauche.SelectedDate != null)
                {
                    dateEmbauche = txtDateEmbauche.SelectedDate.Value;
                }

                if (txtDateNaissance.SelectedDate != null)
                {
                    dateNaissance = txtDateNaissance.SelectedDate.Value;
                }

                if (txtNotes.Document != null)
                {
                    richText = new TextRange(txtNotes.Document.ContentStart, txtNotes.Document.ContentEnd).Text;
                }

                Employes employe = new Employes
                {                    
                    EmployeID = ++maxEmployeID,
                    Nom = txtNom.Text,
                    Prenom = txtPrenom.Text,
                    Titre = txtTitre.Text,
                    DateDeNaissance = dateNaissance,
                    DateEmbauche = dateEmbauche,
                    Adresse = txtAdresse.Text,
                    Province = txtProvince.Text,
                    CodePostal = txtCodePostal.Text,
                    Pays = txtPays.Text,
                    Telephone = txtTelephone.Text,
                    Extension = txtExtension.Text,
                    Notes = richText
                };
            
                if (employe != null)
                {
                    entityContext.Employes.Add(employe);
                    entityContext.SaveChanges();
                    listViewEmployes.ItemsSource = entityContext.Employes.ToList();
                }
            }
        }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment quitter?", "Attention", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;  
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            using (masterEntities entityContext = new masterEntities())
            {
                var selectedItem = listViewEmployes.SelectedItem as Employes;

                if (selectedItem == null)
                {
                    MessageBox.Show("Vous devez sélectionner un employé.", "Attention");
                    return;
                }
               
                // Chercher l'employé dans la liste des employés
                Employes employeToModify = entityContext.Employes.FirstOrDefault(x => x.EmployeID == selectedItem.EmployeID);
                DateTime dateEmbauche = DateTime.Now;
                DateTime dateNaissance = DateTime.Now;
                string richText = "";

                if (txtDateEmbauche.SelectedDate != null)
                {
                    dateEmbauche = txtDateEmbauche.SelectedDate.Value;
                }

                if (txtDateNaissance.SelectedDate != null)
                {
                    dateNaissance = txtDateNaissance.SelectedDate.Value;
                }

                if (txtNotes.Document != null)
                {
                    richText = new TextRange(txtNotes.Document.ContentStart, txtNotes.Document.ContentEnd).Text;
                }

                if (employeToModify != null)
                {
                    employeToModify.Nom = txtNom.Text;
                    employeToModify.Prenom = txtPrenom.Text;
                    employeToModify.Titre = txtTitre.Text;
                    employeToModify.DateDeNaissance = dateNaissance;
                    employeToModify.DateEmbauche = dateEmbauche;
                    employeToModify.Adresse = txtAdresse.Text;
                    employeToModify.Province = txtProvince.Text;
                    employeToModify.CodePostal = txtCodePostal.Text;
                    employeToModify.Telephone = txtTelephone.Text;
                    employeToModify.Extension = txtExtension.Text;
                    employeToModify.Notes = richText;
                   
                    entityContext.SaveChanges();

                    listViewEmployes.ItemsSource = entityContext.Employes.ToList();
                }
            }
        }

        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            using (masterEntities entityContext = new masterEntities())
            {
                var selectedItem = listViewEmployes.SelectedItem as Employes;

                if (selectedItem == null)
                {
                    MessageBox.Show("Vous devez sélectionner un employé.", "Attention");
                    return;
                }

                Employes employeToDelete = entityContext.Employes.FirstOrDefault(x => x.EmployeID == selectedItem.EmployeID);
                IEnumerable<Commandes> commandsToDelete = entityContext.Commandes.Where(x => x.EmployeID == selectedItem.EmployeID);
                if (employeToDelete != null)
                {
                    MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer cet employé?", "Attention", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                    else
                    {
                        if (commandsToDelete != null)
                        {
                            entityContext.Commandes.RemoveRange(commandsToDelete);
                        }

                        entityContext.Employes.Remove(employeToDelete);
                        entityContext.SaveChanges();
                        listViewEmployes.ItemsSource = entityContext.Employes.ToList();
                    }                   
                }
            }
        }

        private void btnWebView_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.YouTube.com");
        }
    }
}
