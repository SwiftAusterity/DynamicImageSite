using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API
{
    public interface ITextCommand
    {
        string Command { get; set; }
        string Path { get; set; }
        bool Handled { get; set; }
        DateTime Created { get; set; }
    }
}
