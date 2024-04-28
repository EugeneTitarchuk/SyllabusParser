using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vnau.SyllabusParser.Cli
{
    internal class Worker
    {
        ILogger _logger;

        public Worker(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Run(AppOptions options)
        {

        }
    }
}
