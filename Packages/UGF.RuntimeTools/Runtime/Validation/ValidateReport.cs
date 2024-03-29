﻿using System;
using System.Collections.Generic;

namespace UGF.RuntimeTools.Runtime.Validation
{
    public readonly struct ValidateReport
    {
        public IList<ValidateMemberResult> Results { get { return m_results ?? throw new ArgumentException("Value not specified."); } }
        public bool HasResults { get { return m_results != null && m_results.Count > 0; } }

        private readonly IList<ValidateMemberResult> m_results;

        public ValidateReport(IList<ValidateMemberResult> results)
        {
            m_results = results ?? throw new ArgumentNullException(nameof(results));
        }

        public bool IsValid()
        {
            return m_results != null;
        }

        public bool HasAnyValid()
        {
            if (!IsValid()) throw new ArgumentException("Value should be valid.");

            for (int i = 0; i < m_results.Count; i++)
            {
                if (m_results[i].Result)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasAnyInvalid()
        {
            if (!IsValid()) throw new ArgumentException("Value should be valid.");

            for (int i = 0; i < m_results.Count; i++)
            {
                if (!m_results[i].Result)
                {
                    return true;
                }
            }

            return false;
        }

        public List<ValidateMemberResult> GetByMemberResults(string memberName)
        {
            var results = new List<ValidateMemberResult>();

            TryGetByMemberResults(memberName, results);

            return results;
        }

        public bool TryGetByMemberResults(string memberName, ICollection<ValidateMemberResult> results)
        {
            if (string.IsNullOrEmpty(memberName)) throw new ArgumentException("Value cannot be null or empty.", nameof(memberName));
            if (results == null) throw new ArgumentNullException(nameof(results));
            if (!IsValid()) throw new ArgumentException("Value should be valid.");

            bool result = false;

            for (int i = 0; i < m_results.Count; i++)
            {
                ValidateMemberResult memberResult = m_results[i];

                if (memberResult.Member.Name == memberName)
                {
                    result = true;
                    results.Add(memberResult);
                }
            }

            return result;
        }
    }
}
