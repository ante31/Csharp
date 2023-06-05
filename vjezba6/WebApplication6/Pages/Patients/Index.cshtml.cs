using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WebApplication6.Pages.Patients
{
    public class IndexModel : PageModel
    {
        public List<PatientInfo> listPatients = new List<PatientInfo>();
        public string SearchString { get; set; }
        public string SortOrder { get; set; } = "gender_asc";
        public string genderFilter { get; set; }

        public void OnGet(string genderFilter)
        {
            string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=vjezba5;Integrated Security=True";
            string sql = "SELECT * FROM patients";
            string whereClause = "";
            string orderByClause = "";

            // Check if gender filter exists in query parameters
            if (!string.IsNullOrEmpty(genderFilter))
            {
                whereClause = " WHERE gender = '" + genderFilter + "'";
            }

            // Check if search query exists in query parameters
            if (Request.Query.ContainsKey("searchString"))
            {
                SearchString = Request.Query["searchString"];
                if (!string.IsNullOrEmpty(whereClause))
                {
                    whereClause += " AND (name LIKE '%" + SearchString + "%' " +
                                  " OR oib LIKE '%" + SearchString + "%' " +
                                  " OR mbo LIKE '%" + SearchString + "%' " +
                                  " OR diagnosis LIKE '%" + SearchString + "%')";
                }
                else
                {
                    whereClause = " WHERE name LIKE '%" + SearchString + "%' " +
                                  " OR oib LIKE '%" + SearchString + "%' " +
                                  " OR mbo LIKE '%" + SearchString + "%' " +
                                  " OR diagnosis LIKE '%" + SearchString + "%'";
                }
            }

            // Check if sort order exists in query parameters
            if (Request.Query.ContainsKey("sortOrder"))
            {
                SortOrder = Request.Query["sortOrder"];
            }


            // Build the order by clause based on sort order
            switch (SortOrder)
            {
                case "gender_asc":
                    orderByClause = " ORDER BY gender ASC";
                    break;
                case "gender_desc":
                    orderByClause = " ORDER BY gender DESC";
                    break;
                case "name_asc":
                    orderByClause = " ORDER BY name ASC";
                    break;
                case "name_desc":
                    orderByClause = " ORDER BY name DESC";
                    break;
                case "oib_asc":
                    orderByClause = " ORDER BY oib ASC";
                    break;
                case "oib_desc":
                    orderByClause = " ORDER BY oib DESC";
                    break;
                case "mbo_asc":
                    orderByClause = " ORDER BY mbo ASC";
                    break;
                case "mbo_desc":
                    orderByClause = " ORDER BY mbo DESC";
                    break;
                case "doa_asc":
                    orderByClause = " ORDER BY dateOfAdmission ASC";
                    break;
                case "doa_desc":
                    orderByClause = " ORDER BY dateOfAdmission DESC";
                    break;
                case "dod_asc":
                    orderByClause = " ORDER BY dateOfDischarge ASC";
                    break;
                case "dod_desc":
                    orderByClause = " ORDER BY dateOfDischarge DESC";
                    break;
                default:
                    orderByClause = " ORDER BY id ASC";
                    break;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sql + whereClause + orderByClause, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PatientInfo patientInfo = new PatientInfo();
                                patientInfo.id = "" + reader.GetInt32(0);
                                patientInfo.oib = "" + reader.GetString(1);
                                patientInfo.mbo = "" + reader.GetString(2);
                                patientInfo.name = reader.GetString(3);
                                patientInfo.gender = reader.GetString(4);
                                patientInfo.diagnosis = reader.GetString(5);
                                patientInfo.dateOfBirth = reader.GetDateTime(6).Date.ToString("yyyy-MM-dd");
                                patientInfo.dateOfAdmission = reader.GetDateTime(7).Date.ToString("yyyy-MM-dd");
                                if (!reader.IsDBNull(8))
                                {
                                    patientInfo.dateOfDischarge = reader.GetDateTime(8).Date.ToString("yyyy-MM-dd");
                                }
                                listPatients.Add(patientInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            ViewData["genderFilter"] = genderFilter;
            ViewData["SearchString"] = SearchString;
            ViewData["SortOrder"] = SortOrder;
        }
    }

    public class PatientInfo
    {
        public string id;
        public string oib;
        public string mbo;
        public string name;
        public string gender;
        public string diagnosis;
        public string dateOfBirth;
        public string dateOfAdmission;
        public string? dateOfDischarge;
    }
}
