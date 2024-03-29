using EasyDesk.Testing.MatrixExpansion;
using EasyDesk.Tools;
using EasyDesk.Tools.Utils;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static EasyDesk.Tools.Collections.EnumerableUtils;

namespace EasyDesk.Testing.UnitTests;

public class MatrixTests
{
    private readonly IEqualityComparer<object[]> _comparer = EqualityComparers.From<object[]>(
        (a, b) => a.SequenceEqual(b),
        x => x.CombineHashCodes());

    [Fact]
    public void ShouldExpandSingleLevel()
    {
        var result = Matrix.Axis(1, 2, 3).Build();

        var expected = Items(
            new object[] { 1 },
            new object[] { 2 },
            new object[] { 3 });

        result.ShouldBe(expected, _comparer, ignoreOrder: true);
    }

    [Fact]
    public void ShouldExpandMultipleLevels()
    {
        var result = Matrix
            .Axis(0, 1)
            .Axis("a", "b")
            .Axis(true, false)
            .Build();

        var expected = Items(
            new object[] { 0, "a", true },
            new object[] { 0, "a", false },
            new object[] { 0, "b", true },
            new object[] { 0, "b", false },
            new object[] { 1, "a", true },
            new object[] { 1, "a", false },
            new object[] { 1, "b", true },
            new object[] { 1, "b", false });

        result.ShouldBe(expected, _comparer, ignoreOrder: true);
    }

    [Fact]
    public void ShouldFilterAtDeepLevels()
    {
        var result = Matrix
            .Axis(0, 1)
            .Axis('a', 'b')
            .Axis(true, false)
            .Filter(x => x.Item2 == 'a' + x.Item1)
            .Build();

        var expected = Items(
            new object[] { 0, 'a', true },
            new object[] { 0, 'a', false },
            new object[] { 1, 'b', true },
            new object[] { 1, 'b', false });

        result.ShouldBe(expected, _comparer, ignoreOrder: true);
    }
}
