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

        private async Task CreateDatabase()
        {
            SQLiteConnection.CreateFile(_dbLocation);

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                await new SQLiteCommand(SQLQueries.CreateTables(), conn).ExecuteNonQueryAsync();

                await AddGroupWithConnection(conn, "Notes");
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

        public async Task<long> AddNote(string groupName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var group = await GetGroupWithConnection(conn, groupName);

                long groupID = 0;
                if (group == null)
                    groupID = await AddGroupWithConnection(conn, groupName);
                else
                    groupID = group.ID;

                using (var cmd = new SQLiteCommand(SQLQueries.AddNote(), conn))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@LastMOdified", DateTime.Now.ToString()));
                    cmd.Parameters.Add(new SQLiteParameter("@Content", ""));
                    cmd.Parameters.Add(new SQLiteParameter("@GroupID", groupID));

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
                    cmd.Parameters.Add(new SQLiteParameter("@Group", note.Group));

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

        public async Task<Group> GetGroup(string groupName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                return await GetGroupWithConnection(conn, groupName);
            }
        }

        public async Task<List<Group>> GetAllGroups()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                return await GetAllGroupsWithConnection(conn);
            }
        }

        public async Task<long> AddGroup(string groupName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                return await AddGroupWithConnection(conn, groupName);
            }
        }

        public Task UpdateGroup(string groupName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmdGet = new SQLiteCommand(SQLQueries.GetGroup(), conn);
                cmdGet.Parameters.Add(new SQLiteParameter("@Group", groupName.ToUpper()));

                var reader = cmdGet.ExecuteReader();
                int groupID = Convert.ToInt32(reader["ID"]);

                var cmdUpdate = new SQLiteCommand(SQLQueries.UpdateGroup(), conn);
                cmdUpdate.Parameters.Add(new SQLiteParameter("@Group", groupName.ToUpper()));
                cmdUpdate.Parameters.Add(new SQLiteParameter("@GroupID", groupID));

                return cmdUpdate.ExecuteNonQueryAsync();
            }
        }

        private async Task<Group> GetGroupWithConnection(SQLiteConnection conn, string groupName)
        {
            using (var cmd = new SQLiteCommand(SQLQueries.GetGroup(), conn))
            {
                cmd.Parameters.Add(new SQLiteParameter("@Group", groupName));

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                        return ReadGroupFromSQL(reader);
                    else
                        return null;
                }
            }
        }

        private async Task<List<Group>> GetAllGroupsWithConnection(SQLiteConnection conn)
        {
            using (var reader = await new SQLiteCommand(SQLQueries.GetAllGroups(), conn)
                                                 .ExecuteReaderAsync())
            {
                var groups = new List<Group>();

                while (reader.Read())
                    groups.Add(ReadGroupFromSQL(reader));

                return groups;
            }
        }

        private async Task<long> AddGroupWithConnection(SQLiteConnection conn, string groupName)
        {
            using (var cmd = new SQLiteCommand(SQLQueries.AddGroup(), conn))
            {
                cmd.Parameters.Add(new SQLiteParameter("@Group", groupName.ToUpper()));

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
                            ConvertFromDBVal<string>(reader["Group"]));
        }

        private Group ReadGroupFromSQL(DbDataReader reader)
        {
            return new Group(ConvertFromDBVal<int>(reader["ID"]),
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
