using System.Collections.Generic;

namespace MichalBialecki.com.Data.Dto
{
    public class Folder
    {
        public Folder()
        {
            Folders = new List<Folder>();
            Files = new List<File>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public float Size { get; set; }

        public bool Hidden { get; set; }

        public List<Folder> Folders { get; set; }

        public List<File> Files { get; set; }
    }
}
