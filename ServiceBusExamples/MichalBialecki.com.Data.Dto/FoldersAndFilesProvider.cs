using System;
using System.Collections.Generic;

namespace MichalBialecki.com.Data.Dto
{
    public class FoldersAndFilesProvider : IFoldersAndFilesProvider
    {
        Random random = new Random();
        private IEnumerable<Folder> folders;

        public IEnumerable<Folder> GetFolders(int numberOfFilesOrFolders = 20, int structureDepth = 3)
        {
            if (folders == null)
            {
                folders = GetTreeStructure(numberOfFilesOrFolders, structureDepth).Folders;
            }

            return folders;
        }

        public Folder GetTreeStructure(int numberOfFilesOrFolders, int structureDepth)
        {
            var id = Guid.NewGuid().ToString();

            return new Folder
            {
                Id = "folder_" + id,
                Name = "folderName_" + id,
                Size = random.Next(1, 200000),
                Hidden = false,
                Folders = GetFoldersRecursive(structureDepth, numberOfFilesOrFolders),
                Files = GetFiles(numberOfFilesOrFolders)
            };
        }

        private List<Folder> GetFoldersRecursive(int structureDepth, int numberOfFilesOrFolders)
        {
            var folders = new List<Folder>();

            if (structureDepth == 0)
            {
                return folders;
            }

            for (int i = 0; i < numberOfFilesOrFolders; i++)
            {
                var id = Guid.NewGuid().ToString();

                folders.Add(new Folder
                {
                    Id = "folder_" + id,
                    Name = "folderName_" + id,
                    Size = random.Next(1, 200000),
                    Hidden = random.Next(0, 100) > 50, 
                    Folders = GetFoldersRecursive(structureDepth - 1, (numberOfFilesOrFolders) / 2),
                    Files = GetFiles(random.Next(0, numberOfFilesOrFolders / 2))
                });
            }

            return folders;
        }

        private List<File> GetFiles(int numberOfFilesOrFolders)
        {
            var files = new List<File>();

            if (numberOfFilesOrFolders == 0)
            {
                return files;
            }
            
            for (int i = 0; i < numberOfFilesOrFolders; i++)
            {
                var id = Guid.NewGuid().ToString();

                files.Add(new File
                {
                    Id = "file_" + id,
                    Name = "fileName_" + id,
                    Size = random.Next(1, 200000),
                    Hidden = random.Next(0, 100) > 50
                });
            }

            return files;
        }
    }
}
