using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Site.Data.API;
using Site.Data.API.Repository;

using Ninject;

namespace Site.Data.DTO
{
    [Serializable]
    public class TextCommand : ITextCommand
    {
        public String Command { get; set; }
        public String Path { get; set; }
        public bool Handled { get; set; }
        public DateTime Created { get; set; }
    }
}
