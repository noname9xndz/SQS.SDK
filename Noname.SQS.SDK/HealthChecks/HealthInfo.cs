﻿using System;

namespace Noname.SQS.SDK.HealthChecks
{
    public class HealthInfo
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
    }
}