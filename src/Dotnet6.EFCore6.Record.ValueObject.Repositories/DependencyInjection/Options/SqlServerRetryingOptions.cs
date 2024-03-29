﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories.DependencyInjection.Options
{
    public class SqlServerRetryingOptions
    {
        [Required, Range(5, 20)]
        public int MaxRetryCount { get; init; }

        [Required, Range(5, 20)]
        public int MaxSecondsRetryDelay { get; init; }

        public int[] ErrorNumbersToAdd { get; init; }

        internal TimeSpan MaxRetryDelay
            => TimeSpan.FromSeconds(MaxSecondsRetryDelay);
    }
}