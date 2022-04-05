using Articles.Client.Pages;
using Bunit;
using System;
using Xunit;

namespace Articles.Client.Test
{
    public class UnitTest1 : TestContext
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var cut = RenderComponent<Counter>();
            var paraElm = cut.Find("p");

            // Act
            cut.Find("button").Click();

            // Wait for state before continuing test
            cut.WaitForState(
                () => cut.Find("p").TextContent == $"Current count: 1",
                TimeSpan.FromSeconds(3));

            var paraElmText = paraElm.TextContent;

            // Assert
            paraElmText.MarkupMatches("Current count: 1");
        }
    }
}