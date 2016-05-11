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

                var notes = new List<Note>();

                var reader = await new SQLiteCommand(SQLQueries.GetAllNotes(), conn)
                                                     .ExecuteReaderAsync();

                while (reader.Read())
                    notes.Add(ReadNoteFromSQL(reader));

                return notes;
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

                var cmd = new SQLiteCommand(SQLQueries.AddNote(), conn);
                cmd.Parameters.Add(new SQLiteParameter("@Content", ""));
                cmd.Parameters.Add(new SQLiteParameter("@FolderID", folderName));

                await cmd.ExecuteNonQueryAsync();

                var cmdGetID = new SQLiteCommand(SQLQueries.GetLastInsertedRowID(), conn);
                return (long)await cmdGetID.ExecuteScalarAsync();
            }
        }

        public Task UpdateNote(Note note)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand(SQLQueries.UpdateNote(), conn);
                cmd.Parameters.Add(new SQLiteParameter("@ID", note.ID));
                cmd.Parameters.Add(new SQLiteParameter("@CloudKey", note.CloudKey));
                cmd.Parameters.Add(new SQLiteParameter("@LastModified", note.LastModified));
                cmd.Parameters.Add(new SQLiteParameter("@Content", note.Content));
                cmd.Parameters.Add(new SQLiteParameter("@Folder", note.Folder));

                return cmd.ExecuteNonQueryAsync();
            }
        }

        public Task DeleteNote(Note note)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand(SQLQueries.DeleteNote(), conn);
                cmd.Parameters.Add(new SQLiteParameter("@ID", note.ID));

                return cmd.ExecuteNonQueryAsync();
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
            var cmd = new SQLiteCommand(SQLQueries.GetAllFolders(), conn);
            cmd.Parameters.Add(new SQLiteParameter("@Name", folderName));
            var reader = await cmd.ExecuteReaderAsync();

            if (reader.Read())
                return ReadFolderFromSQL(reader);
            else
                return null;
        }

        private async Task<List<Folder>> GetAllFoldersWithConnection(SQLiteConnection conn)
        {
            var folders = new List<Folder>();

            var reader = await new SQLiteCommand(SQLQueries.GetAllFolders(), conn)
                                                 .ExecuteReaderAsync();

            while (reader.Read())
                folders.Add(ReadFolderFromSQL(reader));

            return folders;
        }

        private async Task<long> AddFolderWithConnection(SQLiteConnection conn, string folderName)
        {
            var cmd = new SQLiteCommand(SQLQueries.AddFolder(), conn);
            cmd.Parameters.Add(new SQLiteParameter("@Folder", folderName));

            await cmd.ExecuteNonQueryAsync();

            var cmdGetID = new SQLiteCommand(SQLQueries.GetLastInsertedRowID(), conn);
            return (long)await cmdGetID.ExecuteScalarAsync();
        }

        private Note ReadNoteFromSQL(DbDataReader reader)
        {
            return new Note(Convert.ToInt32(reader["ID"]),
                            reader["CloudKey"].ToString(),
                            Convert.ToDateTime(reader["LastModified"]),
                            reader["Content"].ToString(),
                            reader["Folder"].ToString());
        }

        private Folder ReadFolderFromSQL(DbDataReader reader)
        {
            return new Folder(Convert.ToInt32(reader["ID"]),
                              reader["Name"].ToString());
        }
    }
}
