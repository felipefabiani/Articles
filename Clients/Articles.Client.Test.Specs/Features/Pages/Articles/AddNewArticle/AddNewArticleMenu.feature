@SaveArticleMenu

Feature: AddNewArticleMenu
	User can log in the website

Scenario: 1 - Click Add New Article valid user
	Given A logged Author
	When author attempts to navigate to Add Article
	Then open add article page

Scenario: 2 - Click Add New Article invalid user
	Given A logged User
	When user attempts to navigate to Add Article
	Then get a not authorized message