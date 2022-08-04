using System;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public readonly struct ValidateReport
    {
        public IList<ValidateMemberResult> Results { get { return m_results ?? throw new ArgumentException("Value not specified."); } }
        public bool HasResults { get { return m_results.Count > 0; } }

        private readonly IList<ValidateMemberResult> m_results;

        public ValidateReport(IList<ValidateMemberResult> results)
        {
            m_results = results ?? throw new ArgumentNullException(nameof(results));
        }

        public bool IsValid()
        {
            return HasResults;
        }
    }
}
