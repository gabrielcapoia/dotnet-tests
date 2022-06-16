using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    public class CommomSteps
    {
        [Given(@"that the visitor is accessing the store's website")]
        public void GivenThatTheVisitorIsAccessingTheStoreSWebsite()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"He will be redirected to the showcase")]
        public void ThenHeWillBeRedirectedToTheShowcase()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"A greeting with your email will be displayed in the top menu")]
        public void ThenAGreetingWithYourEmailWillBeDisplayedInTheTopMenu()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
