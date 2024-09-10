# Overview
EchoChat is a real-time chat application built with ASP.NET Core MVC. It leverages SQL Server for database management, Firebase, WebRTC, and offers features like user authentication, private messaging, and live audio and video calls. The project demonstrates the use of modern web technologies to create a chatting experience.

## Features
- Real-time chat: Communicate instantly in live chats.
- Audio and video calls
- Private messaging: Send direct messages to other users.
- User authentication: Secure user registration and login system.
- Firestore Integration: Efficient storage of messages and user data.
- Firebase Storage: Efficient storage of media and user files.

## Technologies Used
- ASP.NET Core MVC: Backend framework for creating dynamic web applications.
- SignalR: Real-time communication between clients and the server.
- SQL Server: Relational database for managing chat data.
- Entity Framework Core: ORM for database operations.
- Bootstrap: Front-end framework for responsive design.
- Firestore Database: for NoSQL data storage

## Prerequisites
- .NET SDK
- SQL Server
- A modern browser

## Getting Started
1. Clone the repository
> https://github.com/AdnanMuhaisen/EchoChat.git

2. Setup Database
Ensure SQL Server is installed and running.

Update the appsettings.json file with your SQL Server connection string:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ChatAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
Run database migrations to create the necessary tables:

> dotnet ef database update
3. Run the application
> dotnet run

4. Access the application
Open your browser and navigate to https://localhost:5001.
Register a new user or log in with existing credentials.

Start chatting!

## Usage
- Private Messages: Send direct messages to specific users for one-on-one conversations.
- Audio and video calls: call your frinds with audio and video calls.
- User Authentication: Log in or register securely to access chat features.

## Contribution
Feel free to contribute by submitting issues or pull requests to improve the project.

License
This project is licensed under the MIT License. See the LICENSE file for details.
