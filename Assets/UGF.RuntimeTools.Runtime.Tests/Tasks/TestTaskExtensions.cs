using System;
using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Tasks;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace UGF.RuntimeTools.Runtime.Tests.Tasks
{
    public class TestTaskExtensions
    {
        [UnityTest]
        public IEnumerator WaitCoroutine()
        {
            Task<int> task = TestWaitCoroutineAsync();

            yield return task.WaitCoroutine();

            int result = task.Result;

            Assert.AreEqual(10, result);
        }

        [UnityTest]
        public IEnumerator WaitAsyncOperation()
        {
            Task<Object> task = Resources.LoadAsync<Material>("Material_1").WaitAsync();

            yield return task.WaitCoroutine();

            Object result = task.Result;

            Assert.NotNull(result);
            Assert.IsInstanceOf<Material>(result);
            Assert.AreEqual("Material_1", result.name);
        }

        [UnityTest, UnityPlatform(RuntimePlatform.WindowsEditor)]
        public IEnumerator ExceptionCatchTests()
        {
            Task task = TestException();

            yield return task.WaitCoroutine();
            yield return null;

            Assert.Fail("No Exception");
        }

        private async Task<int> TestWaitCoroutineAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Yield();
            }

            return 10;
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
