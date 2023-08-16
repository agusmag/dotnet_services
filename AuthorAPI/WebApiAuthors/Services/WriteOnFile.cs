using System;
using System.IO;

namespace WebApiAuthors.Services
{
	public class WriteOnFile : IHostedService
	{
        private readonly IWebHostEnvironment env;
        private readonly string FileName = "File 1.txt";
        private Timer timer;

        public WriteOnFile(IWebHostEnvironment env)
		{
            this.env = env;
        }

        // Will be executed once at API startup
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Write("Process Initialized");
            return Task.CompletedTask;
        }

        // Will be executed at the end of the API (grafecul)
        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            Write("Process Finalized");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Write("Process execution: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        private void Write(string message)
        {
            var path = $@"{env.ContentRootPath}/wwwroot/{FileName}";
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(message);
            }
        }
    }
}

