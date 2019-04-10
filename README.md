# CustomerInformationSystem

### Business overview

Customer information is managed for proof of concept purposes.

### Technical overview

AspDotNetCore service.
Customer information such as email,firstname,surname and passwords are persisted and retreived from couchbase cocument database.

### Service Level Agreements (SLAs)

TBC

### Service owner

Owned by Xyz Team.

### Contributing applications, daemons, services, middleware

#### External Dependencies
* Couchbase Database (To upsert and retrieve customer records)

## System characteristics

### Hours of operation

24/7.
If downtime occurs, applications will restart itself.

### Infrastructure and network design

Is hosted using docker as container.

### Environmental differences

The auth keys are injected via environment variables, can be different for different environments.

### Tools

See [here](/TESTING-RULES.md) for how to run performance tests.

Postman pack is included in the root of the application for testing purposes.

## Required resources

Docker

Docker-Compose

POSTMAN

## System configuration

### Configuration management

Application configuration is stored in the appsettings.json.
Infrastructure configuration is in docker-compose.yml

### Secrets management

Secrets are passed in via secure environment variables at container run time.

## Monitoring and alerting

None configured since its POC.

### Log aggregation solution

* App Insights SDK for machine level detail and APM diagnostics
* Serilog 

### Log message format

structured JSON format.

### Events and error messages

Warnings logged for rules that fail.
Any log message level of error is a critical error to the system.

### Metrics

### Health checks

/api/status should return 200 if service is up.

### Alarms

## Operational tasks

### Deployment

Image is built on DockerHub cloud platform.
Deployment can be done on docker contianer hosting platforms such as ECS, Kubernetes, RancherOS.

### Recovery

If an instance becomes unhealthy (health check returns non-200) 
Check the logs for errors and investigate service related issues.

### Extension Methods

See [here](/Extension_Methods_Catalogue.md) for extension methods for quick writing of rules.


___
The template for this came from [SkeltonThatcher](https://github.com/SkeltonThatcher/run-book-template/blob/master/run-book-template.md)
