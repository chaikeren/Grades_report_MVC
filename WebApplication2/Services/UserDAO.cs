using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class UserDAO
    {
        string connectionString = $"Data Source=DESKTOP-CJ3645A\\SQLEXPRESS;Initial Catalog=Ruppin_Students;" +
                                  $"Integrated Security=True;Connect Timeout=30;Encrypt=False;" +
                                  $"TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static decimal avg;
        public static string email;

        public List<UserSubjetctsAverageModel> FindUserByIdAndPassword(UserModel user)
        {
            List<UserSubjetctsAverageModel> userSubjects = new List<UserSubjetctsAverageModel>();

            string sqlCommand = "EXEC add_user @Student_Id, @Password, @Email\n" +
                " EXEC user_subjects_average  @Student_Id";  
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
              
                SqlCommand command = new SqlCommand(sqlCommand, connection);

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand("user_average", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@user_id", user.Id);
                    cmd.Parameters.Add("@RetVal", SqlDbType.Float).Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();
                    var read = cmd.ExecuteReader();
                    avg = Convert.ToInt32(cmd.Parameters["@RetVal"].Value);
                }

                command.Parameters.Add("@Student_Id", SqlDbType.NVarChar, 100).Value = user.Id;
                string encryptedPassword = Encrypt(user.Password, user.Id);
                command.Parameters.Add("@Password", SqlDbType.NVarChar, 100).Value = encryptedPassword;
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = user.Email;
                email = user.Email;

                if (userSubjects.Count > 0)
                {
                    userSubjects.Clear();
                }
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        int num = reader.FieldCount;
                        while (reader.Read())
                        {
                            userSubjects.Add(new UserSubjetctsAverageModel() {
                             Subject = reader["subject"].ToString(),
                             GradeAverage = Convert.ToSingle(reader["Course Average"])
                            });
                        }
                    }

                    if (reader.HasRows)
                    {
                        reader.Close();
                        return userSubjects;
                    }

                    connection.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return new List<UserSubjetctsAverageModel>();
            }
        }
        public static string Encrypt(string encryptString, string encryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }
    }
}

