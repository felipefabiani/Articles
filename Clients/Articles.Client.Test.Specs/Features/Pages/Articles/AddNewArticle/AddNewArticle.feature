@SaveArticle
Feature: AddNewArticle
	User can log in the website

Scenario: 1 - Add New Article invalid input
	Given A logged Author
	When author attempts to add new article with invalid title and content
	| Field          | Value                |
	| Title          |                      |
	| Content        |                      |
	| TitleMessage   | Title is required!   |
	| ContentMessage | Content is required! |
	Then article is not created
	And get messages for the inputs fields

Scenario: 2 - Add New Article invalid input
	Given A logged Author
	When author attempts to add new article with invalid title and content
	| Field          | Value                 |
	| Title          | too short             |
	| Content        | too short             |
	| TitleMessage   | Title is too short!   |
	| ContentMessage | Content is too short! |
	Then article is not created
	And get messages for the inputs fields


Scenario: 3 - Add New Article valid input
	Given A logged Author
	When author attempts to add new article with valid title and content
	| Field   | Value         |
	| Title   | not too short |
	| Content | not too short |
	Then article is created
	And get messages for the inputs fields
