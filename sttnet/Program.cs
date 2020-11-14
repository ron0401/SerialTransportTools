using System;
using CommandLine;

namespace sttnet
{
    class Program
    {
        static void Main(string[] args)
        {
            Transporter trans;
            Parser.Default.ParseArguments<Transporter>(args)
                .WithParsed<Transporter>(opt => 
                {
                    opt.Start();
                    trans = opt;
                    })
                .WithNotParsed(er => {/*パースに失敗した場合*/});

            bool end = false;
            while (!end)
            {
                string command = Console.ReadLine();
                switch (command.ToLower())
                {
                    case "exit":
                        end = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }

}
