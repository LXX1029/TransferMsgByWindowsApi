using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Windows.Forms;

namespace ReceiveMsgByWindowApi
{
    class Program
    {

        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                })
                .ConfigureHostConfiguration(config =>
                {

                }).Build();
            Application.Run(new ReceiveMsgForm());
        }
    }
}
