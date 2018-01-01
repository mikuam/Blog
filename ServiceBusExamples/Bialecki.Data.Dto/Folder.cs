using System;
using System.Collections.Generic;

namespace Bialecki.Data.Dto
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

        public IReadOnlyCollection<Folder> Folders { get; set; }

        public IReadOnlyCollection<File> Files { get; set; }
    }
}
