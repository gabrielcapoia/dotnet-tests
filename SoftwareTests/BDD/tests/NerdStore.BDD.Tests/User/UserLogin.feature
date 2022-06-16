Feature: User - Login
as a user
I want to login
So that I can access the other features

Scenario: Login successfully
Given that the visitor is accessing the store's website
When he clicks login
And fill in the login form data
		| Data 			|
		| Email 		|
		| Password		|
And click the login button
Then He will be redirected to the showcase
And A greeting with your email will be displayed in the top menu