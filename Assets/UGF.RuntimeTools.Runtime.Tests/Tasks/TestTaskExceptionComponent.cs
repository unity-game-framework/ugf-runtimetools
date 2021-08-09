using System;
using System.Collections;
using System.Threading.Tasks;
using UGF.RuntimeTools.Runtime.Tasks;
using UnityEngine;

namespace UGF.RuntimeTools.Runtime.Tests.Tasks
{
    public class TestTaskExceptionComponent : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return TestException().WaitCoroutine();

            throw new Exception("Start Exception");
        }

        private async Task TestException()
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Yield();
            }

            throw new Exception("Test Exception");
        }
    }
}
