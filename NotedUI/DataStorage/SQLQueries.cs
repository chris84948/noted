namespace NotedUI.DataStorage
{
    public class SQLQueries
    {
        public static string CreateTables()
        {
            return
                @"  CREATE TABLE IF NOT EXISTS Notes 
                    (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        CloudKey TEXT,
                        LastModified TEXT,
                        Content TEXT,
                        GroupID INTEGER
                    );

                    CREATE TABLE IF NOT EXISTS Groups 
                    (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT
                    );

                    CREATE TABLE IF NOT EXISTS UserSettings
                    (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        KeyName TEXT,
                        KeyValue TEXT
                    );

                    CREATE VIEW IF NOT EXISTS AllNotes AS
                        SELECT
                            notes.ID AS ID,
                            CloudKey,
                            LastModified,
                            Content,
                            groups.Name AS [Group]
                        FROM
                            Notes notes
                            LEFT JOIN Groups groups ON notes.GroupID = groups.ID;";
        }

        public static string GetLastInsertedRowID()
        {
            return
                @"SELECT last_insert_rowid()";
        }

        public static string GetAllNotes()
        {
            return
                @"SELECT * FROM AllNotes";
        }

        public static string GetNote()
        {
            return
                @"SELECT * FROM AllNotes
                  WHERE ID = @ID";
        }

        public static string AddNote()
        {
            return
                @"INSERT INTO Notes (CloudKey, LastModified, Content, GroupID)
                  VALUES (@CloudKey, @LastModified, @Content, @GroupID)";
        }

        public static string UpdateNote()
        {
            return
                @"UPDATE Notes 
                  SET LastModified = @LastModified, 
                      Content = @Content,
                      GroupID = (SELECT Groups.ID FROM Groups
                               WHERE Groups.Name = @Group)
                  WHERE CloudKey = @CloudKey";
        }

        public static string DeleteNote()
        {
            return
                @"DELETE FROM Notes
                  WHERE ID = @ID";
        }

        public static string AddGroup()
        {
            return
                @"INSERT INTO Groups (Name)
                  VALUES (@Group)";
        }

        public static string GetAllGroups()
        {
            return
                @"SELECT * FROM Groups";
        }

        public static string GetGroup()
        {
            return
                @"SELECT * FROM Groups
                  WHERE Name = @Group;";
        }

        public static string UpdateGroup()
        {
            return
                @"UPDATE Groups
                  SET Name = @Group
                  WHERE ID = @GroupID;";
        }

        public static string DeleteGroup()
        {
            return
                @"DELETE FROM Groups
                  WHERE Name = @Group;";
        }

        public static string IsGroupExpanded()
        {
            return
              @"SELECT EXISTS
                (
                    SELECT 1 FROM UserSettings
                    WHERE KeyName = 'ExpandedGroups' AND KeyValue = @Group
                );";
        }


        public static string GetUserSetting(string keyName)
        {
            return
                $@" Select KeyValue 
                    FROM UserSettings 
                    WHERE KeyName = '{ keyName }';";
        }

        public static string InsertUserSetting(string keyName)
        {
            return
                $@" INSERT INTO UserSettings (KeyName, KeyValue)
                    Values ('{ keyName }', @KeyValue);";
        }

        public static string UpdateUserSetting(string keyName)
        {
            return
                $@" UPDATE UserSettings
                    SET KeyValue = @KeyValue
                    WHERE KeyName = '{ keyName }';";
        }

        public static string DeleteUserSetting(string keyName)
        {
            return
                $@" DELETE FROM UserSettings
                    WHERE KeyName = '{ keyName }' AND KeyValue = @KeyValue;";
        }
    }
}