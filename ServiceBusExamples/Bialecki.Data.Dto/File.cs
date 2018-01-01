using System;
using System.Collections.Generic;
using System.Text;

namespace Bialecki.Data.Dto
{
    public class File
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public float Size { get; set; }

        public bool Hidden { get; set; }
    }
}
