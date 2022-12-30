using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading;

namespace PasswordGenerator
{
    public class Program
    {
        private const string Conn = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PasswordGeneratorApp;Integrated Security=True";
        private const int datasize = 3000000;
        private const int num_task = 10;
        private static bool upper, lower, number, special;
       
        public static async Task Main(string[] args)
        {
            upper = lower = number = special = true;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Console.WriteLine("Processing");

            await insertall();
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }

        private static async Task insertall()
        {
            
            var tasks = new Task[num_task];
            
            for (int ctr = 1; ctr <= num_task; ctr++)
            { 
                tasks[ctr - 1] = Task.Run(async () => 
                {
                    SqlConnection sql = new SqlConnection(Conn);
                    sql.Open();
                    for (int i = 0; i < (datasize/num_task); i++)
                    {
                        var result = PasswordGenerate.GeneratePassword(upper, lower, number, special);
                        string s1 = $"Insert  into [dbo].[PasswordGenerator](Password) values('{result}')";
                        SqlCommand cmd = sql.CreateCommand();
                        cmd.CommandText = s1;
                        cmd.Connection = sql;
                        cmd.CommandType = CommandType.Text;
                        await cmd.ExecuteNonQueryAsync();
                       
                    }
                    sql.Close();
                });
                
            }
            var cont = Task.WhenAll(tasks);
            await cont;
        }  
    }

    public static class PasswordGenerate
    {
        private static string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string Number = "0123456789";
        private static string Specialchar = "!@#$%^&*()";

        public static string GeneratePassword(bool uppercase, bool lowercase, bool numbers, bool specialchar)
        {
            Random rand = new Random();
            string charSet = string.Empty;
            char[] password = new char[20];

            if (uppercase) { charSet += Upper; }
            if (lowercase) { charSet += Upper.ToLower(); }
            if (numbers) { charSet += Number; }
            if (specialchar) { charSet += Specialchar; }

            for (int i = 0; i < 20; i++)
            {
                password[i] = charSet[rand.Next(charSet.Length - 1)];
            }
            return string.Join(null, password);
        }
    }
}
