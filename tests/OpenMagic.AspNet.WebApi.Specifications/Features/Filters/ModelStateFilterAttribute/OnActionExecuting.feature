Feature: OnActionExecuting

Scenario: Model is valid
	Given model is valid
	When ModelStateFilterAttribute.OnActionExecuting(HttpActionContext) is called
	Then actionContext.Response should be null

Scenario: Model state is invalid
	Given model has error 'DummyKey', 'DummyErrorMessage'
	When ModelStateFilterAttribute.OnActionExecuting(HttpActionContext) is called
	Then actionContext.Response.StatusCode should be 'BadRequest'
    And actionContext.Response.Content should include the model errors
