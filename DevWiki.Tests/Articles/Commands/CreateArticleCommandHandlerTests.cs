using DevWiki.Application.Articles.Commands.CreateArticle;
using DevWiki.Application.Interfaces;
using DevWiki.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevWiki.Tests.Articles.Commands
{
    public class CreateArticleCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Add_Article_And_Return_Id()
        {
            // Arrange
            var mockContext = new Mock<IApplicationDbContext>();
            
            // Настройка мока для DbSet
            var articles = new List<Article>();
            var tags = new List<Tag>();
            var articleTags = new List<ArticleTag>();
            
            mockContext.Setup(c => c.Articles).ReturnsDbSet(articles);
            mockContext.Setup(c => c.Tags).ReturnsDbSet(tags);
            mockContext.Setup(c => c.ArticleTags).ReturnsDbSet(articleTags);
            
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var handler = new CreateArticleCommandHandler(mockContext.Object);
            var command = new CreateArticleCommand("Test Title", "Test Content", "csharp, tag2");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            mockContext.Verify(c => c.Articles.Add(It.IsAny<Article>()), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
