using AwesomeAssertions;
using Soenneker.Tests.Unit;
using System;
using System.Collections.Generic;

namespace Soenneker.Extensions.List.Tests;

public class ListExtensionTests : UnitTest
{
    [Test]
    public void Default()
    {

    }

    [Test]
    public void Replace_Replaces_First_Match_Only()
    {
        var list = new List<int> { 1, 2, 2, 3 };

        list.Replace(x => x == 2, 9);

        list.Should().BeEquivalentTo(new List<int> { 1, 9, 2, 3 }, options => options.WithStrictOrdering());
    }

    [Test]
    public void Replace_Does_Nothing_When_No_Match()
    {
        var list = new List<int> { 1, 2, 3 };

        list.Replace(x => x == 4, 9);

        list.Should().BeEquivalentTo(new List<int> { 1, 2, 3 }, options => options.WithStrictOrdering());
    }

    [Test]
    public void Replace_Does_Nothing_When_List_Is_Null()
    {
        List<int>? list = null;

        Action action = () => list.Replace(null!, 9);

        action.Should().NotThrow();
    }

    [Test]
    public void Replace_Does_Nothing_When_List_Is_Empty()
    {
        var list = new List<int>();

        Action action = () => list.Replace(null!, 9);

        action.Should().NotThrow();
        list.Should().BeEmpty();
    }

    [Test]
    public void Replace_Throws_When_Match_Is_Null_And_List_Has_Items()
    {
        var list = new List<int> { 1 };

        Action action = () => list.Replace(null!, 9);

        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Remove_Removes_First_Match_Only()
    {
        var list = new List<int> { 1, 2, 2, 3 };

        list.Remove(x => x == 2);

        list.Should().BeEquivalentTo(new List<int> { 1, 2, 3 }, options => options.WithStrictOrdering());
    }

    [Test]
    public void Remove_Does_Nothing_When_No_Match()
    {
        var list = new List<int> { 1, 2, 3 };

        list.Remove(x => x == 4);

        list.Should().BeEquivalentTo(new List<int> { 1, 2, 3 }, options => options.WithStrictOrdering());
    }

    [Test]
    public void Remove_Does_Nothing_When_List_Is_Null()
    {
        List<int>? list = null;

        Action action = () => list.Remove(null!);

        action.Should().NotThrow();
    }

    [Test]
    public void Remove_Does_Nothing_When_List_Is_Empty()
    {
        var list = new List<int>();

        Action action = () => list.Remove(null!);

        action.Should().NotThrow();
        list.Should().BeEmpty();
    }

    [Test]
    public void Remove_Throws_When_Match_Is_Null_And_List_Has_Items()
    {
        var list = new List<int> { 1 };

        Action action = () => list.Remove(null!);

        action.Should().Throw<ArgumentNullException>();
    }
}
