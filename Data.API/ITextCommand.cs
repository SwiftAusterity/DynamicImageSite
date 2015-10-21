using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Site.Data.API
{
    public interface ITextCommand
    {
        String Command { get; set; }
        String Path { get; set; }
        bool Handled { get; set; }
        DateTime Created { get; set; }
    }
}
