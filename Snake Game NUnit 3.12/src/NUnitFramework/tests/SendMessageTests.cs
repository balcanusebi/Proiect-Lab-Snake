// ***********************************************************************
// Copyright (c) 2018 Charlie Poole, Rob Prouse
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.TestData;
using NUnit.TestUtilities;

namespace NUnit.Framework
{
    public class SendMessageTests : ITestListener
    {
        private const string SOME_DESTINATION = "destination";
        private const string SOME_MESSAGE = "message";

        [Test]
        public void TestMessage_SendsToListener()
        {
            var test = TestBuilder.MakeTestFromMethod(typeof(SendMessageFixture), nameof(SendMessageFixture.TestWithMessage));
            var work = TestBuilder.CreateWorkItem(test, new SendMessageFixture());
            work.Context.Listener = this;
            var result = TestBuilder.ExecuteWorkItem(work);

            Assert.That(result.ResultState, Is.EqualTo(ResultState.Success));
            Assert.That(result.Output, Is.EqualTo(""));

            Assert.That(_testMessage.Destination, Is.EqualTo(SOME_DESTINATION));
            Assert.That(_testMessage.Message, Is.EqualTo(SOME_MESSAGE));
            Assert.That(_testMessage.TestId, Is.EqualTo(result.Test.Id));
        }

        #region ITestListener Implementation

        public void TestStarted(ITest test)
        {
        }

        public void TestFinished(ITestResult result)
        {
        }

        TestOutput _testOutput;

        public void TestOutput(TestOutput output)
        {
            _testOutput = output;
        }

        TestMessage _testMessage;

        public void SendMessage(TestMessage message)
        {
            _testMessage = message;
        }

        #endregion
    }
}
