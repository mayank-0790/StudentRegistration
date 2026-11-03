using System;
using System.Configuration;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace StudentRegistrationSystem
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StudentDbConnection"].ConnectionString;
        }

        public bool InsertStudent(Student student, out string errorMessage)
        {
            errorMessage = string.Empty;

            const string query = @"
                INSERT INTO Students (FirstName, LastName, Email, Course) 
                VALUES (@FirstName, @LastName, @Email, @Course);";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@FirstName", MySqlDbType.VarChar, 50).Value = student.FirstName;
                        cmd.Parameters.Add("@LastName", MySqlDbType.VarChar, 50).Value = student.LastName;
                        cmd.Parameters.Add("@Email", MySqlDbType.VarChar, 100).Value = student.Email;
                        cmd.Parameters.Add("@Course", MySqlDbType.VarChar, 50).Value = student.Course;

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    errorMessage = "A student with this email address is already registered.";
                }
                else
                {
                    errorMessage = "Database error: " + ex.Message;
                }
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = "Unexpected error: " + ex.Message;
                return false;
            }
        }

        public DataTable GetFilteredStudents(string searchTerm, string courseFilter, string sortExpression = "StudentId ASC")
        {
            DataTable dt = new DataTable();
            StringBuilder queryBuilder = new StringBuilder("SELECT StudentId, FirstName, LastName, Email, Course, CreatedDate FROM Students WHERE 1=1 ");

            bool hasSearch = !string.IsNullOrWhiteSpace(searchTerm);
            bool hasCourse = !string.IsNullOrWhiteSpace(courseFilter) && courseFilter != "ALL";

            if (hasSearch)
            {
                queryBuilder.Append(" AND (FirstName LIKE @SearchTerm OR LastName LIKE @SearchTerm OR Email LIKE @SearchTerm) ");
            }

            if (hasCourse)
            {
                queryBuilder.Append(" AND Course = @Course ");
            }

            // Allowed sorting whitelist to prevent SQL injection in ORDER BY
            string validatedSort = "StudentId ASC";
            string[] validColumns = { "StudentId", "FirstName", "LastName", "Email", "Course", "CreatedDate" };
            if (!string.IsNullOrWhiteSpace(sortExpression))
            {
                string[] parts = sortExpression.Split(' ');
                string col = parts[0];
                string dir = (parts.Length > 1 && parts[1].Equals("DESC", StringComparison.OrdinalIgnoreCase)) ? "DESC" : "ASC";
                foreach (string validCol in validColumns)
                {
                    if (validCol.Equals(col, StringComparison.OrdinalIgnoreCase))
                    {
                        validatedSort = string.Format("{0} {1}", validCol, dir);
                        break;
                    }
                }
            }

            queryBuilder.AppendFormat(" ORDER BY {0};", validatedSort);

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(queryBuilder.ToString(), conn))
                {
                    if (hasSearch)
                    {
                        cmd.Parameters.Add("@SearchTerm", MySqlDbType.VarChar).Value = "%" + searchTerm.Trim() + "%";
                    }

                    if (hasCourse)
                    {
                        cmd.Parameters.Add("@Course", MySqlDbType.VarChar, 50).Value = courseFilter.Trim();
                    }

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        conn.Open();
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetAllStudents(string sortExpression = "StudentId ASC")
        {
            return GetFilteredStudents(string.Empty, string.Empty, sortExpression);
        }

        public bool UpdateStudent(Student student, out string errorMessage)
        {
            errorMessage = string.Empty;

            const string query = @"
                UPDATE Students 
                SET FirstName = @FirstName, 
                    LastName = @LastName, 
                    Email = @Email, 
                    Course = @Course 
                WHERE StudentId = @StudentId;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@StudentId", MySqlDbType.Int32).Value = student.StudentId;
                        cmd.Parameters.Add("@FirstName", MySqlDbType.VarChar, 50).Value = student.FirstName;
                        cmd.Parameters.Add("@LastName", MySqlDbType.VarChar, 50).Value = student.LastName;
                        cmd.Parameters.Add("@Email", MySqlDbType.VarChar, 100).Value = student.Email;
                        cmd.Parameters.Add("@Course", MySqlDbType.VarChar, 50).Value = student.Course;

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    errorMessage = "Another student is already using this email address.";
                }
                else
                {
                    errorMessage = "Database update error: " + ex.Message;
                }
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = "Unexpected error: " + ex.Message;
                return false;
            }
        }

        public bool DeleteStudent(int studentId, out string errorMessage)
        {
            errorMessage = string.Empty;

            const string query = "DELETE FROM Students WHERE StudentId = @StudentId;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@StudentId", MySqlDbType.Int32).Value = studentId;

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Database delete error: " + ex.Message;
                return false;
            }
        }
    }
}