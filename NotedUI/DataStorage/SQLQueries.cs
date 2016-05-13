namespace NotedUI.DataStorage
{
    public class SQLQueries
    {
        public static string CreateTables()
        {
            return
                @"CREATE TABLE Notes (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CloudKey TEXT,
                    LastModified TEXT,
                    Content TEXT,
                    FolderID INTEGER
                );

                CREATE TABLE Folders (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT
                );

                CREATE VIEW AllNotes AS
                    SELECT
                        notes.ID AS ID,
                        CloudKey,
                        LastModified,
                        Content,
                        folders.Name AS Folder
                    FROM
                        Notes notes
                        LEFT JOIN Folders folders ON (notes.FolderID = folders.ID);";
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
                @"INSERT INTO Notes (CloudKey, LastModified, Content, FolderID)
                  VALUES (null, @LastModified, @Content, @FolderID)";
        }

        public static string UpdateNote()
        {
            return
                @"UPDATE Notes 
                  SET CloudKey = @CloudKey,
                      LastModified = @LastModified, 
                      Content = @Content,
                      FolderID = (SELECT Folders.ID FROM Folders
                               WHERE Folders.Name = @Folder)
                  WHERE ID = @ID";
        }

        public static string DeleteNote()
        {
            return
                @"DELETE FROM Notes
                  WHERE ID = @ID";
        }

        public static string AddFolder()
        {
            return
                @"INSERT INTO Folders (Name)
                  VALUES (@Folder)";
        }

        public static string GetAllFolders()
        {
            return
                @"SELECT * FROM Folders";
        }

        public static string GetFolder()
        {
            return
                @"SELECT * FROM Folders
                  WHERE Name = @Folder;";
        }

        public static string UpdateFolder()
        {
            return
                @"UPDATE Folders
                  SET Name = @Folder
                  WHERE FolderID = @FolderID;";
        }

        public static string DeleteFolder()
        {
            return
                @"DELETE FROM Folders
                  WHERE FolderID = @FolderID;";
        }
    }
}