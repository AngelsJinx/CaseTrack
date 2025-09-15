# Backend
This is a pretty standard ASP.NET project solution.

I have broken it into 3 projects:

* CaseTrack - The REST API project, and business logic.
* CaseTrack.Data - Database persistence using Entity Framework.
* CaseTrack.Tests - Integration tests.


# CaseTrack
This is the main project, containing the business logic and REST endpoint controllers.
There is good argument that the business logic should be in a project of it's own, but that felt like overkill for this tech test.

The Controllers folder has our main TaskController, used for all the tech test functionality. \
I've also included a BaseController parent class, demonstrating how helper methods could be shared. In larger projects this would likely also include helper methods relating to authentication etc.

The Modules folder houses the core business logic - taking inputs from the Controllers and writing them to our Repositories from the CaseTrack.Data project.
In the interest of reducing indirection, I've rolled the validation into the main methods here. In a larger project I would normally use separate validation methods, called by the controller before calling the main action methods, but I was trying to reduce the number of files a reviewer needs to jump between.

The DTOs folder houses the objects that get passed to-and-from the front-end project.

# CaseTrack.Data
The persistence layer.

This uses Entity Framework to manage our tasks in the database - in this case Postgres. \
An example of the postgres setup is in the compose.yaml docker-compose file in the parent directory.

The idea here is that the implementation for IRepository can easily be swapped out if we want to change the persistence (MSSql, or even non-relational databases). Similarly, we could mock the data layer as part of unit or integration tests.

Unfortunately I ran out of time to demonstrate the test part of this.

# CaseTrack.Tests
Since I was limited on time, I elected to write just Integration tests.

These tests use TestContainers to dynamically spin up docker images of Postgres. \
I then use the excellent WebApplicationFactory to spin up the full ASP.NET REST endpoints, connected to those docker images.

I like integration tests, because we're using real HTTP clients to make normal requests, just like the real world, it lets us catch things like serialization issues that are easily missed by unit tests of functionality. \
Given the limited time I think these are more useful than unit tests that mock out the data layer, and don't test the asp.net components.

# API Documentation
http://localhost:9876/swagger/index.html