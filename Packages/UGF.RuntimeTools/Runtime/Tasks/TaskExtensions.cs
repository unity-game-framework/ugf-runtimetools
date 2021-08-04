using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UGF.RuntimeTools.Runtime.Tasks
{
    public static class TaskExtensions
    {
        public static IEnumerator WaitCoroutine(this Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                throw task.Exception ?? new Exception("Task has faulted.");
            }
        }

        public static async Task WaitAsync(this AsyncOperation operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            while (!operation.isDone)
            {
                await Task.Yield();
            }
        }

        public static async Task<Object> WaitAsync(this ResourceRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            while (!request.isDone)
            {
                await Task.Yield();
            }

            return request.asset ? request.asset : throw new NullReferenceException("Result of resource request is null.");
        }
    }
}
