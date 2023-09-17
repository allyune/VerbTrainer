# VerbTrainer Language Learning App
![.NET](https://github.com/allyune/VerbTrainer/actions/workflows/dotnet.yml/badge.svg)

Web application for learning foreign languages by practicing verb conjugations.

## Features
 - User authentication with username and password
 - Password reset
 - Email sender service, suitable for sending all kinds of emails (password reset, registration confirmation, marketing, etc).
 -  Default verb deck associated with all new users (top 100 verbs).
 - Deck management: user is able to create and remove custom decks, add and remove verbs from the decks.
 - Learning verb conjugations.
 - Planned: support for multiple languages.
 - Planned: interactive verb trainer.
 - Planned: measuring and saving user progress with the verb/deck.
 - Planned: sending email notifications to user based on their progress.
 - Planned: mobile application
   
## Technologies and Concepts
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![JavaScript](https://img.shields.io/badge/javascript-%23323330.svg?style=for-the-badge&logo=javascript&logoColor=%23F7DF1E)
![Postgres](https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)
![RabbitMQ](https://img.shields.io/badge/Rabbitmq-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)

 - .NET 7.0: ASP.NET Core, Web API
 - Entity Framework, PostgreSQL
 - JWT authentication using short-lived Access tokens with Refresh tokens
 - Microservices architecture
 - Messaging using RabbitMQ
 - API Documentation using Swagger
 - Email templating using Razor
 - JavaScript, HTML/CSS
