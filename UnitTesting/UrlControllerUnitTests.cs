


using Microsoft.AspNetCore.Http;

namespace UnitTesting
{
    [TestClass]
    public class UrlControllerUnitTests
    {
        Mock<IUnitOfWork> unitOfWork;
        UrlController UrlController;
        ShortUrlVM shortUrlVM;
        ShortenedUrl surl;

        public UrlControllerUnitTests()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            shortUrlVM = new ShortUrlVM()
            {
                UrlRequest = new() {Url= "https://uaserial.club/pokemon/season-1" },
                ShortUrls = new()
                {
                    new ShortenedUrl(){Code="123", UserId="1", LongUrl="https://code-maze.com/csharp-mock-asynchronous-methods-using-moq/", ShortUrl="aaa" },
                    new ShortenedUrl(){Code="1233", UserId="2", LongUrl="https://stackoverflow.com/questions/68853409/an-expression-tree-may-not-contain-a-call-or-invocation-that-uses-optional-argum", ShortUrl="aaa" }
                }
            };
            UrlController = new UrlController(unitOfWork.Object);
        }

        [TestMethod]
        public void Index()
        {
            unitOfWork.Setup(u => u.ShortenedUrlRepository.GetAllAsync(It.IsAny<Expression<Func<ShortenedUrl, bool>>?>(),It.IsAny<string>())).ReturnsAsync(shortUrlVM.ShortUrls);

            var result= UrlController.Index().Result as ViewResult;
            var model=result.Model as ShortUrlVM;
            var list = model.ShortUrls;

            CollectionAssert.AreEqual(list, shortUrlVM.ShortUrls);
        }
        [TestMethod]
        public void Info()
        {
            string code = "123";
            unitOfWork.Setup(u => u.ShortenedUrlRepository.GetAsync(It.IsAny<Expression<Func<ShortenedUrl, bool>>?>(), It.IsAny<string>())).
                ReturnsAsync(shortUrlVM.ShortUrls.FirstOrDefault(u=>u.Code==code));
            var expected = new ShortenedUrl() { Code = "123", UserId = "1", LongUrl = "https://code-maze.com/csharp-mock-asynchronous-methods-using-moq/", ShortUrl = "aaa" };

           
            var result = UrlController.Info("code").Result as ViewResult;
            var model = result.Model as ShortenedUrl;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.Code, expected.Code);
        }
        //other unit tests are not possible, bc they require HttpContext
    }
}