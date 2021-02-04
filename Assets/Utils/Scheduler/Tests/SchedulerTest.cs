using System;
using System.Collections;
using Assets.AllyaExtension;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;
using NUnit.Framework;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Tests
{
    public static class SchedulerTests
    {
        [UnityTest]
        public static IEnumerator AssertThatMethodsAreExecutedInUpdate()
        {
            CreateScheduler();
            IAction action = Substitute.For<IAction>();
            Scheduler.OnUpdate.Subscribe(action.Execute);

            int random = Random.Range(0, 1000);
            yield return AwaitFrames(random);
            action.Received(random).Execute();
        }
        
        [UnityTest]
        public static IEnumerator AssertThatSubscribeOnceOnlyCallsMethodOneTime()
        {
            CreateScheduler();
            IAction action = Substitute.For<IAction>();
            Scheduler.OnUpdate.SubscribeOnce(action.Execute);
            int random = Random.Range(2, 1000);
            yield return AwaitFrames(random);
            action.Received(1).Execute();
            Assert.IsTrue(Scheduler.OnUpdate.Count == 0);
        }
        
        [UnityTest]
        public static IEnumerator AssertThatUnsubscribeWork()
        {
            CreateScheduler();
            IAction action = Substitute.For<IAction>();
            Scheduler.OnUpdate.Subscribe(action.Execute);
            
            int random = Random.Range(0, 1000);
            yield return AwaitFrames(random);
            
            Scheduler.OnUpdate.Unsubscribe(action.Execute);
            yield return null;
            
            action.Received(random).Execute();
            Assert.IsTrue(Scheduler.OnUpdate.Count == 0);
        }
        
        [UnityTest]
        public static IEnumerator AssertThatSubscribingDuringUpdateDoesNotThrowException()
        {
            CreateScheduler();
            IAction doubleAction = Substitute.For<IAction>();
            IAction action = Substitute.For<IAction>();

            doubleAction.When(x => x.Execute()).Do(x => Scheduler.OnUpdate.SubscribeOnce(action.Execute));
            Scheduler.OnUpdate.SubscribeOnce(doubleAction.Execute);
            yield return null;
            doubleAction.Received(1).Execute();
            yield return null;
            action.Received(1).Execute();
            Assert.IsTrue(Scheduler.OnUpdate.Count == 0);
        }
        
        [UnityTest]
        public static IEnumerator AssertThatUnsubscribingUnscheduledEventThrowsException()
        {
            CreateScheduler();
            IAction action = Substitute.For<IAction>();
            yield return null;
            Assert.Throws<ArgumentOutOfRangeException>(() => Scheduler.OnUpdate.Unsubscribe(action.Execute));
        }
        
        [UnityTest]
        public static IEnumerator AssertThatRegisteredActionExceptionDoesNotInfluenceOtherMethods()
        {
            CreateScheduler();
            IAction action1 = Substitute.For<IAction>();
            IAction action2 = Substitute.For<IAction>();
            Scheduler.OnUpdate.Subscribe(action1.Execute);
            Scheduler.OnUpdate.Subscribe(action2.Execute);
            action1.When(x => x.Execute()).Do(x => throw new Exception());
            yield return null;
            action2.Received(1).Execute();
        }
        

        private static void CreateScheduler()
        {
            if (!Object.FindObjectOfType<Scheduler>())
            {
                new GameObject("Scheduler").AddComponent<Scheduler>();
            }
            Scheduler.OnUpdate.Clear();
            Scheduler.OnFixedUpdate.Clear();
        }
        
        private static IEnumerator AwaitFrames(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return null;
            }
        }
    }

    public interface IAction
    {
        void Execute();
    }
}
