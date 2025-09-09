# StudyBuddy Student Todo App

![Avalonia](https://img.shields.io/badge/Avalonia-UI-blue)  ![MVVM](https://img.shields.io/badge/MVVM-Community_Toolkit-green)  ![Cross-Platform](https://img.shields.io/badge/Cross--Platform-orange) ![Linux](https://img.shields.io/badge/Linux-yellow) ![Windows](https://img.shields.io/badge/Windows-blue)


A colorful, engaging, and student-friendly todo list application.

## Screenshots

<img width="904" height="737" alt="image" src="https://github.com/user-attachments/assets/ca8417d5-7135-4462-9b92-86e7966f60b3" />

<img width="215" height="166" alt="image" src="https://github.com/user-attachments/assets/d667b596-a40f-4b95-a151-dbce800fc582" />


## Getting Started

### Prerequisites
- .NET 9.0 SDK or later

### Installation

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd StudentTodoApp
   ```

2. **Restore packages:**
   ```bash
   dotnet restore
   ```

3. **Build the project:**
   ```bash
   dotnet build
   ```

4. **Run the application:**
   ```bash
   dotnet run
   ```

## Architecture Details

### MVVM Implementation
- **Model**: `TodoItem` - Observable model with property change notifications
- **ViewModel**: `MainWindowViewModel` - Business logic, commands, and data management
- **View**: `MainWindow.axaml` - UI definition with data binding


## Contributing 

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

If you encounter any issues or have questions:
1. Check the existing issues on GitHub
2. Create a new issue with detailed information
3. Include your OS, .NET version, and error messages

## Acknowledgments

- Avalonia UI team for the excellent cross-platform framework
- Microsoft for the Community Toolkit
- The open-source community for inspiration and resources


**Happy Working or Studying!**
