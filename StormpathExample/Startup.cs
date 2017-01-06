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
using Microsoft.Owin;
using Owin;
using Stormpath.AspNet;
using Stormpath.SDK.Logging;
using LogLevel = Stormpath.SDK.Logging.LogLevel;

[assembly: OwinStartup(typeof(StormpathExample.Startup))]
namespace StormpathExample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // By default, the Stormpath SDK will look for the
            // API Key ID, API Key Secret, and Application href in environment variables.

            // It will also search the application root for a file called stormpath.yaml or stormpath.json.
            // This example project contains a stormpath.yaml file.
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StormpathMiddleware.log");

            app.UseStormpath(new StormpathMiddlewareOptions()
            {
                Logger = new FileLogger(logPath, LogLevel.Trace)
            });

            // You can optionally pass a configuration object instead.
            // Instantiate and pass an object to configure the SDK via code:
            //app.UseStormpath(new StormpathConfiguration
            //{
            //    Application = new ApplicationConfiguration
            //    {
            //        Href = "YOUR_APPLICATION_HREF"
            //    },
            //    Client = new ClientConfiguration
            //    {
            //        ApiKey = new ClientApiKeyConfiguration
            //        {
            //            Id = "YOUR_API_KEY_ID",
            //            Secret = "YOUR_API_KEY_SECRET"
            //        }
            //    }
            //});
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string _path;
        private readonly LogLevel _severity;

        public FileLogger(string path, LogLevel severity)
        {
            _path = path;
            _severity = severity;
        }

        public void Log(LogEntry entry)
        {
            if (entry.Severity < _severity)
            {
                return;
            }

            var logBuilder = new StringBuilder()
                .Append($"[{entry.Severity}] {entry.Source}: ");

            if (entry.Exception != null)
            {
                logBuilder.Append($"Exception {entry.Exception.GetType().Name} \"{entry.Exception.Message}\" in {entry.Exception.Source} ");
            }

            bool isMessageUseful = !string.IsNullOrEmpty(entry.Message)
                                   && !entry.Message.Equals(entry.Exception?.Message, StringComparison.Ordinal);
            if (isMessageUseful)
            {
                logBuilder.Append($"\"{entry.Message}\"");
            }

            logBuilder.Append("\n");

            File.AppendAllText(_path, logBuilder.ToString());
        }
    }
}
