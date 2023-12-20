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
<INSERT DIAGRAM AND SMALL NOTE>

Here comes a description of our domain model.

![Illustration of the _Chirp!_ data model as UML class diagram.](docs/images/domain_model.png)

## Architecture — In the small
INSERT DIAGRAM AND SMALL NOTE

## Architecture of deployed application
WHY WE CHANGED BACK TO SQLITE WRITE HERE

<<<<<<< HEAD
INSER DIAGRAM AND SMALL NOTE
=======
**Azure web services**
The Chirp! client is hosted on azure, under Azure Web Services, which is connected to the server via the connection string in appsettings.

**Database**
The deployed application supports using both SQLite and Azure SQL database(application in local/development environment also supports SQL Server using Docker), based on the configurations in appsettings.< Development | Production >.json. In the deployed application, the appsettings configuration for SQLite is chosen in the final deployed application, due to insufficient student credits, at the end of the project.

INSER DIAGRAM AND SMALL NOTE
>>>>>>> aae9e2df911c43c1c2e3822310dff6b8ecf3cc37

## User activities
INSERT DIAGRAM AND SMALL NOTE


## Sequence of functionality/calls trough _Chirp!_
INSERT DIAGRAM AND SMALL NOTE

# Process

## Build, test, release, and deployment
INSERT DIAGRAM AND SMALL NOTE

## Team work
The communication channel for the team has primarily been Message with the occasional Discord calls if we weren't able to meet up but thought a meeting was preferred over chatting on Messenger

Very early on in the projects lifespan we agreed to meet at least one time a week on Tuesdays to talk and work on the project. This gave us the ability to talk about whatever we had on our minds, get more comfortable with each other and distribute responsibilities for the assignments at hand.

**Show a screenshot of your project board right before hand-in. Briefly describe which tasks are still unresolved, i.e., which features are missing from your applications or which functionality is incomplete**

When an issue is created a ticket is generated and put into the "New" section of the project board with the tag "Triage" assigned to it. From here it will be moved into either "Backlog" for tickets that cannot be done immediately and "ready" for tickets that are ready to be taken on by a group member. When working on a ticket one should be assigned to it and move it to the "In Progress" section. Once a pull request has been created the ticket should be "In Review" and then moved to "Done" once the issue is resolved. 

## How to make _Chirp!_ work locally
TEXT TO EXPLAIN HOW AND WHY

## How to run test suite locally
TEXT TO EXPLAIN HOW AND WHY

# Ethics

## License
WHICH LICENSE WE CHOSE

## LLMs, ChatGPT, CoPilot, and others
Throughout the project LLM usage has been kept to a minimum, only used to ask quick questions about syntax for either C# code og advice for how to set up 3rd party tools such as Azure and Docker.

Towards the end, however, when finishing the test suite, a lot of different libraries were used and ChatGPT was a great help to understand the basics and provide tips for how to fetch certain necessary components using different methods for separate elements. Here, the use of ChatGPT was used in a way to gain quick and easy access to information, instead of having to learn whole libraries one could ask for very specific guidance.
