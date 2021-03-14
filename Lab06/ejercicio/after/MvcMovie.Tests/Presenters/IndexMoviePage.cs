using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace MvcMovie.Tests.Presenters
{
    public class IndexMoviePage
    {
        SelectElement genreSelect;
        IWebElement filterTitle;
        IWebElement filterButton;
        IWebElement createNewLink;
        List<IndexMoviePageTableRow> moviesRendered;
        IWebDriver browser;
        public IndexMoviePage(IWebDriver browser)
        {
            filterTitle = browser.FindElement(By.CssSelector("input[type=text]"));
            filterButton = browser.FindElement(By.CssSelector("input[type=submit]"));
            genreSelect = new SelectElement(browser.FindElement(By.TagName("select")));
            createNewLink = browser.FindElement(By.LinkText("Create New"));
            moviesRendered = browser.FindElements(By.CssSelector("tbody > tr"))
                            .Select(r => new IndexMoviePageTableRow(r))
                            .ToList();
            this.browser = browser;
        }

        public List<string> Genres()
        {
            return genreSelect.Options.Select(o => o.Text).ToList();
        }
        public void SelectGenre(string genre)
        {
            genreSelect.SelectByText(genre);
        }
        public string FilterTitle
        {
            get
            {
                return filterTitle.Text;
            }
            set
            {
                filterTitle.Clear();
                filterTitle.SendKeys(value);
            }
        }

        public void SendSearchRequest()
        {
            filterButton.Click();            
        }

        public void SendCreateNewRequest()
        {
            createNewLink.Click();            
        }

        public List<IndexMoviePageTableRow> MoviesRendered
        {
            get
            {
                return moviesRendered;
            }
        }

    }

    



}
