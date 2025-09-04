# StudyBuddy - Student Todo App 🎓

A colorful, engaging, and student-friendly todo list application built with Avalonia UI using the MVVM pattern and CommunityToolkit.Mvvm.

## Features ✨

### Core Functionality
- **Add Tasks**: Create tasks with titles, descriptions, due dates, priorities, and categories
- **Task Management**: Mark tasks as complete, delete tasks, and filter tasks
- **Smart Filtering**: View all tasks, pending tasks, completed tasks, or high-priority tasks
- **Progress Tracking**: Visual progress bar and statistics showing completion status

### Student-Friendly Design
- **Colorful Interface**: Vibrant gradients and modern color scheme
- **Engaging UI**: Playful emojis and student-focused terminology
- **Priority System**: Visual priority indicators (🔥 High, ⚡ Medium, 🌱 Low)
- **Category Organization**: Academic, Work, Personal, and Health categories
- **Smooth Animations**: Hover effects, transitions, and micro-interactions

### Technical Features
- **MVVM Architecture**: Clean separation of concerns using CommunityToolkit.Mvvm
- **Responsive Design**: Scales beautifully across different screen sizes
- **Modern UI Components**: Custom-styled buttons, inputs, and cards
- **Real-time Updates**: Live statistics and progress tracking
- **Cross-Platform**: Runs on Windows, macOS, and Linux

## Project Structure 📁

```
StudentTodoApp/
├── Models/
│   └── TodoItem.cs              # Task model with observable properties
├── ViewModels/
│   └── MainWindowViewModel.cs   # Main view model with commands and data
├── Views/
│   ├── MainWindow.axaml         # Main window UI definition
│   └── MainWindow.axaml.cs      # Code-behind for main window
├── Converters/
│   └── Converters.cs            # Value converters for UI bindings
├── App.axaml                    # Application resources and styles
├── App.axaml.cs                 # Application entry point
└── Program.cs                   # Main program entry point
```

## Getting Started 🚀

### Prerequisites
- .NET 8.0 SDK or later
- JetBrains Rider IDE (recommended for Pop!_OS)
- Git

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

### Development in Rider

1. Open Rider IDE
2. Choose "Open" and select the `StudentTodoApp` folder
3. Wait for the project to load and restore packages
4. Press F5 or click the Run button to start debugging

## Usage Guide 📖

### Adding Tasks
1. Fill in the task title (required)
2. Optionally add a description
3. Set the priority level (High, Medium, Low)
4. Choose a category (Academic, Work, Personal, Health)
5. Select a due date
6. Click "Add Task ➕"

### Managing Tasks
- **Complete Tasks**: Check the checkbox next to any task
- **Delete Tasks**: Click the "Delete" button on individual tasks
- **Filter Tasks**: Use the dropdown to filter by status or priority
- **Clear All**: Remove all tasks at once

### Visual Indicators
- **Priority Colors**: High (red), Medium (yellow), Low (green)
- **Category Colors**: Each category has its own color scheme
- **Progress Bar**: Shows completion percentage
- **Statistics**: Live counts of completed and pending tasks

## Customization 🎨

### Adding New Categories
1. Update the `TaskCategory` enum in `Models/TodoItem.cs`
2. Add the new category to the ComboBox in `MainWindow.axaml`
3. Update the `CategoryToClassConverter` for styling
4. Add corresponding CSS classes for colors

### Styling Changes
- Modify colors in the `Window.Styles` section of `MainWindow.axaml`
- Update gradients and backgrounds in the Grid.Background definitions
- Customize animations by adjusting `Transitions` properties

### Adding Features
- Extend the `TodoItem` model for new properties
- Add corresponding UI elements in `MainWindow.axaml`
- Implement new commands in `MainWindowViewModel.cs`

## Architecture Details 🏗️

### MVVM Implementation
- **Model**: `TodoItem` - Observable model with property change notifications
- **ViewModel**: `MainWindowViewModel` - Business logic, commands, and data management
- **View**: `MainWindow.axaml` - UI definition with data binding

### Key Patterns Used
- **ObservableObject**: Base class for property change notifications
- **ObservableProperty**: Source generators for automatic property implementation
- **RelayCommand**: Command pattern implementation for UI actions
- **Value Converters**: Transform data between model and view representations

### Data Binding
- Two-way binding for form inputs
- One-way binding for display data
- Command binding for user actions
- Collection binding for task lists

## Contributing 🤝

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Dependencies 📦

- **Avalonia UI**: Cross-platform .NET UI framework
- **CommunityToolkit.Mvvm**: MVVM helpers and source generators
- **Avalonia.Themes.Fluent**: Modern Fluent Design theme
- **Avalonia.Fonts.Inter**: Beautiful Inter font family

## License 📄

This project is licensed under the MIT License - see the LICENSE file for details.

## Roadmap 🗺️

### Planned Features
- [ ] Task notifications and reminders
- [ ] Data persistence (JSON/SQLite)
- [ ] Task templates for common student activities
- [ ] Time tracking and Pomodoro timer integration
- [ ] Export tasks to calendar applications
- [ ] Dark mode support
- [ ] Keyboard shortcuts
- [ ] Task search functionality
- [ ] Drag and drop task reordering
- [ ] Study session planning

### Performance Improvements
- [ ] Virtual scrolling for large task lists
- [ ] Lazy loading of task data
- [ ] Memory optimization for long-running sessions

## Screenshots 📸

*Note: Add screenshots of your application here once it's running*

## Support 💬

If you encounter any issues or have questions:
1. Check the existing issues on GitHub
2. Create a new issue with detailed information
3. Include your OS, .NET version, and error messages

## Acknowledgments 🙏

- Avalonia UI team for the excellent cross-platform framework
- Microsoft for the Community Toolkit
- The open-source community for inspiration and resources

---

**Happy Studying! 🎓✨**