using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utils.Providers
{
    public interface IFileProvider
    {
        StreamWriter AppendText(string path);
    }
}