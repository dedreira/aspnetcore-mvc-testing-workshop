using MvcMovie.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace MvcMovie.Tests.Presenters
{
    public class CreateMoviePage
    {
        IWebElement titleInput;
        IWebElement releaseDateInput;
        IWebElement genreInput;
        IWebElement priceInput;
        IWebElement createButton;

        public CreateMoviePage(IWebDriver browser)
        {
            titleInput = browser.FindElement(By.Id(nameof(Movie.Title)));
            releaseDateInput = browser.FindElement(By.Id(nameof(Movie.ReleaseDate)));
            genreInput = browser.FindElement(By.Id(nameof(Movie.Genre)));
            priceInput = browser.FindElement(By.Id(nameof(Movie.Price)));
            createButton = browser.FindElement(By.CssSelector("input[type=submit]"));
        }

        public string Title
        {
            get
            {
                return titleInput.Text;
            }
            set
            {
                titleInput.Clear();
                titleInput.SendKeys(value);
            }
        }
        public string ReleaseDate
        {
            get
            {
                return releaseDateInput.Text;
            }
            set
            {
                releaseDateInput.Clear();
                releaseDateInput.SendKeys(value);
            }
        }
        public string Genre 
        { 
            get
            {
                return genreInput.Text;
            }
            set
            {
                genreInput.Clear();
                genreInput.SendKeys(value);
            }
        }
        public string Price
        {
            get
            {
                return priceInput.Text;
            }
            set
            {
                priceInput.Clear();
                priceInput.SendKeys(value.ToString());
            }
        }

        public void SendRequest()
        {
            createButton.Click();
        }
    }
}
