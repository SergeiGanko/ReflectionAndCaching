using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace ReflectionTask.Tests
{
    public class TestData
    {
        public static IEnumerable<TestCaseData> EqualsIEnumerablesTestCases
        {
            get
            {
                yield return new TestCaseData(
                        new[] { 1, 2, 3 },
                        new[] { 1, 2, 3 })
                    .Returns(true);

                yield return new TestCaseData(
                        new[] { 1, 2, 3 },
                        new[] { 1, 2, 6 })
                    .Returns(false);

                yield return new TestCaseData(
                        new[] { new SimpleClass { Id = 1, Name = "name" }, new SimpleClass { Id = 2, Name = "name2" } },
                        new[] { new SimpleClass { Id = 1, Name = "name" } })
                    .Returns(false);

                yield return new TestCaseData(
                        new[] { new SimpleClass { Id = 1, Name = "name" }},
                        new[] { new SimpleClass { Id = 1, Name = "name2" }})
                    .Returns(false);

                yield return new TestCaseData(
                        new[] { new SimpleClass { Id = 1, Name = "name" } },
                        new[] { new SimpleClass { Id = 1, Name = "name" } })
                    .Returns(true);

                yield return new TestCaseData(
                        new[] { 1, 2, 3 },
                        new[] { 1, 2 })
                    .Returns(false);

                yield return new TestCaseData(
                        new[] { 1, 2 },
                        new[] { 1, 2, 3 })
                    .Returns(false);

                yield return new TestCaseData(
                    new ArrayList { 1, 2, 3 },
                    new ArrayList { 1, 2, 3 })
                    .Returns(true);

                yield return new TestCaseData(
                    new ArrayList { 1, 2, 3 },
                    new ArrayList { 1, 2, 6 })
                    .Returns(false);

                yield return new TestCaseData(
                    new Queue(new [] { 1, 2, 3 }),
                    new Queue(new [] { 1, 2, 3 }))
                    .Returns(true);

                yield return new TestCaseData(
                    new Queue(new[] { 1, 2, 3 }),
                    new Queue(new[] { 1, 2, 6 }))
                    .Returns(false);

                yield return new TestCaseData(
                    new Stack(new[] { 1, 2, 3 }),
                    new Stack(new[] { 1, 2, 3 }))
                    .Returns(true);

                yield return new TestCaseData(
                    new Stack(new[] { 1, 2, 3 }),
                    new Stack(new[] { 1, 2, 6 }))
                    .Returns(false);
            }
        }

        public static IEnumerable<TestCaseData> EqualsCustomClassesTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new SimpleClass { Id = 1, Name = "str1", SomeEnum = SomeEnum.One },
                    new SimpleClass { Id = 1, Name = "str1", SomeEnum = SomeEnum.Two })
                    .Returns(false);

                yield return new TestCaseData(
                        new SimpleClass { Id = 1, Name = "str1", SomeEnum = SomeEnum.One },
                        new SimpleClass { Id = 1, Name = "str1", SomeEnum = SomeEnum.One })
                    .Returns(true);

                yield return new TestCaseData(
                        new ComplexClass()
                        {
                            Id = 1,
                            Name = "str1",
                            Value1 = 1.0001,
                            Value2 = 1.1f,
                            Arr = new[] { 1, 2, 3 },
                            List = new List<int> { 1, 2, 3 },
                            Simple = new SimpleClass { Id = 2, Name = "name", SomeEnum = SomeEnum.Two }
                        },
                        new ComplexClass()
                        {
                            Id = 1,
                            Name = "str1",
                            Value1 = 1.0001,
                            Value2 = 1.1f,
                            Arr = new[] { 1, 2, 3 },
                            List = new List<int> { 1, 2, 5 },
                            Simple = new SimpleClass { Id = 2, Name = "name", SomeEnum = SomeEnum.Two }
                        })
                    .Returns(false);

                yield return new TestCaseData(
                        new ComplexClass()
                        {
                            Id = 1,
                            Name = "str1",
                            Value1 = 1.0001,
                            Value2 = 1.1f,
                            Arr = new[] { 1, 2, 3 },
                            List = new List<int> { 1, 2, 3 },
                            Simple = new SimpleClass { Id = 2, Name = "name", SomeEnum = SomeEnum.Two }
                        },
                        new ComplexClass()
                        {
                            Id = 1,
                            Name = "str1",
                            Value1 = 1.0001,
                            Value2 = 1.1f,
                            Arr = new[] { 1, 2, 3 },
                            List = new List<int> { 1, 2, 3 },
                            Simple = new SimpleClass { Id = 2, Name = "name", SomeEnum = SomeEnum.Two }
                        })
                    .Returns(true);
            }
        }
    }

    public class SimpleClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SomeEnum SomeEnum { get; set; } = SomeEnum.One;
    }

    public enum SomeEnum
    {
        One,
        Two
    }

    public struct SomeStruct
    {
        public int Value { get; set; }
        public string Str { get; set; }
    }

    public class ComplexClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value1 { get; set; }
        public float Value2 { get; set; }
        public SimpleClass Simple { get; set; }
        public SomeStruct SomeStruct { get; set; }
        public int[] Arr { get; set; }
        public List<int> List { get; set; }
    }
}
