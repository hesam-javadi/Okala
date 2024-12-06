# Okala Recruitment Project

## Summary
This project was developed as part of the recruitment process for Okala. It is a .NET 8 Web API application designed to display cryptocurrency exchange rates for specific currencies using external APIs.

The project is built with clean architecture principles, emphasizing scalability, maintainability, and testability.

## Get Started

To run the project locally, follow these steps:
1. **(Optional but Recommended) Start Redis:** Run Redis on your local computer using Docker with the following command:
   
   ```bash
   docker run --name redis -d -p 6379:6379 redis
   ```
2. **(Optional) Install and Run Seq:** Install Seq for structured logging and run it on your local host (default port: 5341). You can download Seq from https://datalust.co/seq.
3. Run the Application: Open the project in your IDE (e.g., Visual Studio or Rider) and hit the Run button to start the application. Once running, you can interact with the API to see the results.

## UML Diagram

Below is the UML diagram illustrating the structure and flow of the application:

![UmlDiagram](img/UmlDiagram.png)
