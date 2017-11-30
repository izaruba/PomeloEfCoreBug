using System;

namespace PomeloEfCoreBug
{
    internal class Program
    {
        internal const string Database = "pomelo_test";

        private static void Main()
        {
            // (!) Ensure DB `pomelo_test` is deleted before running the test (!)

            //ThisWorks();

            RunBug();
        }

        private static void ThisWorks()
        {
           // (!) NOTE(!) the first connection is valid

            Run("root", "@r00t@");              // DB CREATED 
            Run(string.Empty, string.Empty);    // Access denied for user ''@'localhost' (using password: NO)
            Run("root", "@r00t@");              // DB EXISTS
            Run(string.Empty, string.Empty);    // Access denied for user ''@'localhost' (using password: NO)
            Run("root", "@r00t@");              // DB EXISTS
        }

        private static void RunBug()
        {
            // (!) NOTE (!) the first connection is invalid

            Run(string.Empty, string.Empty);    // Access denied for user ''@'localhost' (using password: NO)
            Run("root", "@r00t@");              // Access denied for user ''@'localhost' (using password: NO) - BUG
            Run(string.Empty, string.Empty);    // Access denied for user ''@'localhost' (using password: NO)
            Run("root", "@r00t@");              // Access denied for user ''@'localhost' (using password: NO) - BUG
        }

        private static void Run(string name, string password)
        {
            var ctx = new ApplicationDbContext(name, password);

            try
            {
                var created = ctx.Database.EnsureCreated();

                Console.WriteLine(created ? "DB CREATED" : "DB EXISTS");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
