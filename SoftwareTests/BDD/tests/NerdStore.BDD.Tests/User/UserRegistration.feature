Feature: User - Registration
As a store visitor
I want to register as a user
So that I can make purchases in the store

Scenario: User registration successful
Given that the visitor is accessing the store's website
When He clicks register
And fill in the form data
		| Data 					|
		| Email					|
		| Password 				|
		| Password Confirmation |
And Click on the register button
Then He will be redirected to the showcase
And A greeting with your email will be displayed in the top menu

Scenario: Registration with a password without capitals
Given that the visitor is accessing the store's website
When He clicks register
And Fill the form data with a password without capitals
		| Data 					|
		| Email					|
		| Password 				|
		| Password Confirmation |
And Click on the register button
Then He will get an error message that the password must contain an uppercase letter

Scenario: Registration with password without special character
Given that the visitor is accessing the store's website
When He clicks register
And Fill the form data with a password with no special character
		| Data 					|
		| Email					|
		| Password 				|
		| Password Confirmation |
And Click on the register button
Then He will get an error message that the password must contain a special character
