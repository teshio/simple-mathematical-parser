using System;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace NiceTest.Spec
{
    [Binding]
    public class SimpleMathematicalParserSteps
    {
        MathematicalParser p;
        int result = 0;

        [Given(@"I have a mathematical parser")]
        public void GivenIHaveAMathematicalParser()
        {
            p = new MathematicalParser();
        }
        
        [When(@"I provide input '(.*)'")]
        public void WhenIProvideInput(string p0)
        {
            result = p.Parse(p0);
        }
        
        [Then(@"the result should be '(.*)'")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            Assert.AreEqual(p0, result);
        }
    }
}
