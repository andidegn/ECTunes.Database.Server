using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECTunes.Database.Util;
using ECTunesDB;

namespace ECTunes.Database_Server {
    class Program {
        static void Main(string[] args) {

            DbConnector db = new DbConnector();

            //Server s1 = new Server();
            //while (true) {
            //    System.Threading.Thread.Sleep(5000);
            //}

            Console.Write("Server started at: ");
            Console.WriteLine(DateTime.Now);
            try {
                db.IsConnected();
                Server s = new Server();
                Console.WriteLine("Server Running...");
                Console.WriteLine(s.GetServerInfo());
                Console.Write("Press [s] for server info or [q] to quit!");
                Enter();
                while (true) {
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.S) {
                        Console.Write("\n" + s.GetServerInfo());
                        Enter();
                    }
                    if (key == ConsoleKey.Q) {
                        Console.Write("\nAre you sure you want to exit the server? Clients will be disconnected. [y/n]");
                        Enter();
                        do {
                            key = Console.ReadKey().Key;
                            if (key == ConsoleKey.Y) {
                                Console.Write("Exiting server...");
                                return;
                            }
                            else if (key == ConsoleKey.N) {
                                Console.Write("\nCanceled");
                                Enter();
                                break;
                            }
                        } while (true);
                    }
                }
            }
            catch (Exception) {
                Console.WriteLine("An error has occurred, press any key to exit...");
                Console.Read();
            }
        }

        private static void Enter() {
            Console.Write("\n:>");
        }

        private static String NoConnection(Exception msg) {
            StringBuilder sb = new StringBuilder();
            sb.Append("No connection to the server!");
                sb.Append("'\nException:\n");
                sb.Append(msg.Data);
                sb.Append("\nMsg:\n");
                sb.Append(msg.Message);
                sb.Append("\nStackTrace:\n");
                sb.Append(msg.StackTrace);
                sb.Append("\nInnerException:\n");
                sb.Append(msg.InnerException);
            return sb.ToString();
        }
    }
}
