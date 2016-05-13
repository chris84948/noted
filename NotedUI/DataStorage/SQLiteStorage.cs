using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotedUI.Models;
using System.IO;
using System.Data.SQLite;
using System.Data.Common;

namespace NotedUI.DataStorage
{
    public class SQLiteStorage : ILocalStorage
    {
        private string connectionString;
        private string _dbLocation;

        public async Task Initialize()
        {
            _dbLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Noted.db";

            connectionString = $"Data Source={ _dbLocation };Version=3;";

            if (!File.Exists(_dbLocation))
                await CreateDatabase();
        }

        private Task CreateDatabase()
        {
            SQLiteConnection.CreateFile(_dbLocation);

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                return new SQLiteCommand(SQLQueries.CreateTables(), conn).ExecuteNonQueryAsync();
            }
        }

        public async Task<List<Note>> GetAllNotes()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var reader = await new SQLiteCommand(SQLQueries.GetAllNotes(), conn)
                                                     .ExecuteReaderAsync())
                {
                    var notes = new List<Note>();

                    while (reader.Read())
                        notes.Add(ReadNoteFromSQL(reader));

                    return notes;
                }
            }
        }

        public async Task<long> AddNote(string folderName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var folder = await GetFolderWithConnection(conn, folderName);

                long folderID = 0;
                if (folder == null)
                    folderID = await AddFolderWithConnection(conn, folderName);
                else
                    folderID = folder.ID;

                using (var cmd = new SQLiteCommand(SQLQueries.AddNote(), conn))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@LastMOdified", DateTime.Now.ToString()));
                    cmd.Parameters.Add(new SQLiteParameter("@Content", ""));
                    cmd.Parameters.Add(new SQLiteParameter("@FolderID", folderID));

                    await cmd.ExecuteNonQueryAsync();
                }

                using (var cmdGetID = new SQLiteCommand(SQLQueries.GetLastInsertedRowID(), conn))
                    return (long)cmdGetID.ExecuteScalar();
            }
        }

        public Task UpdateNote(Note note)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(SQLQueries.UpdateNote(), conn))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@ID", note.ID));
                    cmd.Parameters.Add(new SQLiteParameter("@CloudKey", note.CloudKey));
                    cmd.Parameters.Add(new SQLiteParameter("@LastModified", note.LastModified));
                    cmd.Parameters.Add(new SQLiteParameter("@Content", note.Content));
                    cmd.Parameters.Add(new SQLiteParameter("@Folder", note.Folder));

                    return cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public Task DeleteNote(Note note)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(SQLQueries.DeleteNote(), conn))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@ID", note.ID));

                    return cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Folder> GetFolder(string folderName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                return await GetFolderWithConnection(conn, folderName);
            }
        }

        public async Task<List<Folder>> GetAllFolders()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                return await GetAllFoldersWithConnection(conn);
            }
        }

        public async Task<long> AddFolder(string folderName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                return await AddFolderWithConnection(conn, folderName);
            }
        }

        public Task UpdateFolder(string folderName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmdGet = new SQLiteCommand(SQLQueries.GetFolder(), conn);
                cmdGet.Parameters.Add(new SQLiteParameter("@Folder", folderName));

                var reader = cmdGet.ExecuteReader();
                int folderID = Convert.ToInt32(reader["ID"]);

                var cmdUpdate = new SQLiteCommand(SQLQueries.UpdateFolder(), conn);
                cmdUpdate.Parameters.Add(new SQLiteParameter("@FolderID", folderID));

                return cmdUpdate.ExecuteNonQueryAsync();
            }
        }

        private async Task<Folder> GetFolderWithConnection(SQLiteConnection conn, string folderName)
        {
            using (var cmd = new SQLiteCommand(SQLQueries.GetAllFolders(), conn))
            {
                cmd.Parameters.Add(new SQLiteParameter("@Name", folderName));

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                        return ReadFolderFromSQL(reader);
                    else
                        return null;
                }
            }
        }

        private async Task<List<Folder>> GetAllFoldersWithConnection(SQLiteConnection conn)
        {
            using (var reader = await new SQLiteCommand(SQLQueries.GetAllFolders(), conn)
                                                 .ExecuteReaderAsync())
            {
                var folders = new List<Folder>();

                while (reader.Read())
                    folders.Add(ReadFolderFromSQL(reader));

                return folders;
            }
        }

        private async Task<long> AddFolderWithConnection(SQLiteConnection conn, string folderName)
        {
            using (var cmd = new SQLiteCommand(SQLQueries.AddFolder(), conn))
            {
                cmd.Parameters.Add(new SQLiteParameter("@Folder", folderName));

                await cmd.ExecuteNonQueryAsync();
            }

            using (var cmdGetID = new SQLiteCommand(SQLQueries.GetLastInsertedRowID(), conn))
                return (long)await cmdGetID.ExecuteScalarAsync();
        }

        private Note ReadNoteFromSQL(DbDataReader reader)
        {
            return new Note(ConvertFromDBVal<Int32>(reader["ID"]),
                            ConvertFromDBVal<string>(reader["CloudKey"]),
                            Convert.ToDateTime(reader["LastModified"]),
                            ConvertFromDBVal<string>(reader["Content"]),
                            ConvertFromDBVal<string>(reader["Folder"]));
        }

        private Folder ReadFolderFromSQL(DbDataReader reader)
        {
            return new Folder(ConvertFromDBVal<int>(reader["ID"]),
                              ConvertFromDBVal<string>(reader["Name"]));
        }

        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                try
                {
                    return (T)Convert.ChangeType(obj, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }
        }
    }
}
