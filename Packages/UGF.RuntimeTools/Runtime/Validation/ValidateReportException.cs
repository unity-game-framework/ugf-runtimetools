﻿using System;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public class ValidateReportException : AggregateException
    {
        public ValidateReport Report { get; }

        public ValidateReportException(ValidateReport report) : base("Values are invalid.", GetExceptions(report))
        {
            Report = report;
        }

        private static IEnumerable<Exception> GetExceptions(ValidateReport report)
        {
            if (!report.IsValid()) throw new ArgumentException("Value should be valid.", nameof(report));

            var exceptions = new Exception[report.Results.Count];

            for (int i = 0; i < report.Results.Count; i++)
            {
                exceptions[i] = new ValidateResultException(report.Results[i]);
            }

            return exceptions;
        }
    }
}
