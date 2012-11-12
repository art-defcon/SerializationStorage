using System.Linq;
using NUnit.Framework;
using SerializationStorage;

namespace SerializationStorageTests
{
    [TestFixture]
    public class StorageTests
    {
        [Test]
        public void CreateStorageTest()
        {
            var test = new TestClasse
                           {
                               PropertyOne = "test",
                               PropertyTwo = 1,
                           };
            var store = new SerializationStorage<TestClasse> {test};

            Assert.IsTrue(store.Any());
        }

        /// <summary>
        /// The purpose of this test is to see if file IO locks
        /// </summary>
        [Test]
        public void TestWriteTwice()
        {
            var test = new TestClasse
            {
                PropertyOne = "test",
                PropertyTwo = 1,
            };

            using (var store = new SerializationStorage<TestClasse>())
            {
                store.Add(test);
            }

            using (var store = new SerializationStorage<TestClasse>())
            {
                Assert.IsTrue(store.Last().PropertyOne == "test");
                store.Last().PropertyTwo = 2;
            }

            using (var store = new SerializationStorage<TestClasse>())
            {
                Assert.IsTrue(store.Last().PropertyTwo == 2);
            }
        }

        /// <summary>
        /// Storage should selfdelete if disposed empty
        /// </summary>
        [Test]
        public void CleanStorageTest()
        {
            var test = new TestClasse
            {
                PropertyOne = "test",
                PropertyTwo = 1,
            };

            using (var store = new SerializationStorage<TestClasse>())
            {
                store.Add(test);
            }
            using (var store = new SerializationStorage<TestClasse>())
            {
                store.Clear();
            }

            using (var store = new SerializationStorage<TestClasse>())
            {
                Assert.IsFalse(store.Any());
            }
        }

        [Test]
        public void ShouldOverWriteFile()
        {
            var test = new TestClasse
            {
                PropertyOne = "test",
                PropertyTwo = 1,
            };

            using (var store = new SerializationStorage<TestClasse>())
            {
                store.Add(test);
                store.Add(test);
                store.Add(test);
                store.Add(test);
            }

            var test2 = new TestClasse
            {
                PropertyOne = "test2",
                PropertyTwo = 2,
            };

            using (var store = new SerializationStorage<TestClasse>())
            {
                store.Clear();
                store.Add(test2);
            }

            using (var store = new SerializationStorage<TestClasse>())
            {
                Assert.IsTrue(store.Last().PropertyTwo == 2);
            }
        }
    }

    public class TestClasse
    {
        public string PropertyOne { get; set; }
        public int PropertyTwo { get; set; }
    }
}
