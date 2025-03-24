using Microsoft.Playwright;

namespace LambdatestPlaywrightTests
{
    public class TestLocators
    {
        private readonly IPage _page;

        public TestLocators(IPage page)
        {
            this._page = page;
        }

        public ILocator SimpleFrmDemo => _page.Locator("xpath=//*[contains(text(),'Simple Form Demo')]");
        public ILocator SimplefrmIp => _page.Locator("[placeholder='Please enter your Message']");
        public ILocator GetCheckedValue => _page.Locator("#showInput");
        public ILocator SampleMsg => _page.Locator("#message");
        public ILocator DragAndDrop => _page.Locator("xpath=//*[contains(text(),'Drag & Drop Sliders')]");
        public ILocator Slider => _page.Locator("#slider3 > div > input");
        public ILocator SliderValue => _page.Locator("#rangeSuccess");
        public ILocator InputForm => _page.Locator("xpath=//*[contains(text(),'Input Form Submit')]");
        public ILocator SubmitForm => _page.Locator("xpath=//*[contains(text(),'Submit')]");
        public ILocator Name => _page.Locator("#name");
        public ILocator Email => _page.Locator("#inputEmail4");
        public ILocator Password => _page.Locator("[name='password']");
        public ILocator Company => _page.Locator("[name='company']");
        public ILocator Website => _page.Locator("[name='website']");
        public ILocator Country => _page.Locator("[name='country']");
        public ILocator City => _page.Locator("[name='city']");
        public ILocator Address1 => _page.Locator("#inputAddress1");
        public ILocator Address2 => _page.Locator("#inputAddress2");
        public ILocator State => _page.Locator("#inputState");
        public ILocator Zip => _page.Locator("[name='zip']");
        public ILocator SuccessMsg => _page.Locator("xpath=//*[contains(text(),'Thanks for contacting us, we will get back to you shortly.')]");
        public ILocator sliderValue => _page.Locator("#rangeSuccess");
        public async Task ClickAsync(ILocator locator)
        {
            await locator.ClickAsync();
        }

        public async Task EnterTextAsync(ILocator locator, string text)
        {
            await locator.FillAsync(text);
        }
    }
}
