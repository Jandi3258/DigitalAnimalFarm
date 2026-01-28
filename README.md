# Digital Animal Farm - Blazor Edition

Digital implementation of the Animal farm. This project is a web-based application allowing users to play against an automated bot opponent while adhering to the original mathematical rules of animal husbandry.

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)
* [Game Logic](#game-logic)

## General info
The goal of the project was to create a functional, turn-based version of the Super Farmer game using modern web technologies. The application showcases the implementation of complex game logic, resource management, and a strategic bot (FarmerBot) that mimics human decision-making processes in trading and risk management.

## Technologies
Project is created with:
* .NET 9.0
* Blazor InteractiveServer
* C# 12
* Bootstrap 5.0
* CSS3

## Setup
To run this project locally, you need to have the .NET SDK installed.

1. Clone the repository:
```bash
git clone https://github.com/Jandi3258/DigitalAnimalFarm.git
```

2. Navigate to the project directory:
```
cd DigitalAnimalFarm
```

4. Restore dependencies and run the application:
```
dotnet run
```

6. Open your browser and go to http://localhost:5000 (or the port specified in the console).

## Game Logic
The core engine utilizes functional programming patterns in C#. Key highlights include:

Delegate Mapping: Use of lambda-based dictionaries to manage animal inventories efficiently, avoiding complex conditional statements.

Asynchronous Turn Pattern: Implementation of Task-based delays to provide visual feedback during the AI's turn.

Accurate Reproduction Model: Growth is calculated using the floor of the sum of owned and rolled animals divided by two.

