using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightPractice

// [Parallelizable(ParallelScope.Self)] - It's better to not use it if there is a _browser below
// (I'm using it to select Firefox for Playwright)
// For example there is a test GetStartedLink that can fail because of this
// Since this Task is opening in a parallel mode it can cause errors and fail some tests
// To use Parallelizable you should use browser's context for better isolation

[Parallelizable(ParallelScope.All)]
[TestFixture]
public class ExampleTest
{
    private static IPlaywright _playwright;
    private static IBrowser _browser;
    private string url = "https://playwright.dev";

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
    }

    [OneTimeTearDown]
    public async Task OneTimeTeardown()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [Test]
    public async Task HasTitle()
    {
        await using var context = await _browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync(url);

        Assert.That(await page.TitleAsync(), Does.Contain("Playwright"));
    }

    [Test]
    public async Task GetStartedLink()
    {
        await using var context = await _browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync(url);

        await page.GetByRole(AriaRole.Link, new() { Name = "Get started" }).ClickAsync();

        var heading = page.GetByRole(AriaRole.Heading, new() { Name = "Installation" });
        Assert.That(await heading.IsVisibleAsync(), Is.True);
    } 
}