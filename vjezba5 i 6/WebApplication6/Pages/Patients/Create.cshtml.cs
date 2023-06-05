using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebApplication6.Pages.Patients
{
    public class CreateModel : PageModel
    {
        public PatientInfo patientInfo = new PatientInfo();
        public string errorMessage = "";
		public string successMessage = "";
		public void OnGet()
        {
        }

        public void OnPost() 
        { 
            patientInfo.oib = Request.Form["oib"];
			patientInfo.mbo = Request.Form["mbo"];
			patientInfo.name = Request.Form["name"];
			patientInfo.gender = Request.Form["gender"];
			patientInfo.diagnosis = Request.Form["diagnosis"];

			if (patientInfo.oib.Length == 0 || patientInfo.mbo.Length == 0 || patientInfo.name.Length == 0 || patientInfo.diagnosis.Length == 0)
			{
				errorMessage = "Sva polja moraju biti popunjena!";
				return;
			}

			if (patientInfo.oib.Length != 11 || !patientInfo.oib.All(char.IsDigit))
			{
				errorMessage = "OIB mora imati 11 brojeva!";
				return;
			}

			if (patientInfo.mbo.Length != 8 || !patientInfo.mbo.All(char.IsDigit))
			{
				errorMessage = "MBO mora imati 8 brojeva!";
				return;
			}

			if (!patientInfo.name.Contains(" "))
			{
				errorMessage = "Ime i prezime moraju biti uneseni i odvojeni razmakom!";
				return;
			}

			DateTime dateOfBirth;
			if (DateTime.TryParse(Request.Form["dateOfBirth"], out dateOfBirth))
			{
				patientInfo.dateOfBirth = dateOfBirth.ToString("yyyy-MM-dd");
			}
			else
			{
				errorMessage = "Neispravan format datuma!";
				return;
			}

			//dodavanje pacijenta u bazu
			try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=vjezba5;Integrated Security=True";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "insert into patients" +
								  "(oib, mbo, name, gender, diagnosis, dateOfBirth) values " +
								  "(@oib, @mbo, @name, @gender, @diagnosis, @dateOfBirth);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@oib", patientInfo.oib);
                        command.Parameters.AddWithValue("@mbo", patientInfo.mbo);
                        command.Parameters.AddWithValue("@name", patientInfo.name);
                        command.Parameters.AddWithValue("@gender", patientInfo.gender);
                        command.Parameters.AddWithValue("@diagnosis", patientInfo.diagnosis);
						command.Parameters.AddWithValue("@dateOfBirth", patientInfo.dateOfBirth);


						command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }


            patientInfo.oib = "";
			patientInfo.mbo = "";
			patientInfo.name = "";
			patientInfo.gender = "";
			patientInfo.dateOfBirth = "";
			patientInfo.diagnosis = "";
			successMessage = "Pacijent dodan!";
		}
	}
}
