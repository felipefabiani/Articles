@Login
Feature: Login
	User can log in the website

Scenario: 1 - User and password are required
	Given a logged out user
	When the user attempts to log in with invalid email and password credentials
	| Field        | Value                 |
	| Email        |                       |
	| Password     |                       |
	| EmailMessage | Username is required! |
	| PwdMessage   | Password is required! |
	Then the log is not logged in
	And the user messages for the inputs fields

Scenario: 2 - User cannot log in with invalid email and short password
	Given a logged out user
	When the user attempts to log in with invalid email and password credentials	
	| Field        | Value                                                |
	| Email        | t                                                    |
	| EmailMessage | Please include an '@' in the email address.          |
	| Password     | 01234                                                |
	| PwdMessage   | Password length must be between 6 and 10 characters. |
	Then the log is not logged in
	And the user messages for the inputs fields

Scenario: 3 - User cannot log in with invalid email and long password
	Given a logged out user
	When the user attempts to log in with invalid email and password credentials	
	| Field        | Value                                                |
	| Email        | t                                                    |
	| EmailMessage | Please include an '@' in the email address.          |
	| Password     | 01234567891                                          |
	| PwdMessage   | Password length must be between 6 and 10 characters. |
	Then the log is not logged in
	And the user messages for the inputs fields

Scenario: 4 - User cannot log in with invalid credentials
	Given a logged out user
	When the user attempts to log in with invalid credentials
	| Field       | Value                       |
	| Email       | author.test@article.ie      |
	| Password    | 012345                      |
	| Alert       | User or password incorrect! |
	Then the log is not logged in
	And the user see a error message

Scenario: 5 - Admin can log in with valid credentials
	Given a logged out user
	When the user attempts to log in with valid credentials
	| Field    | Value                  |
	| Email    | admin.test@article.ie  |
	| Password | 123456                 |
	Then the log in successfully

Scenario: 6 - Author can log in with valid credentials
	Given a logged out user
	When the user attempts to log in with valid credentials
	| Field    | Value                  |
	| Email    | author.test@article.ie |   
	| Password | 123456                 |
	Then the log in successfully

Scenario: 7 - User can log in with valid credentials
	Given a logged out user
	When the user attempts to log in with valid credentials
	| Field    | Value                  |
	| Email    | user.test@article.ie   |
	| Password | 123456                 |
	Then the log in successfully