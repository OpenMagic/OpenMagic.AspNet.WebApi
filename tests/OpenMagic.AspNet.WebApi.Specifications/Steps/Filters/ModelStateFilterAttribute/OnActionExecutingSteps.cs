using System.Net.Http;
using System.Web.Http.Controllers;
using FakeItEasy;
using FluentAssertions;
using OpenMagic.AspNet.WebApi.Specifications.Helpers;
using TechTalk.SpecFlow;

namespace OpenMagic.AspNet.WebApi.Specifications.Steps.Filters.ModelStateFilterAttribute
{
    [Binding]
    public class OnActionExecutingSteps
    {
        private readonly Actual _actual;
        private readonly Given _given;

        public OnActionExecutingSteps(Given given, Actual actual)
        {
            _given = given;
            _actual = actual;
        }

        [Given(@"model is valid")]
        public void GivenModelIsValid()
        {
            // nothing to do!
        }

        [Given(@"model has error '(.*)', '(.*)'")]
        public void GivenModelHasError(string p0, string p1)
        {
            _given.ModelErrors.Add(p0, p1);
        }

        [When(@"ModelStateFilterAttribute\.OnActionExecuting\(HttpActionContext\) is called")]
        public void WhenModelStateFilterAttribute_OnActionExecutingHttpActionContextIsCalled()
        {
            var filter = new WebApi.Filters.ModelStateFilterAttribute();
            var requestContext = new HttpRequestContext();
            var requestMessage = new HttpRequestMessage();
            var controllerDescriptor = new HttpControllerDescriptor();
            var controller = A.Fake<IHttpController>();
            var controllerContext = new HttpControllerContext(requestContext, requestMessage, controllerDescriptor, controller);
            var actionDescriptor = new ReflectedHttpActionDescriptor();
            var actionContext = new HttpActionContext(controllerContext, actionDescriptor);

            foreach (var modelError in _given.ModelErrors)
            {
                actionContext.ModelState.AddModelError(modelError.Key, modelError.Value);
            }

            filter.OnActionExecuting(actionContext);

            _actual.Response = actionContext.Response;
        }

        [Then(@"actionContext\.Response should be null")]
        public void ThenActionContext_ResponseShouldBeNull()
        {
            _actual.Response.Should().BeNull();
        }

        [Then(@"actionContext\.Response\.StatusCode should be '(.*)'")]
        public void ThenActionContext_Response_StatusCodeShouldBe(string p0)
        {
            _actual.Response.StatusCode.ToString().Should().Be(p0);
        }

        [Then(@"actionContext\.Response\.Content should include the model errors")]
        public void ThenActionContext_Response_ContentShouldIncludeTheModelErrors()
        {
            var actualModelState = _actual.Response.Content.GetModelErrors();

            actualModelState.ShouldAllBeEquivalentTo(_given.ModelErrors);
        }
    }
}