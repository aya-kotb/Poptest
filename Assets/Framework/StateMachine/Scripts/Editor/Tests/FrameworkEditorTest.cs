using UnityEngine;
using UnityEditor;
using NUnit.Framework;

namespace Poptropica2
{
    public class FrameworkEditorTest
    {
        [Test]
        public void TestInstantiate()
        {
            SAMApplication.Instantiate();
            Assert.True(SAMApplication.mainInstance != null);
        }
        [Test]
        public void TestAddRemoveServices()
        {
            SAMApplication.Instantiate();
            //Act
            SAMApplication.mainInstance.AddService("QuestService", new QuestService());
            SAMApplication.mainInstance.AddService("QuestService2", new QuestService());
            SAMApplication.mainInstance.AddService("QuestService3", new QuestService());


            var serviceListCopy = SAMApplication.mainInstance.CopyServiceDictionary();
            //Assert
            Assert.AreEqual(serviceListCopy.Count, 3);
            SAMApplication.mainInstance.RemoveService<QuestService>("QuestService2");


            serviceListCopy = SAMApplication.mainInstance.CopyServiceDictionary();
            Assert.AreEqual(serviceListCopy.Count, 2);

            //Assert.Catch(TestBadRemoval);  // currently it returns null. Should it throw an exception?

            Assert.True(serviceListCopy.ContainsKey("QuestService"));
            Assert.True(serviceListCopy.ContainsKey("QuestService3"));
            Assert.False(serviceListCopy.ContainsKey("QuestService2"));

        }

        [Test]
        public void TestRemoveServicesExceptions()
        {
            SAMApplication.Instantiate();
            //Act
            SAMApplication.mainInstance.AddService("QuestService", new QuestService());
            SAMApplication.mainInstance.AddService("QuestService2", new QuestService());
            SAMApplication.mainInstance.AddService("QuestService3", new QuestService());


            Assert.Catch(TestBadRemoval);  // currently it returns null. Should it throw an exception?

        }

        void TestBadRemoval()
        {
            SAMApplication.mainInstance.RemoveService<QuestService>("QuestService5462");

        }
    }
}