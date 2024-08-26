<div align="center">

<img src="./img/habitLoggerLogo.png" alt="calculator logo" width="200px" />
<h1>HabitLogger</h1>

</div>


FIX BELOW

Welcome to the Calculator App!

This app helps you solve quick calculations. 

You can perform addition, subtraction, multiplication, and division and much more advanced calculations!

This is the C# Console Project #2

## Features

- **Advanced Calculations**: Solve difficult calculations such as square roots. 
- **Calculation Counter**: You can see how many times you've used the calculator.
- **Calculation Repeater**: You can choose an answer from the list of your latest calculation to do more calculations.


## Getting Started

### Prerequisites

- .NET 8 SDK installed on your system.

### Installation

#### Console

1. Clone the repository:
	- `git clone https://github.com/Jinboi/calculator.git`

2. Navigate to the project directory:
	- `cd src\Calculator`

3. Run the application using the .NET CLI:
	- `dotnet run`

### Console Screenshots

This is the initial screen of the app when you start:

![calculator initial screen](./img/calculatorInitialScreen.PNG)


You will then asked to input the first number and you will be given a menu to choose a calculation option:

![calculator main menu screen](./img/calculatorMainMenu.PNG)


## Choose an option:
- **a**: For Addition
- **s**: For Subtraction
- **m**: For Multiplication
- **d**: For Division
- **p**: For Power
- **r**: For Square Roots
- **e**: For 10x
- **sin**: For Sine
- **cos**: For Cosine
- **tan**: For Tangent

Once you choose your option, you will be asked to provide your second number:

![calculator second number screen](./img/calculatorList.PNG)

Then, the calculator application will provide you the answer of the calculation based on you input such as first number, calculation option and your second number:

![calculator second number screen](./img/calculatorList.PNG)

Also, you can perform more calculations using the previous answer. 

![calculator perform more calculations using previous answers](./img/calculatorMoreList.PNG)

Keep in mind that you can press 'c' to clear the list. 

![calculator calculation from previous answer](./img/calculatorCToClearList.PNG)

Finally, you can see press 'n' to end the application:

![calculator end application](./img/calculatorNToExit.PNG)

## How It Works

- **Menu Navigation**: Follow the provided instructions to input your numbers and choose calculation option.
- **Calculation Counter**: You can see how many times the calculator was used. 
- **Answers List**: Your answers are stored in a list. These answers are not stored in a database so it will be cleared once the application is closed. Or, you could clear the list by inputting 'c'. 

## Room for Improvements

- Spectre.console could've been used for better UI.
- While there's the inputHandler and outputHandler, couldn't you have created models using getters and setters?
- Implement database so that list doesn't get erased when the application is closed.

## Contributing

- Contributions are welcome! Please fork the repository and create a pull request with your changes. 
- For major changes, please open an issue first to discuss what you would like to change.

## License

- This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

## Contact

- For any questions or feedback, please open an issue.

---
***Thank you and Happy Coding!***