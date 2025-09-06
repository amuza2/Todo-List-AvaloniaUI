# StudyBuddy Student Todo App

![Avalonia](https://img.shields.io/badge/Avalonia-UI-blue)  ![MVVM](https://img.shields.io/badge/MVVM-Community_Toolkit-green)  ![MVVM](https://img.shields.io/badge/MVVM-Community_Toolkit-green)  ![Cross-Platform](https://img.shields.io/badge/Cross--Platform-orange)


A colorful, engaging, and student-friendly todo list application.

## Screenshots

<img width="1032" height="859" alt="image" src="https://github.com/user-attachments/assets/f7b4ef0a-b9a2-4858-95e8-a0d31449d3db" />


## Features

### Core Functionality
- **Add Tasks**: Create tasks with titles, descriptions, due dates, priorities, and categories
- **Task Management**: Mark tasks as complete, delete tasks, and filter tasks
- **Smart Filtering**: View all tasks, pending tasks, completed tasks, or high-priority tasks
- **Progress Tracking**: Visual progress bar and statistics showing completion status

### Student-Friendly Design
- **Colorful Interface**: Vibrant gradients and modern color scheme
- **Engaging UI**: Playful emojis and student-focused terminology
- **Priority System**: Visual priority indicators (High, Medium, Low)
- **Category Organization**: Academic, Work, Personal, and Health categories
- **Smooth Animations**: Hover effects, transitions, and micro-interactions

### Technical Features
- **MVVM Architecture**: Clean separation of concerns using CommunityToolkit.Mvvm
- **Responsive Design**: Scales beautifully across different screen sizes
- **Modern UI Components**: Custom-styled buttons, inputs, and cards
- **Real-time Updates**: Live statistics and progress tracking
- **Cross-Platform**: Runs on Windows, macOS, and Linux

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
