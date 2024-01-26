using Microsoft.Data.SqlClient;

namespace HomeWork2
{
    internal class Program
    {
        static string connectionString = "Server=localhost;Database=MountainsChronicles;Trusted_Connection=True;TrustServerCertificate=True;";
        static void Main(string[] args)
        {
            AddMountain("Mount Everest", 8848, "Nepal", "Himalayas");
            UpdateMountain(1, "Mount Everest", 8850, "Nepal", "Himalayas");
            DeleteMountain(2);
            ShowClimbingGroupsForMountain(1);
            AddNewPeak("New Peak", 8000, "New Country", "New Region");
            EditPeak(3, "Edited Peak", 8100, "Edited Country", "Edited Region");
            ShowClimbersInDateRange(DateTime.Now.AddDays(-30), DateTime.Now);
            AddClimberToGroup(1, "John", "Doe", "Address");
            ShowClimbsCountPerMountainPerClimber();
            AddNewGroup("New Group", 3, DateTime.Now);
            ShowClimbersCountPerMountain();
        }

        static void AddMountain(string name, int height, string country, string region)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO Mountains (Name, Height, Country, Region) VALUES (@Name, @Height, @Country, @Region)", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Height", height);
                    command.Parameters.AddWithValue("@Country", country);
                    command.Parameters.AddWithValue("@Region", region);
                    command.ExecuteNonQuery();
                }
            }
        }
        static void UpdateMountain(int id, string name, int height, string country, string region)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UPDATE Mountains SET Name = @Name, Height = @Height, Country = @Country, Region = @Region WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Height", height);
                    command.Parameters.AddWithValue("@Country", country);
                    command.Parameters.AddWithValue("@Region", region);
                    command.ExecuteNonQuery();
                }
            }
        }

        static void DeleteMountain(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Mountains WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        static void ShowClimbingGroupsForMountain(int mountainId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM ClimbingGroups WHERE MountainId = @MountainId ORDER BY StartTime", connection))
                {
                    command.Parameters.AddWithValue("@MountainId", mountainId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Group Id: {reader["GroupId"]}, Mountain Id: {reader["MountainId"]}, StartTime: {reader["StartTime"]}");
                        }
                    }
                }
            }
        }
        static void AddNewPeak(string name, int height, string country, string region)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO Mountains (Name, Height, Country, Region) VALUES (@Name, @Height, @Country, @Region)", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Height", height);
                    command.Parameters.AddWithValue("@Country", country);
                    command.Parameters.AddWithValue("@Region", region);
                    command.ExecuteNonQuery();
                }
            }
        }
        static void EditPeak(int mountainId, string name, int height, string country, string region)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UPDATE Mountains SET Name = @Name, Height = @Height, Country = @Country, Region = @Region WHERE Id = @MountainId", connection))
                {
                    command.Parameters.AddWithValue("@MountainId", mountainId);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Height", height);
                    command.Parameters.AddWithValue("@Country", country);
                    command.Parameters.AddWithValue("@Region", region);
                    command.ExecuteNonQuery();
                }
            }
        }
        static void ShowClimbersInDateRange(DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Climbers WHERE EXISTS (SELECT 1 FROM Climbs WHERE Climbs.ClimberId = Climbers.ClimberId AND Climbs.StartDate BETWEEN @StartDate AND @EndDate)", connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Climber Id: {reader["ClimberId"]}, First Name: {reader["FirstName"]}, Last Name: {reader["LastName"]}");
                        }
                    }
                }
            }
        }
        static void AddClimberToGroup(int groupId, string firstName, string lastName, string address)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO Climbers (FirstName, LastName, Address) VALUES (@FirstName, @LastName, @Address); INSERT INTO Climbs (ClimberId, GroupId, StartDate, EndDate) VALUES (SCOPE_IDENTITY(), @GroupId, GETDATE(), GETDATE())", connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@GroupId", groupId);
                    command.ExecuteNonQuery();
                }
            }
        }
        static void ShowClimbsCountPerMountainPerClimber()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Climbers.ClimberId, Climbers.FirstName, Climbers.LastName, Mountains.Name AS MountainName, COUNT(*) AS ClimbsCount FROM Climbers INNER JOIN Climbs ON Climbers.ClimberId = Climbs.ClimberId INNER JOIN ClimbingGroups ON Climbs.GroupId = ClimbingGroups.GroupId INNER JOIN Mountains ON ClimbingGroups.MountainId = Mountains.MountainId GROUP BY Climbers.ClimberId, Climbers.FirstName, Climbers.LastName, Mountains.Name", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Climber Id: {reader["ClimberId"]}, First Name: {reader["FirstName"]}, Last Name: {reader["LastName"]}, Mountain: {reader["MountainName"]}, Climbs Count: {reader["ClimbsCount"]}");
                        }
                    }
                }
            }
        }
        static void AddNewGroup(string groupName, int mountainId, DateTime startTime)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO ClimbingGroups (MountainId, StartTime) VALUES (@MountainId, @StartTime); INSERT INTO Climbers (FirstName, LastName, Address) VALUES ('Default', 'Climber', 'Default Address'); INSERT INTO Climbs (ClimberId, GroupId, StartDate, EndDate) VALUES (SCOPE_IDENTITY(), SCOPE_IDENTITY(), GETDATE(), GETDATE())", connection))
                {
                    command.Parameters.AddWithValue("@MountainId", mountainId);
                    command.Parameters.AddWithValue("@StartTime", startTime);
                    command.ExecuteNonQuery();
                }
            }
        }
        static void ShowClimbersCountPerMountain()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Mountains.MountainId, Mountains.Name AS MountainName, COUNT(DISTINCT Climbers.ClimberId) AS ClimbersCount FROM Mountains LEFT JOIN ClimbingGroups ON Mountains.MountainId = ClimbingGroups.MountainId LEFT JOIN Climbs ON ClimbingGroups.GroupId = Climbs.GroupId LEFT JOIN Climbers ON Climbs.ClimberId = Climbers.ClimberId GROUP BY Mountains.MountainId, Mountains.Name", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Mountain Id: {reader["MountainId"]}, Mountain Name: {reader["MountainName"]}, Climbers Count: {reader["ClimbersCount"]}");
                        }
                    }
                }
            }
        }
    }
}
