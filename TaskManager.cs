using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CybersecurityChatbot
{
    public class CyberTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    class TaskManager
    {
        private string connectionString = "Server=localhost;Database=cyberchatbot;Uid=root;Pwd=;";
        private List<CyberTask> localTasks = new List<CyberTask>();
        private bool dbAvailable = false;

        public TaskManager()
        {
            TryInitDatabase();
        }

        private void TryInitDatabase()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"CREATE TABLE IF NOT EXISTS Tasks (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Title VARCHAR(255) NOT NULL,
                        Description TEXT,
                        Reminder VARCHAR(255),
                        IsCompleted BOOLEAN DEFAULT FALSE,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";
                    new MySqlCommand(sql, conn).ExecuteNonQuery();
                    dbAvailable = true;
                }
            }
            catch
            {
                dbAvailable = false;
            }
        }

        public string AddTask(string title, string description, string reminder = null)
        {
            var task = new CyberTask
            {
                Title = title,
                Description = description,
                Reminder = reminder,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };

            if (dbAvailable)
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string sql = "INSERT INTO Tasks (Title, Description, Reminder) VALUES (@t, @d, @r)";
                        var cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@t", title);
                        cmd.Parameters.AddWithValue("@d", description);
                        cmd.Parameters.AddWithValue("@r", reminder ?? "");
                        cmd.ExecuteNonQuery();
                        task.Id = (int)cmd.LastInsertedId;
                    }
                }
                catch { dbAvailable = false; }
            }

            localTasks.Add(task);

            string msg = $"Task added: '{title}' — {description}";
            if (!string.IsNullOrEmpty(reminder))
                msg += $" | Reminder: {reminder}";
            return msg;
        }

        public List<CyberTask> GetAllTasks()
        {
            if (dbAvailable)
            {
                try
                {
                    var tasks = new List<CyberTask>();
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        var cmd = new MySqlCommand("SELECT * FROM Tasks ORDER BY CreatedAt DESC", conn);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasks.Add(new CyberTask
                                {
                                    Id = reader.GetInt32("Id"),
                                    Title = reader.GetString("Title"),
                                    Description = reader.GetString("Description"),
                                    Reminder = reader.IsDBNull(reader.GetOrdinal("Reminder")) ? "" : reader.GetString("Reminder"),
                                    IsCompleted = reader.GetBoolean("IsCompleted"),
                                    CreatedAt = reader.GetDateTime("CreatedAt")
                                });
                            }
                        }
                    }
                    return tasks;
                }
                catch { }
            }
            return localTasks;
        }

        public string MarkCompleted(int index)
        {
            var tasks = GetAllTasks();
            if (index < 1 || index > tasks.Count)
                return "Invalid task number.";
            var task = tasks[index - 1];
            task.IsCompleted = true;
            if (dbAvailable)
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        var cmd = new MySqlCommand("UPDATE Tasks SET IsCompleted=TRUE WHERE Id=@id", conn);
                        cmd.Parameters.AddWithValue("@id", task.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch { }
            }
            return $"Task '{task.Title}' marked as completed!";
        }

        public string DeleteTask(int index)
        {
            var tasks = GetAllTasks();
            if (index < 1 || index > tasks.Count)
                return "Invalid task number.";
            var task = tasks[index - 1];
            if (dbAvailable)
            {
                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        var cmd = new MySqlCommand("DELETE FROM Tasks WHERE Id=@id", conn);
                        cmd.Parameters.AddWithValue("@id", task.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch { }
            }
            localTasks.RemoveAll(t => t.Title == task.Title);
            return $"Task '{task.Title}' deleted.";
        }

        public string FormatTaskList()
        {
            var tasks = GetAllTasks();
            if (tasks.Count == 0)
                return "You have no tasks yet. Type 'add task [title]' to add one.";
            string result = "Here are your tasks:\n";
            for (int i = 0; i < tasks.Count; i++)
            {
                string status = tasks[i].IsCompleted ? "✓" : "○";
                string rem = string.IsNullOrEmpty(tasks[i].Reminder) ? "" : $" | Reminder: {tasks[i].Reminder}";
                result += $"{i + 1}. [{status}] {tasks[i].Title}{rem}\n";
            }
            return result.TrimEnd();
        }
    }
}