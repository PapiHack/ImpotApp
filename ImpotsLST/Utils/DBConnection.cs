using ImpotsLST.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ImpotsLST.Utils
{
    public class DBConnection
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string Host { get; set; }
        private MySqlConnection connection;

        public DBConnection(string username, string password, string dbName, string host)
        {
            Username = username;
            Password = password;
            DatabaseName = dbName;
            Host = host;
            MySqlConnectionStringBuilder connectionParams = new MySqlConnectionStringBuilder();
            connectionParams.Database = DatabaseName;
            connectionParams.Server = Host;
            connectionParams.UserID = Username;
            connectionParams.Password = Password;
            connection = new MySqlConnection(connectionParams.ConnectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                /* switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }*/
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Insert(Employe employe)
        {
      
            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "INSERT INTO employe VALUES(@matricule, @nom, @prenom, @salaire, @enfants, @conjoint)";
                //cmd.Prepare();
                cmd.Parameters.AddWithValue("matricule", employe.Matricule);
                cmd.Parameters.AddWithValue("nom", employe.Nom);
                cmd.Parameters.AddWithValue("prenom", employe.Prenom);
                cmd.Parameters.AddWithValue("salaire", employe.SalaireBrut);
                cmd.Parameters.AddWithValue("enfants", employe.Enfants);
                cmd.Parameters.AddWithValue("conjoint", employe.Conjoint == CodeConjoint.Celibataire ? 0 : 1);
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public List<Employe> Select()
        {
            List<Employe> Employes = new List<Employe>();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM employe", connection);

            if (OpenConnection() == true)
            {
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Employes.Add(new Employe(dataReader["matricule"].ToString(), dataReader["nom"].ToString(), dataReader["prenom"].ToString(), double.Parse(dataReader["salaire"].ToString()), 0, int.Parse(dataReader["enfant"].ToString()), (int.Parse(dataReader["conjoint"].ToString())) == 1 ? CodeConjoint.ConjointNonSalarie : CodeConjoint.Celibataire));
                }
                dataReader.Close();

                CloseConnection();
            }
            else
            {
                return Employes;
            }

            return Employes;
        }
    }
}
