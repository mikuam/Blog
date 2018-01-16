using System.Collections.Generic;

namespace MichalBialecki.com.Data.Dto
{
    public interface IFoldersAndFilesProvider
    {
        IEnumerable<Folder> GetFolders(int numberOfFilesOrFolders = 20, int structureDepth = 3);
    }
}
