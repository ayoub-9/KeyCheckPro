using System.Data.SqlClient;

namespace EngineMova
{

    class Program
    {
        static void Main(string[] arge)
        {
            string connectionString = "Data Source=DESKTOP-UUBPBQN;Initial Catalog=users;User ID=admin;Password=admin;"; // Replace with your actual connection string

            Console.WriteLine("Weclome to EngineMova");

            Console.Write("Please enter your subscription key: ");
            string subscriptionKeyToCheck = Console.ReadLine();

            int daysLeft = CheckSubscriptionExpiry(connectionString, subscriptionKeyToCheck);

            if (daysLeft < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Subscription key is expired.");
            }
            else if (daysLeft == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Subscription key expires today.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"KeyCheckProis active enjoy (: - {daysLeft} days left until expiry.");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }

        static int CheckSubscriptionExpiry(string connectionString, string subscriptionKey)
        {
            string query = "SELECT ExpiryDate FROM Subscriptions WHERE SubscriptionKey = @SubscriptionKey";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SubscriptionKey", subscriptionKey);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        DateTime expiryDate = (DateTime)result;
                        TimeSpan timeLeft = expiryDate.Date - DateTime.Today;
                        return timeLeft.Days;
                    }

                    // If the subscription key is not found, return -1
                    return -1;
                }
            }
        }
    }
}
