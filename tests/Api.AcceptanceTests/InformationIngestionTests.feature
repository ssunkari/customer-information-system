Feature: InformationIngestionTests
	In order to ingest the application information
	As a service provider
	I want to store the values


Scenario: sucessful stored customer information
	Given I have no customer records
	When I submit a customer record	
	Then I should get http ok response
