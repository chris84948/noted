using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotedUI.Models;
using System.IO;
using System.Data.Common;
using System.Data.SQLite;

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
                SQLiteConnection.CreateFile(_dbLocation);

            await CreateTables();
        }
        
        private async Task CreateTables()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                await new SQLiteCommand(SQLQueries.CreateTables(), conn).ExecuteNonQueryAsync();
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
            return await AddNote(new Note() { CloudKey = null,
                                              Content = "",
                                              LastModified = DateTime.Now,
                                              Group = groupName });
        }

        public async Task<long> AddNote(Note note)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var group = await GetGroupWithConnection(conn, note.Group);

                long groupID = 0;
                if (group == null)
                    groupID = await AddGroupWithConnection(conn, note.Group);
                else
                    groupID = group.ID;

                using (var cmd = new SQLiteCommand(SQLQueries.AddNote(), conn))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@CloudKey", note.CloudKey));
                    cmd.Parameters.Add(new SQLiteParameter("@LastMOdified", note.LastModified.ToString()));
                    cmd.Parameters.Add(new SQLiteParameter("@Content", note.Content));
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

        public async Task UpdateGroup(string oldGroupName, string newGroupName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var oldGroup = await GetGroupWithConnection(conn, oldGroupName.ToUpper());

                var cmdUpdate = new SQLiteCommand(SQLQueries.UpdateGroup(), conn);
                cmdUpdate.Parameters.Add(new SQLiteParameter("@Group", newGroupName.ToUpper()));
                cmdUpdate.Parameters.Add(new SQLiteParameter("@GroupID", oldGroup.ID));

                await cmdUpdate.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteGroup(string groupName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmdDelete = new SQLiteCommand(SQLQueries.DeleteGroup(), conn);
                cmdDelete.Parameters.Add(new SQLiteParameter("@Group", groupName.ToUpper()));

                await cmdDelete.ExecuteNonQueryAsync();
            }
        }
        
        public async Task<bool> IsGroupExpanded(string groupName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand(SQLQueries.IsGroupExpanded(), conn);
                cmd.Parameters.Add(new SQLiteParameter("@Group", groupName.ToUpper()));

                long result = (long)(await cmd.ExecuteScalarAsync());

                return result > 0;
            }
        }

        public async Task InsertExpandedGroup(string groupName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand(SQLQueries.InsertExpandedGroup(), conn);
                cmd.Parameters.Add(new SQLiteParameter("@Group", groupName.ToUpper()));

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteExpandedGroup(string groupName)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand(SQLQueries.DeleteExpandedGroup(), conn);
                cmd.Parameters.Add(new SQLiteParameter("@Group", groupName.ToUpper()));

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task<string> GetSelectedNoteID()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand(SQLQueries.GetSelectedNote(), conn);

                return (string)(await cmd.ExecuteScalarAsync());
            }
        }
        
        public async Task InsertSelectedNoteID(string cloudKey)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand(SQLQueries.InsertSelectedNote(), conn);
                cmd.Parameters.Add(new SQLiteParameter("@NoteCloudKey", cloudKey));

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateSelectedNoteID(string cloudKey)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand(SQLQueries.UpdateSelectedNote(), conn);
                cmd.Parameters.Add(new SQLiteParameter("@NoteCloudKey", cloudKey));

                await cmd.ExecuteNonQueryAsync();
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
