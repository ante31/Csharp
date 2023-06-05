using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string datoteka = "pacijenti.json";
        class Pacijent
        {
            public string Oib { get; set; }
            public string Mbo { get; set; }
            public string ImePrezime { get; set; }
            public DateTime DatumRodjenja { get; set; }
            public string Spol { get; set; }
            public string Dijagnoza { get; set; }

            public void Ispisi()
            {
                Console.WriteLine("OIB: " + Oib);
                Console.WriteLine("MBO: " + Mbo);
                Console.WriteLine("Ime i prezime: " + ImePrezime);
                Console.WriteLine("Datum rođenja: " + DatumRodjenja.ToString("dd.MM.yyyy."));
                Console.WriteLine("Spol: " + Spol);
                Console.WriteLine("Dijagnoza: " + Dijagnoza);
            }
        }

        

    private void ButtonAddPacijentClick(object sender, RoutedEventArgs e)
        {
            // Get the input values
            string oib = txtOib.Text.Trim();
            string mbo = txtMbo.Text.Trim();
            string imePrezime = txtImePrezime.Text.Trim();
            DateTime DatumRodjenja = (dpDatumRodjenja.SelectedDate ?? DateTime.Now).Date;
            string spol = (cbSpol.SelectedItem as ComboBoxItem)?.Content.ToString();
            string dijagnoza = txtDijagnoza.Text.Trim();

            // Validate the input values
            if (string.IsNullOrEmpty(oib) || string.IsNullOrEmpty(mbo) || string.IsNullOrEmpty(imePrezime) ||
                string.IsNullOrEmpty(spol) || string.IsNullOrEmpty(dijagnoza))
            {
                MessageBox.Show("Nisu uneseni svi podaci za novog pacijenta.");
                return;
            }
            else if (oib.Length != 11)
            {
                MessageBox.Show("OIB mora biti 11 znamenki dug.");
                return;
            }
            else if (int.TryParse(oib, out _))
            {
                MessageBox.Show("OIB mora biti broj.");
                return;
            }

            else if (mbo.Length != 8)
            {
                MessageBox.Show("MBO mora biti 8 znamenki dug.");
                return;
            }
            else if (!int.TryParse(mbo, out _))
            {
                MessageBox.Show("MBO mora biti broj.");
                return;
            }

            // Create a new Pacijent object with the input values
            Pacijent pacijent = new Pacijent
            {
                Oib = oib,
                Mbo = mbo,
                ImePrezime = imePrezime,
                DatumRodjenja = DatumRodjenja,
                Spol = spol,
                Dijagnoza = dijagnoza
            };

            // Add the Pacijent object to the lstPacijenti ListBox
            lstPacijenti.Items.Add(pacijent);
            string json = JsonConvert.SerializeObject(lstPacijenti.Items.Cast<Pacijent>().ToList());
            File.WriteAllText(datoteka, json);
        }


        public MainWindow()
        {
            InitializeComponent();

            // Check if the JSON file exists
            if (File.Exists(datoteka))
            {
                // Read the JSON from the file
                string json = File.ReadAllText(datoteka);

                // Deserialize the JSON into a list of Pacijent objects
                List<Pacijent> pacijenti = JsonConvert.DeserializeObject<List<Pacijent>>(json);

                // Add the Pacijent objects to the lstPacijenti ListBox
                foreach (Pacijent pacijent in pacijenti)
                {
                    lstPacijenti.Items.Add(pacijent);
                }
            }
        }

        private void MenuItem1Click(object sender, RoutedEventArgs e)
        {
            // Show stackPanel1, hide stackPanel2
            stackPanel1.Visibility = Visibility.Visible;
            stackPanel3.Visibility = Visibility.Collapsed;
            stackPanel2.Visibility = Visibility.Collapsed;
        }

        private void MenuItem2Click(object sender, RoutedEventArgs e)
        {
            // Show stackPanel2, hide stackPanel1
            stackPanel1.Visibility = Visibility.Collapsed;
            stackPanel3.Visibility = Visibility.Collapsed;
            stackPanel2.Visibility = Visibility.Visible;
        }

        private void MenuItem3Click(object sender, RoutedEventArgs e)
        {
            // Show stackPanel2, hide stackPanel1
            stackPanel1.Visibility = Visibility.Collapsed;
            stackPanel2.Visibility = Visibility.Collapsed;
            stackPanel3.Visibility = Visibility.Visible;
        }

        private void ButtonEditPacijentClickVisibility(object sender, RoutedEventArgs e)
        {

            string searchOib = txtsearchOib.Text.Trim();

            // Find the Pacijent object with the given OIB in lstPacijenti

            Pacijent pacijentToEdit = lstPacijenti.Items.OfType<Pacijent>().FirstOrDefault(p => p.Oib == searchOib);


            if (pacijentToEdit != null)
            {
                // Set the values of the controls in the child StackPanel based on the Pacijent object
                txtEditOib.Text = pacijentToEdit.Oib;
                txtEditMbo.Text = pacijentToEdit.Mbo;
                txtEditImePrezime.Text = pacijentToEdit.ImePrezime;
                dpEditDatumRodjenja.SelectedDate = pacijentToEdit.DatumRodjenja;
                cbEditSpol.SelectedIndex = pacijentToEdit.Spol == "Muško" ? 0 : 1;
                txtEditDijagnoza.Text = pacijentToEdit.Dijagnoza;

                // Toggle the visibility of the child StackPanel
                stackPanel4.Visibility = stackPanel4.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void ButtonDeletePacijentClick(object sender, RoutedEventArgs e)
        {
            // Get the OIB to delete from the TextBox
            string oibToDelete = txtDeleteOib.Text.Trim();

            // Find the Pacijent object with the given OIB in lstPacijenti
            Pacijent pacijentToDelete = lstPacijenti.Items.OfType<Pacijent>().FirstOrDefault(p => p.Oib == oibToDelete);

            if (pacijentToDelete != null)
            {
                // If a Pacijent object with the given OIB is found, remove it from lstPacijenti
                lstPacijenti.Items.Remove(pacijentToDelete);
                string json = JsonConvert.SerializeObject(lstPacijenti.Items.Cast<Pacijent>().ToList());
                File.WriteAllText(datoteka, json);
            }
            else
            {
                // If no Pacijent object with the given OIB is found, display an error message
                MessageBox.Show("Pacijent s OIB-om " + oibToDelete + " ne postoji.");
            }
        }

        private void ButtonUrediPacijentClick(object sender, RoutedEventArgs e)
        {
            // Get the Pacijent object to edit
            string searchOib = txtsearchOib.Text.Trim();
            Pacijent pacijentToEdit = lstPacijenti.Items.OfType<Pacijent>().FirstOrDefault(p => p.Oib == searchOib);

            // Update the properties of the Pacijent object
            pacijentToEdit.Mbo = txtEditMbo.Text.Trim();
            pacijentToEdit.ImePrezime = txtEditImePrezime.Text.Trim();
            pacijentToEdit.DatumRodjenja = dpEditDatumRodjenja.SelectedDate.Value;
            pacijentToEdit.Spol = cbEditSpol.SelectedItem.ToString();
            pacijentToEdit.Dijagnoza = txtEditDijagnoza.Text.Trim();

            int index = lstPacijenti.Items.IndexOf(pacijentToEdit);

            // Remove the old Pacijent object from lstPacijenti
            lstPacijenti.Items.RemoveAt(index);

            // Insert the updated Pacijent object at the same index in lstPacijenti
            lstPacijenti.Items.Insert(index, pacijentToEdit);
            string json = JsonConvert.SerializeObject(lstPacijenti.Items.Cast<Pacijent>().ToList());
            File.WriteAllText(datoteka, json);

            // Hide the child StackPanel
            stackPanel4.Visibility = Visibility.Collapsed;
        }
    }
}
