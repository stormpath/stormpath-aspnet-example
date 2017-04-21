// <copyright file="Startup.cs" company="Stormpath, Inc.">
// Copyright (c) 2016 Stormpath, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Owin;
using Owin;
using Stormpath.AspNet;
using Stormpath.Configuration.Abstractions;

[assembly: OwinStartup(typeof(StormpathExample.Startup))]
namespace StormpathExample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StormpathMiddleware.log");

            // By default, these environment variables will be seached for configuration:
            // OKTA_ORG
            // OKTA_APITOKEN
            // OKTA_APPLICATION_ID

            // You can optionally pass a configuration object instead.
            // Instantiate and pass an object to configure the SDK via code:
            var stormpathConfiguration = new StormpathConfiguration()
            {
                Org = "https://dev-123456.oktapreview.com/",
                ApiToken = "your_api_token",
                Application = new OktaApplicationConfiguration()
                {
                    Id = "abcd1234"
                }
            };

            app.UseStormpath(new StormpathMiddlewareOptions()
            {
                Logger = new FileLogger(logPath, LogLevel.Trace),
                Configuration = stormpathConfiguration
            });
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string _path;
        private readonly LogLevel _severity;
        private readonly object _lock = new object();

        public FileLogger(string path, LogLevel severity)
        {
            _path = path;
            _severity = severity;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel <= _severity;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logBuilder = new StringBuilder()
                .Append($"[{logLevel}] {eventId}: ");

            var message = formatter(state, exception);
            logBuilder.AppendLine(message);

            lock (_lock)
            {
                File.AppendAllText(_path, logBuilder.ToString());
            }
        }
    }
}
