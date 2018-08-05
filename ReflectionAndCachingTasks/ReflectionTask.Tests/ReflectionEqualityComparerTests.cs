using NUnit.Framework;

namespace ReflectionTask.Tests
{
    [TestFixture]
    public class ReflectionEqualityComparerTests
    {
        [Test]
        public void Equals_PassesRefToTheSameObjectAsFirstAndSecondParameter_ExpectsTrue()
        {
            var first = new SimpleClass();

            var reflectionEqualityComparer = new ReflectionEqualityComparer<SimpleClass>();

            Assert.True(reflectionEqualityComparer.Equals(first, first));
        }

        [Test]
        public void Equals_PassesNullAsOneParameter_ExpectsFalse()
        {
            var first = new SimpleClass();

            var reflectionEqualityComparer = new ReflectionEqualityComparer<SimpleClass>();

            Assert.False(reflectionEqualityComparer.Equals(null, first));
            Assert.False(reflectionEqualityComparer.Equals(first, null));
        }

        [TestCase(1, 1, ExpectedResult = true)]
        [TestCase(4, 2, ExpectedResult = false)]
        [TestCase(int.MaxValue, int.MaxValue, ExpectedResult = true)]
        [TestCase(int.MinValue, int.MinValue, ExpectedResult = true)]
        [TestCase(int.MaxValue, int.MinValue, ExpectedResult = false)]
        [TestCase(0, 0, ExpectedResult = true)]
        public bool Equals_PassesIntsAsParameters_ExpectsSuccess(int first, int second)
        {
            var reflectionEqualityComparer = new ReflectionEqualityComparer<int>();

            return reflectionEqualityComparer.Equals(first, second);
        }

        [TestCase(1.12, 0.001, ExpectedResult = false)]
        [TestCase(4.000, 4, ExpectedResult = true)]
        [TestCase(double.MaxValue, double.MaxValue, ExpectedResult = true)]
        [TestCase(double.MinValue, double.MinValue, ExpectedResult = true)]
        [TestCase(double.MaxValue, double.MinValue, ExpectedResult = false)]
        [TestCase(0, 0.0000, ExpectedResult = true)]
        public bool Equals_PassesDoublesAsParameters_ExpectsSuccess(double first, double second)
        {
            var reflectionEqualityComparer = new ReflectionEqualityComparer<double>();

            return reflectionEqualityComparer.Equals(first, second);
        }

        [TestCase("str1", "str2", ExpectedResult = false)]
        [TestCase("str1", "str1", ExpectedResult = true)]
        public bool Equals_PassesStringsAsParameters_ExpectsSuccess(string first, string second)
        {
            var reflectionEqualityComparer = new ReflectionEqualityComparer<string>();

            return reflectionEqualityComparer.Equals(first, second);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.EqualsIEnumerablesTestCases))]
        public bool Equals_PassesIEnumerablesAsParameters_ExpectsSuccess<T>(T obj1, T obj2)
        {
            var reflectionEqualityComparer = new ReflectionEqualityComparer<T>();

            return reflectionEqualityComparer.Equals(obj1, obj2);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.EqualsCustomClassesTestCases))]
        public bool Equals_PassesCustomClassesAsParameters_ExpectsSuccess<T>(T obj1, T obj2)
        {
            var reflectionEqualityComparer = new ReflectionEqualityComparer<T>();

            return reflectionEqualityComparer.Equals(obj1, obj2);
        }
    }
}
