using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libTools
{
    public interface IItem
    {
        void Save();
        void Save(string filename);
        void Export(string filename);
    }
}
