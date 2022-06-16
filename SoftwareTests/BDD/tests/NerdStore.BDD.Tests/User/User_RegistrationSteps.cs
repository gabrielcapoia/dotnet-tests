using System;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    public class User_RegistrationSteps
    {   
        [When(@"He clicks register")]
        public void WhenHeClicksRegister()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"fill in the form data")]
        public void WhenFillInTheFormData(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"Click on the register button")]
        public void WhenClickOnTheRegisterButton()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"Fill the form data with a password without capitals")]
        public void WhenFillTheFormDataWithAPasswordWithoutCapitals(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"Fill the form data with a password with no special character")]
        public void WhenFillTheFormDataWithAPasswordWithNoSpecialCharacter(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"He will get an error message that the password must contain an uppercase letter")]
        public void ThenHeWillGetAnErrorMessageThatThePasswordMustContainAnUppercaseLetter()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"He will get an error message that the password must contain a special character")]
        public void ThenHeWillGetAnErrorMessageThatThePasswordMustContainASpecialCharacter()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
