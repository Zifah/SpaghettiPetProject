using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FMTest.Controllers
{
    public class AttendanceController : ApiController
    {
        // GET: api/Attendance
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Attendance/5
        public List<Student> Get(int year, int month, int day, string theClass)
        {
            var date = new DateTime(year, month, day);

            return GetStudents(date, theClass);
        }

        // POST: api/Attendance
        public void Post([FromBody]string value)
        {
        }

        public List<Student> GetStudents(DateTime date, string theClass)
        {
            List<Student> students = new List<Student>();
            try
            {
                string connectionString = @"Data Source=C:\Users\p6613\Documents\Personal\FMDQTests.sqlite; 
Version=3; FailIfMissing=True; Foreign Keys=True;";
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = string.Format(
                        "select * from students where id in (select studentid from attendance where date = '{0}') and class = '{1}'",
                        date.ToString("dd/MM/yyyy"), theClass);

                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                    using (cmd)
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Student student = new Student
                                {
                                    Age = Convert.ToInt32(reader["Age"]),
                                    Class = Convert.ToString(reader["Class"]),
                                    Name = Convert.ToString(reader["Name"])
                                };

                                students.Add(student);
                            }
                        }
                    }
                    conn.Close();
                }
            }

            catch (SQLiteException e)
            {
                throw;
            }

            return students;
        }
    }

    public class Student
    {
        public string Name { set; get; }
        public int Age { set; get; }
        public string Class { set; get; }
    }
}
