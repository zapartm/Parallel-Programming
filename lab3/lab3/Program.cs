using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = GetJankielsNumber();
            MailBox mailBox = new MailBox(N);

            var jankiels = ImportAndInitiateJankels(mailBox);
            foreach (var j in jankiels)
            {
                j.FindNeighbours(jankiels);
            }

            Server server = new Server(N, mailBox, jankiels);
            Thread serverThread = new Thread(new ThreadStart(server.DoWork));
            serverThread.Name = "Server";
            serverThread.Start();

            for (int i = 0; i < N; i++)
            {
                Thread thread = new Thread(new ThreadStart(jankiels[i].DoWork));
                thread.Name = "Jankiel " + i.ToString();
                thread.Start();
            }

        }


        private static List<Jankiel> ImportAndInitiateJankels(MailBox mail)
        {
            var result = new List<Jankiel>();

            string[] allLines = File.ReadAllLines(@".\positions.txt");

            var query = from line in allLines
                        let data = line.Split(' ')
                        where data.Length > 1
                        select new
                        {
                            X = int.Parse(data[0]),
                            Y = int.Parse(data[1])
                        };

            int i = 0;
            foreach (var v in query)
            {
                result.Add(new Jankiel(i++, v.X, v.Y, mail));
            }

            return result;
        }


        private static int GetJankielsNumber()
        {
            string[] allLines = File.ReadAllLines(@".\positions.txt");
            return allLines.Length - 1;
        }
    }
}
