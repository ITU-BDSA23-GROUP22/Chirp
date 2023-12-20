---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group 22
authors:
- "Patrick Herlin Henriksen <pahe@itu.dk>"
- "Emil Parkel <park@itu.dk>"
- "Rasmus Emil Sylvest Hendil <rehe@itu.dk>"
- "Marcus Kofoed Kirkegaard <mkki@itu.dk>"
numbersections: true
---

# Design and Architecture of _Chirp!_



## Domain model

<img src="Diagrams/DomainModel.drawio.svg" alt="UML Diagram of the domain model" backgroundcolor="white" style="height:700px;"/>



## Architecture — In the small

<img src="Diagrams/SmallArchitecture.drawio.svg" alt="UML Diagram of the domain model" style="height:500px;"/>

> On the above diagram we display the onion-like architecture that is employed within the application. Classes located in any circle are dependant on the classes in the circles closer to the middle. The outer layers are closer to what the user interacts with while the inner circles are the core functionalities of _Chirp!_.



## Architecture of deployed application
WHY WE CHANGED BACK TO SQLITE WRITE HERE

<img src="Diagrams/DeployedArchithecture.drawio.svg" alt="UML Diagram of the domain model" style="height:500px;"/>

**Azure web services**
The Chirp! client is hosted on azure, under Azure Web Services, which is connected to the server via the connection string in appsettings.

**Database**
The deployed application supports using both SQLite and Azure SQL database(application in local/development environment also supports SQL Server using Docker), based on the configurations in appsettings.< Development | Production >.json. In the deployed application, the appsettings configuration for SQLite is chosen in the final deployed application, due to insufficient student credits, at the end of the project.



## User activities

<img src="Diagrams/UserJourney.drawio.svg" alt="UML Diagram of the domain model" style="height:500px;"/>

> The above diagram shows a user journey scenario (a typical way to traverse the _Chirp!_ application). We show the options of pages and actions aswell as conditionals. 



## Sequence of functionality/calls trough _Chirp!_

<img src="Diagrams/SequenceOfFunctionality.drawio.svg" alt="UML Diagram of the domain model" style="height:500px;"/>

> The sequence of functionalit through _Chirp!_ . We have illustrated this with an unauthorized using the application to view the public timeline.




# Process

## Build, test, release, and deployment
INSERT DIAGRAM AND SMALL NOTE

<img src="Diagrams/BuildTestReleaseDeploy.drawio.svg" alt="UML Diagram of the domain model" style="height:500px;"/>
In order to automate the majority of the development process, the deployment and release workflows are automated using GitHub Actions Workflow. 

The Release workflow first builds and tests the application, to minimise the risk of errors and bugs in the application to be released. If the build is succeeded and the tests are passed, the workflow will create a new release on github with a dependent, ready-to-run-application with the given semantic version tag “vx.x.x”. The release workflow is triggered by pushing a new tag to the main branch.

The Deployment workflow also builds and tests the application as the first step. If the first step is succeeded, the workflow will ¿upload artifacts? which are then deployed to azure(tjek lige workflowet igennem).
The deployment workflow is triggered on any push or merge to main, which means that.... <TBC>




## Team work

When an issue is created a ticket is generated and put into the "New" section of the project board with the tag "Triage" assigned to it. From here it will be moved into either "Backlog" for tickets that cannot be done immediately and "ready" for tickets that are ready to be taken on by a group member. When working on a ticket one should be assigned to it and move it to the "In Progress" section. Once a pull request has been created the ticket should be "In Review" and then moved to "Done" once the issue is resolved. 

<img src="Diagrams/ProjectBoardDiagram.drawio.svg" alt="UML diagram of how tickets travel through the projectboard"/>
> illustration of a ticket moving through the projectboard. From a new issue to pull request.

## How to make _Chirp!_ work locally
TEXT TO EXPLAIN HOW AND WHY

In As Chirp! both supports using SQLite and SQL server on Docker, in the code it is defaulted to connect to the SQL server docker container. The following steps are to run the default configurations.
Open Terminal
```Docker run```
```cd src/Chirp.Web```
```dotnet run````

If the developer wants to change the configuration, the configuration in appsettings…


## How to run test suite locally
TEXT TO EXPLAIN HOW AND WHY

# Ethics

## License
WHICH LICENSE WE CHOSE

## LLMs, ChatGPT, CoPilot, and others
Throughout the project LLM usage has been kept to a minimum, only used to ask quick questions about syntax for either C# code og advice for how to set up 3rd party tools such as Azure and Docker.

Towards the end, however, when finishing the test suite, a lot of different libraries were used and ChatGPT was a great help to understand the basics and provide tips for how to fetch certain necessary components using different methods for separate elements. Here, the use of ChatGPT was used in a way to gain quick and easy access to information, instead of having to learn whole libraries one could ask for very specific guidance.
