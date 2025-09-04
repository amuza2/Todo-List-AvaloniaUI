using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoListApp.Models;

namespace TodoListApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<TodoItem> _tasks = new();

    [ObservableProperty]
    private ObservableCollection<TodoItem> _filteredTasks = new();

    [ObservableProperty]
    private string _newTaskTitle = string.Empty;

    [ObservableProperty]
    private string _newTaskDescription = string.Empty;

    [ObservableProperty] private DateTime _newTaskDueDate;

    [ObservableProperty]
    private int _newTaskPriority = 1; // Medium

    [ObservableProperty]
    private int _newTaskCategory = 0; // Academic

    [ObservableProperty]
    private int _filterIndex = 0; // All tasks

    [ObservableProperty]
    private int _completedTasksCount;

    [ObservableProperty]
    private int _totalTasksCount;

    [ObservableProperty]
    private int _pendingTasksCount;

    [ObservableProperty]
    private double _progressPercentage;
    
    [ObservableProperty]
    private TodoItem? _selectedTask;
    
    [ObservableProperty] private string _buttonText;

    public MainWindowViewModel()
    {
        _newTaskDueDate = DateTime.UtcNow.Date;;
        Tasks.CollectionChanged += OnTasksCollectionChanged;
        UpdateFilteredTasks();
        UpdateStats();
        ButtonText = "Add";
        
        // Add some sample tasks for demonstration
        AddSampleTasks();
    }

    partial void OnSelectedTaskChanged(TodoItem? value)
    {
        if (value is not null)
        {
            NewTaskTitle = value.Title;
            NewTaskDescription = value.Description;
            NewTaskPriority = (int)value.Priority;
            NewTaskCategory = (int)value.Category;
            NewTaskDueDate = value.DueDate;

            ButtonText = "Update";
        }
        else
        {
            ButtonText = "Add";
        }
        
    }

    private void AddSampleTasks()
    {
        Tasks.Add(new TodoItem
        {
            Title = "Complete Math Assignment",
            Description = "Finish algebra homework problems 1-20",
            DueDate = DateTime.Today.AddDays(2),
            Priority = TaskPriority.High,
            Category = TaskCategory.Academic
        });

        Tasks.Add(new TodoItem
        {
            Title = "Study for History Exam",
            Description = "Review chapters 5-8, focus on key dates and events",
            DueDate = DateTime.Today.AddDays(5),
            Priority = TaskPriority.High,
            Category = TaskCategory.Academic
        });

        Tasks.Add(new TodoItem
        {
            Title = "Morning Workout",
            Description = "30 minutes cardio + strength training",
            DueDate = DateTime.Today,
            Priority = TaskPriority.Medium,
            Category = TaskCategory.Health
        });

        Tasks.Add(new TodoItem
        {
            Title = "Call Mom",
            Description = "Weekly check-in call",
            DueDate = DateTime.Today.AddDays(1),
            Priority = TaskPriority.Low,
            Category = TaskCategory.Personal,
            IsCompleted = true
        });

        Tasks.Add(new TodoItem
        {
            Title = "Update Resume",
            Description = "Add recent project experience and skills",
            DueDate = DateTime.Today.AddDays(7),
            Priority = TaskPriority.Medium,
            Category = TaskCategory.Work
        });
    }

    private void OnTasksCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (TodoItem item in e.OldItems)
            {
                item.PropertyChanged -= OnTaskPropertyChanged;
            }
        }

        if (e.NewItems != null)
        {
            foreach (TodoItem item in e.NewItems)
            {
                item.PropertyChanged += OnTaskPropertyChanged;
            }
        }

        UpdateFilteredTasks();
        UpdateStats();
    }

    private void OnTaskPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TodoItem.IsCompleted))
        {
            UpdateStats();
            UpdateFilteredTasks();
        }
    }

    [RelayCommand]
    private void AddTask()
    {
        if (string.IsNullOrWhiteSpace(NewTaskTitle))
            return;
        
        // update task
        if (SelectedTask != null)
        {
            SelectedTask.Title = NewTaskTitle.Trim();
            SelectedTask.Description = NewTaskDescription.Trim() ?? string.Empty;
            SelectedTask.DueDate = NewTaskDueDate;
            SelectedTask.Priority = (TaskPriority)NewTaskPriority;
            SelectedTask.Category = (TaskCategory)NewTaskCategory;
        }
        else
        {
            var newTask = new TodoItem
            {
                Title = NewTaskTitle.Trim(),
                Description = NewTaskDescription?.Trim() ?? string.Empty,
                DueDate = NewTaskDueDate,
                Priority = (TaskPriority)NewTaskPriority,
                Category = (TaskCategory)NewTaskCategory
            };

            Tasks.Add(newTask);
        }
        

        // Clear form
        NewTaskTitle = string.Empty;
        NewTaskDescription = string.Empty;
        NewTaskDueDate = DateTime.Today.AddDays(1);
        NewTaskPriority = 1;
        NewTaskCategory = 0;
    }

    [RelayCommand]
    private void DeleteTask(TodoItem task)
    {
        Tasks.Remove(task);
    }

    [RelayCommand]
    private void ClearAllTasks()
    {
        Tasks.Clear();
    }

    partial void OnFilterIndexChanged(int value)
    {
        UpdateFilteredTasks();
    }

    private void UpdateFilteredTasks()
    {
        var filtered = FilterIndex switch
        {
            0 => Tasks.OrderBy(t => t.IsCompleted).ThenByDescending(t => (int)t.Priority).ThenBy(t => t.DueDate),
            1 => Tasks.Where(t => !t.IsCompleted).OrderByDescending(t => (int)t.Priority).ThenBy(t => t.DueDate),
            2 => Tasks.Where(t => t.IsCompleted).OrderByDescending(t => t.CreatedDate),
            3 => Tasks.Where(t => t.Priority == TaskPriority.High).OrderBy(t => t.IsCompleted).ThenBy(t => t.DueDate),
            _ => Tasks.OrderBy(t => t.IsCompleted).ThenByDescending(t => (int)t.Priority).ThenBy(t => t.DueDate)
        };

        FilteredTasks.Clear();
        foreach (var task in filtered)
        {
            FilteredTasks.Add(task);
        }
    }

    private void UpdateStats()
    {
        TotalTasksCount = Tasks.Count;
        CompletedTasksCount = Tasks.Count(t => t.IsCompleted);
        PendingTasksCount = TotalTasksCount - CompletedTasksCount;
        ProgressPercentage = TotalTasksCount > 0 ? (double)CompletedTasksCount / TotalTasksCount * 100 : 0;
    }
}