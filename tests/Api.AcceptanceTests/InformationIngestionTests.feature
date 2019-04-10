Feature: InformationIngestionTests
	In order to ingest the application information
	As a service provider
	I want to store the values

Scenario: successfully stored customer information
	Given I have no customer records
	When I submit a customer record	
	Then I should get http Ok response

Scenario: couchbase server is unavailable
	Given I database server is unavailable
	When I submit a customer record	
	Then I should get http InternalServerError response

Scenario: check if user already exists
	Given I have existing customer record 
	When I submit a customer record	
	Then I should get http BadRequest response

Scenario: successfully update customer information
	Given I have existing customer record 
	When I update a customer record	
	Then I should get http Ok response

Scenario: update customer information failure
	Given I have no customer records
	When I update a customer record	
	Then I should get http BadRequest response

Scenario: get customer information with empty database
	Given I have no customer records
	When I request a customer record	
	Then I should get http BadRequest response

Scenario: get customer information success
	Given I have existing customer record
	When I request a customer record	
	Then I should get http Ok response
