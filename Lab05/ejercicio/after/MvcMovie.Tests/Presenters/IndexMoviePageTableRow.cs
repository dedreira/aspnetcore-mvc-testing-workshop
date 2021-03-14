using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace MvcMovie.Tests.Presenters
{
    public class IndexMoviePageTableRow
    {
        string title;
        string releaseDate;
        string genre;
        string price;
        IWebElement editLink;
        IWebElement detailsLink;
        IWebElement deleteLink;
        public IndexMoviePageTableRow(IWebElement row)
        {
            title = row.FindElement(By.CssSelector("td:nth-of-type(1)")).Text;
            releaseDate = row.FindElement(By.CssSelector("td:nth-of-type(2)")).Text;
            genre = row.FindElement(By.CssSelector("td:nth-of-type(3)")).Text;
            price = row.FindElement(By.CssSelector("td:nth-of-type(4)")).Text;
            editLink = row.FindElement(By.LinkText("Edit"));
            detailsLink = row.FindElement(By.LinkText("Details"));
            deleteLink = row.FindElement(By.LinkText("Delete"));
        }

        public string Title
        {
            get
            {
                return title;
            }
        }
        public string ReleaseDate
        {
            get
            {
                return releaseDate;
            }
        }
        public string Genre
        {
            get
            {
                return genre;
            }
        }
        public string Price
        {
            get
            {
                return price;
            }
        }

        public void SendEditRequest()
        {
            editLink.Click();
        }
        public void SendDeleteRequest()
        {
            deleteLink.Click();
        }
        public void SendDetailsRequest()
        {
            detailsLink.Click();
        }
    }
}
