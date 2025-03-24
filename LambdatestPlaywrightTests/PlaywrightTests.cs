
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.Playwright;
using Newtonsoft.Json;
using NUnit.Framework.Interfaces;

namespace LambdatestPlaywrightTests
{
    [TestFixture]
    public class PlaywrightTests
    {
        private IPage _page;
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPlaywright _playwright;
        private TestLocators _testLocator;
        private readonly string browser = Environment.GetEnvironmentVariable("BROWSER") ?? "Chrome";
        private readonly string os = Environment.GetEnvironmentVariable("OS") ?? "win";
        public static String dirPath = "Reports";
        public static ExtentReports _extent;
        public String TC_Name;
        public ExtentTest _test;

        [OneTimeSetUp]
        protected void ExtentStart()
        {
            var path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            var actualPath = path.Substring(0, path.LastIndexOf("bin"));
            var projectPath = new Uri(actualPath).LocalPath;

            Directory.CreateDirectory(projectPath.ToString() + dirPath);
            var reportPath = projectPath + dirPath + "//SeleniumPlaygroundReport.html";

            /* For Version 3 */
            /* var htmlReporter = new ExtentV3HtmlReporter(reportPath); */
            /* For version 4 --> Creates Index.html */
            ExtentSparkReporter htmlReporter = new ExtentSparkReporter(reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
            _extent.AddSystemInfo("Host Name", "Selenium Playground Testing on HyperTest Grid");
            _extent.AddSystemInfo("Environment", os);
            _extent.AddSystemInfo("UserName", "Sumeet pal");
            //htmlReporter.LoadConfig(projectPath + "Configurations//report-config.xml");
        }
        [SetUp]
        public async Task Setup()
        {
            string user, accessKey;
            user = "sumeet_palhotmail";
            accessKey = "LT_XYpMDieuAk9lCgAJsdvCJGiJbe7pmg0MXnev95xmI5LQoqy";

            Dictionary<string, object> capabilities = new Dictionary<string, object>();
            Dictionary<string, string> ltOptions = new Dictionary<string, string>();


            ltOptions.Add("name", "Playwright Test");
            ltOptions.Add("build", "Playwright C-Sharp tests on Hyperexecute");
            ltOptions.Add("platform", os);
            ltOptions.Add("user", user);
            ltOptions.Add("accessKey", accessKey);

            capabilities.Add("browserName", browser);
            capabilities.Add("browserVersion", "latest");
            capabilities.Add("LT:Options", ltOptions);
            string capabilitiesJson = JsonConvert.SerializeObject(capabilities);
            string cdpUrl = "wss://cdp.lambdatest.com/playwright?capabilities=" + Uri.EscapeDataString(capabilitiesJson);
            _playwright = await Playwright.CreateAsync();
            Console.WriteLine("Browser : " + browser);
            Console.WriteLine("os : " + os);
            _browser = await _playwright.Chromium.ConnectAsync(cdpUrl);
              _context = await _browser.NewContextAsync(new());
            _page = await _context.NewPageAsync();
            _testLocator = new TestLocators(_page);
            _context = await _browser.NewContextAsync();
            await _context.Tracing.StartAsync(new TracingStartOptions
            {
                Title = "Test Trace",
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });

        }

        [Test]
        public async Task TestScenario1()
        {
            String context_name = TestContext.CurrentContext.Test.Name + " on " + browser + " " + os;
            TC_Name = context_name;
            _test = _extent.CreateTest(context_name);
            Console.WriteLine("Navigating to Selenium Playground");
            await _page.GotoAsync("https://www.lambdatest.com/selenium-playground/");
            Console.WriteLine("Open Simple Demo Form");
            await _testLocator.ClickAsync(_testLocator.SimpleFrmDemo);
            Console.WriteLine("Typed Welcome to Lambdatest");
            await _testLocator.EnterTextAsync(_testLocator.SimplefrmIp, "Welcome to LambdaTest");
            Console.WriteLine("Click show typed message");
            await _testLocator.ClickAsync(_testLocator.GetCheckedValue);
            Console.WriteLine("Assert the text");
            Assert.That(_page.Url, Does.Contain("simple-form-demo"));
            Assert.That(await _testLocator.SampleMsg.InnerTextAsync(), Is.EqualTo("Welcome to LambdaTest"));
            var actualMeesageText = await _testLocator.SampleMsg.InnerTextAsync();
            var expMessageText = "Welcome to LambdaTest";
            Assert.That(actualMeesageText.Equals(expMessageText));
            if (await _testLocator.SampleMsg.InnerTextAsync() == "Welcome to LambdaTest")
            {
                await SetTestStatus("passed", "Messaged matched", _page);
                Console.WriteLine("Test Passed");
            }
            else
            {
                await SetTestStatus("failed", "Messaged not matched", _page);
                Console.WriteLine("Test Failed");
            }

        }


        [Test]
        public async Task TestScenario2()
        {
            String context_name = TestContext.CurrentContext.Test.Name + " on " + browser + " " + os;
            TC_Name = context_name;
            _test = _extent.CreateTest(context_name);
            Console.WriteLine("Navigating to Selenium Playground");
            await _page.GotoAsync("https://www.lambdatest.com/selenium-playground/");
            Console.WriteLine("Navigating to Drag and Drop page");
            await _testLocator.ClickAsync(_testLocator.DragAndDrop);
            Console.WriteLine("Drag the slider to 95");
            await _page.EvaluateAsync(@"() => {
                document.body.style.zoom = '0.50'; 
            }");
            var dragger = _testLocator.Slider;
            var box = await dragger.BoundingBoxAsync();

            if (box != null)
            {
                await _page.Mouse.MoveAsync(box.X + box.Width / 2, box.Y + box.Height / 2);
                await _page.Mouse.DownAsync();
                await _page.Mouse.MoveAsync(box.X + box.Width / 2 + 107, box.Y + box.Height / 2);
                await _page.Mouse.UpAsync();
            }

            var textContent = await _testLocator.SliderValue.TextContentAsync();
            Console.WriteLine("Assert the drag value");
            Assert.That(textContent, Is.EqualTo("95"));
            if (textContent == "95")
            {
                await SetTestStatus("passed", "Slidder value matched", _page);
                Console.WriteLine("Test Passed");
            }
            else
            {
                await SetTestStatus("failed", "Slidder value not matched", _page);
                Console.WriteLine("Test Failed");
            }
        }

        [Test]
        public async Task TestScenario3()
        {
            String context_name = TestContext.CurrentContext.Test.Name + " on " + browser + " " + os;
            TC_Name = context_name;
            _test = _extent.CreateTest(context_name);
            Console.WriteLine("Navigating to Selenium Playground");
            await _page.GotoAsync("https://www.lambdatest.com/selenium-playground/");
            await _page.SetViewportSizeAsync(1920, 1080);
            Console.WriteLine("Navigating to Input Form");
            await _testLocator.ClickAsync(_testLocator.InputForm);
            Console.WriteLine("Click submit form");
            await _testLocator.ClickAsync(_testLocator.SubmitForm);
            var validationMessage = await _page.EvaluateAsync<string>("document.activeElement.validationMessage");
            Console.WriteLine("Assert validation");
            Assert.That(validationMessage, Is.EqualTo("Please fill out this field."));
            Console.WriteLine("Enter Name");
            await _testLocator.EnterTextAsync(_testLocator.Name, "Lambda");
            Console.WriteLine("Enter Email");
            await _testLocator.EnterTextAsync(_testLocator.Email, "lambda@email.com");
            Console.WriteLine("Enter password");
            await _testLocator.EnterTextAsync(_testLocator.Password, "PaSsWoRd");
            Console.WriteLine("Enter Company");
            await _testLocator.EnterTextAsync(_testLocator.Company, "LambdaTest");
            Console.WriteLine("Enter Website");
            await _testLocator.EnterTextAsync(_testLocator.Website, "https://www.lambdatest.com");
            Console.WriteLine("Select Country");
            await _testLocator.Country.SelectOptionAsync(new SelectOptionValue { Label = "United States" });
            Console.WriteLine("Enter City");
            await _testLocator.EnterTextAsync(_testLocator.City, "City");
            Console.WriteLine("Enter Address1");
            await _testLocator.EnterTextAsync(_testLocator.Address1, "Address1");
            Console.WriteLine("Enter Address2");
            await _testLocator.EnterTextAsync(_testLocator.Address2, "Address2");
            Console.WriteLine("Enter State");
            await _testLocator.EnterTextAsync(_testLocator.State, "State");
            Console.WriteLine("Enter Zip");
            await _testLocator.EnterTextAsync(_testLocator.Zip, "12345");
            Console.WriteLine("Submot Form");
            await _testLocator.ClickAsync(_testLocator.SubmitForm);
            Console.WriteLine("Assert Success Messages");
            Assert.Multiple(async () =>
            {
                Assert.That(await _testLocator.SuccessMsg.IsVisibleAsync(), Is.True);
                Assert.That(await _testLocator.SuccessMsg.InnerTextAsync(), Is.EqualTo("Thanks for contacting us, we will get back to you shortly."));
            });

            if (await _testLocator.SuccessMsg.InnerTextAsync() == "Thanks for contacting us, we will get back to you shortly.")
            {
                await SetTestStatus("passed", "Title matched", _page);
                Console.WriteLine("Test Passed");
            }
            else
            {
                await SetTestStatus("failed", "Title not matched", _page);
                Console.WriteLine("Test Failed");
            }
        }

        [TearDown]
        public async Task Close()
        {
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed;
            var exec_status = TestContext.CurrentContext.Result.Outcome.Status;
            switch (exec_status)
            {
                case TestStatus.Failed:
                    _test.Log(Status.Fail, "Fail");
                    break;
                case TestStatus.Passed:
                    _test.Log(Status.Pass, "Pass");
                    break;
                default:
                    break;
            }

            await _context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = "trace.zip"
            });

            await _browser.CloseAsync();
            _playwright.Dispose();
        }
        [OneTimeTearDown]
        protected void ExtentClose()
        {
            Console.WriteLine("OneTimeTearDown");
            _extent.Flush();
        }
        public static async Task SetTestStatus(string status, string remark, IPage page)
        {
            await page.EvaluateAsync("_ => {}", "lambdatest_action: {\"action\": \"setTestStatus\", \"arguments\": {\"status\":\"" + status + "\", \"remark\": \"" + remark + "\"}}");
        }
    }
}
