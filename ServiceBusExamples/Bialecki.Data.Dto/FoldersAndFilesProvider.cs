using System;
using System.Collections.Generic;

namespace Bialecki.Data.Dto
{
    public class FoldersAndFilesProvider
    {
        public Folder GetTreeStructure(int numberOfFilesOrFolders, int structureDepth)
        {
            var id = Guid.NewGuid().ToString();

            return new Folder
            {
                Id = "folder_" + id,
                Name = "folderName_" + id,
                Size = (new Random()).Next(1, 200000),
                Hidden = false,
                Folders = GetFolders((new Random()).Next(0, structureDepth - 1), numberOfFilesOrFolders - 1),
                Files = GetFiles(numberOfFilesOrFolders)
            };
        }

        private List<Folder> GetFolders(int structureDepth, int numberOfFilesOrFolders)
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
                    Size = (new Random()).Next(1, 200000),
                    Hidden = (new Random()).Next(0, 1) > 0, 
                    Folders = GetFolders((new Random()).Next(0, structureDepth - 1), numberOfFilesOrFolders - 1)
                });
            }

            return folders;
        }

        private List<File> GetFiles(int numberOfFilesOrFolders)
        {
            var folders = new List<File>();

            for (int i = 0; i < numberOfFilesOrFolders; i++)
            {
                var id = Guid.NewGuid().ToString();

                folders.Add(new File
                {
                    Id = "file_" + id,
                    Name = "fileName_" + id,
                    Size = (new Random()).Next(1, 200000),
                    Hidden = (new Random()).Next(0, 1) > 0,
                });
            }

            return folders;
        }
    }
}
