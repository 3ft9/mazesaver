using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ReactionDiffusionSaver
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new ScreenSaverWindow().Run();
       }
    }
}
