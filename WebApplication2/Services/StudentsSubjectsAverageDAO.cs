using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class StudentsSubjectsAverageDAO
    {
        string connectionString = $"Your DB connection string";
        public static int sumOfStudents;
        public List<StudentsSubjectsAverageModel> ShowAllSubjects()
        {
            List<StudentsSubjectsAverageModel> subjects = new List<StudentsSubjectsAverageModel>();

            string sqlCommand = "EXEC subjects_average";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlCommand, connection);

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand("number_of_students", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RetVal", SqlDbType.Float).Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();
                    sumOfStudents = Convert.ToInt32(cmd.Parameters["@RetVal"].Value);

                }

                if (subjects.Count > 0)
                {
                    subjects.Clear();
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
                            subjects.Add(new StudentsSubjectsAverageModel()
                            {
                                Code = reader["code"].ToString(),
                                Subject = reader["subject"].ToString(),
                                CourseAverage = (int)Convert.ToSingle(reader["Course Average"])
                            });
                        }
                    }

                    if (reader.HasRows)
                    {
                        return subjects;
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return new List<StudentsSubjectsAverageModel>();
            }
        }
    }
}
