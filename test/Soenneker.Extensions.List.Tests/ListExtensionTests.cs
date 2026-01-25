using Soenneker.Tests.Unit;
using System;
using System.Collections.Generic;
using Xunit;

namespace Soenneker.Extensions.List.Tests;

public class ListExtensionTests : UnitTest
{
    [Fact]
    public void Default()
    {

    }

    [Fact]
    public void Replace_Replaces_First_Match_Only()
    {
        var list = new List<int> { 1, 2, 2, 3 };

        list.Replace(x => x == 2, 9);

        Assert.Equal(new List<int> { 1, 9, 2, 3 }, list);
    }

    [Fact]
    public void Replace_Does_Nothing_When_No_Match()
    {
        var list = new List<int> { 1, 2, 3 };

        list.Replace(x => x == 4, 9);

        Assert.Equal(new List<int> { 1, 2, 3 }, list);
    }

    [Fact]
    public void Replace_Does_Nothing_When_List_Is_Null()
    {
        List<int>? list = null;

        list.Replace(null!, 9);
    }

    [Fact]
    public void Replace_Does_Nothing_When_List_Is_Empty()
    {
        var list = new List<int>();

        list.Replace(null!, 9);

        Assert.Empty(list);
    }

    [Fact]
    public void Replace_Throws_When_Match_Is_Null_And_List_Has_Items()
    {
        var list = new List<int> { 1 };

        Assert.Throws<ArgumentNullException>(() => list.Replace(null!, 9));
    }
}
