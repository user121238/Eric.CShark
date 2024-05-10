using Eric.CShark.Utility;

namespace Eric.CShark.Tests
{

    [TestClass]
    public class PropertiesHelperTest
    {
        /// <summary>
        /// 测试用的类
        /// </summary>
        private class TestClass
        {
            public int IntProperty { get; set; } = 1;
            public string StringProperty { get; set; } = "initial";
            public DateTime DateProperty { get; set; } = DateTime.Now;
            // 假设这个属性我们不想重置
            public string ExcludeProperty { get; set; } = "don't reset";
        }



        /// <summary>
        /// 测试重置所有字段
        /// </summary>
        [TestMethod]
        public void ResetAllProperties()
        {
            var testObj = new TestClass();

            void ResetAction(TestClass obj) => PropertiesHelper.ResetObjectPropertiesToDefault(obj, c => false);

            ResetAction(testObj);

            Assert.AreEqual(default(int), testObj.IntProperty);

            Assert.AreEqual(default(string), testObj.StringProperty);

            Assert.AreEqual(default(DateTime), testObj.DateProperty);

            Assert.AreEqual(default(string), testObj.ExcludeProperty);

        }

        /// <summary>
        /// 测试排除某个属性不重置
        /// </summary>
        [TestMethod]
        public void ExcludePropertyFromReset()
        {
            var testObj = new TestClass();

            void ResetAction(TestClass obj) => PropertiesHelper.ResetObjectPropertiesToDefault(obj, c => c.Name == nameof(TestClass.ExcludeProperty));

            ResetAction(testObj);

            Assert.AreEqual(default(int), testObj.IntProperty);

            Assert.AreEqual(default(string), testObj.StringProperty);

            Assert.AreEqual(default(DateTime), testObj.DateProperty);

            Assert.AreEqual("don't reset", testObj.ExcludeProperty);
        }


        /// <summary>
        /// 测试将某个属性设置为特定值
        /// </summary>
        [TestMethod]
        public void CustomSettingsForProperties()
        {
            var testObj = new TestClass();

            void ResetAction(TestClass obj) => PropertiesHelper.ResetObjectPropertiesToDefault(obj, c => c.Name == nameof(TestClass.ExcludeProperty), c => c.IntProperty = 2);

            ResetAction(testObj);

            Assert.AreEqual(2, testObj.IntProperty);

            Assert.AreEqual(default(string), testObj.StringProperty);

            Assert.AreEqual(default(DateTime), testObj.DateProperty);

            Assert.AreEqual("don't reset", testObj.ExcludeProperty);
        }

        /// <summary>
        /// 深度克隆测试类
        /// </summary>
        [Serializable]
        private class DeepCloneTestClass : IEquatable<DeepCloneTestClass>
        {
            public int Value { get; set; }
            public string Name { get; set; }

            public List<DeepCloneTestClass> List { get; set; } = new();



            public override bool Equals(object? obj)
            {
                return obj is DeepCloneTestClass other && Value == other.Value && Name == other.Name && List.SequenceEqual(other.List);
            }

            public bool Equals(DeepCloneTestClass? other)
            {
                return other != null && Value == other.Value && Name == other.Name && List.SequenceEqual(other.List);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Value, Name, List);
            }
        }


        /// <summary>
        /// 二进制深度克隆测试
        /// </summary>
        [TestMethod]
        public void Test_DeepCloneWithBinarySerialize()
        {
            var testClass = new DeepCloneTestClass
            {
                Value = 0,
                Name = "test"
            };

            testClass.List.Add(new DeepCloneTestClass
            {
                Value = 2,
                Name = "test1"
            });

            var cloned = testClass.DeepCloneWithBinarySerialize();

            Assert.IsNotNull(cloned);

            Assert.IsFalse(object.ReferenceEquals(testClass, cloned));
            Assert.IsFalse(object.ReferenceEquals(testClass.List, cloned.List));

            Assert.AreEqual(testClass.Value, cloned.Value);

            // Assert.AreEqual(testClass.List, cloned.List);
            Assert.IsTrue(testClass.List.SequenceEqual(cloned.List));

            cloned.List.Add(new DeepCloneTestClass());

            Assert.AreNotEqual(testClass.List.Count, cloned.List.Count);

        }

        /// <summary>
        /// Json序列化深度克隆测试
        /// </summary>
        [TestMethod]
        public void Test_DeepCloneWithJsonSerialize()
        {
            var testClass = new DeepCloneTestClass
            {
                Value = 0,
                Name = "test"
            };

            testClass.List.Add(new DeepCloneTestClass
            {
                Value = 2,
                Name = "test1"
            });

            var cloned = testClass.DeepCloneWithJsonSerialize();

            Assert.IsNotNull(cloned);

            Assert.IsFalse(object.ReferenceEquals(testClass, cloned));
            Assert.IsFalse(object.ReferenceEquals(testClass.List, cloned.List));

            Assert.AreEqual(testClass.Value, cloned.Value);

            // Assert.AreEqual(testClass.List, cloned.List);
            Assert.IsTrue(testClass.List.SequenceEqual(cloned.List));

            cloned.List.Add(new DeepCloneTestClass());

            Assert.AreNotEqual(testClass.List.Count, cloned.List.Count);
        }


    }
}
