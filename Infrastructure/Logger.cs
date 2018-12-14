using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lomtseu {
    public class Logger {
        private static readonly ConsoleColor defaultColor = ConsoleColor.White;

        public void Log(String text, Nullable<ConsoleColor> color = null) {
            this.WriteWithColor("> ", ConsoleColor.Green);
            this.WriteWithColor(String.Format("[{0}] ", DateTime.Now.ToString("hh:mm:ss")), ConsoleColor.Yellow);
            this.WriteWithColor(text, color ?? Logger.defaultColor);
            Console.WriteLine();
        }

        public void WriteWithColor(String text, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}