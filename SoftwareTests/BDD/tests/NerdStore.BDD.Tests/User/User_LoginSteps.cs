using System;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    public class User_LoginSteps
    {
        [When(@"he clicks login")]
        public void WhenHeClicksLogin()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"fill in the login form data")]
        public void WhenFillInTheLoginFormData(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"click the login button")]
        public void WhenClickTheLoginButton()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
