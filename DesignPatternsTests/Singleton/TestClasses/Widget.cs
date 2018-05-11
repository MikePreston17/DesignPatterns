﻿using System.Diagnostics;

namespace DesignPatterns.Tests
{
    class Widget
    {
        private readonly IEmailService service;
        private ILogger Logger { get; }

        public Widget(IEmailService service)
        {
            this.service = service;
            Logger = service.Logger;
            Debug.WriteLine(Logger.Name);
        }
    }
}