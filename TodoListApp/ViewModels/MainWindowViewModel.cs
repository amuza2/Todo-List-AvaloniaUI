using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoListApp.Models;
using TodoListApp.Services;

namespace TodoListApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IJsonDataService _jsonDataService;
    
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
    private TaskPriority _newTaskPriority = TaskPriority.Medium;

    [ObservableProperty] private TaskCategory _newTaskCategory = TaskCategory.Academic;

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

    public MainWindowViewModel(IJsonDataService jsonDataService)
    {
        _jsonDataService = jsonDataService;
        _newTaskDueDate = DateTime.UtcNow.Date;
        Tasks.CollectionChanged += OnTasksCollectionChanged;
        ButtonText = "Add";
        
        // Add some sample tasks for demonstration
        // _ = AddSampleTasks();
        _ = LoadTasksAsync();
    }

    public MainWindowViewModel() : this(null!){ }

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

    partial void OnSelectedTaskChanged(TodoItem? value)
    {
        if (value is not null)
        {
            NewTaskTitle = value.Title;
            NewTaskDescription = value.Description;
            NewTaskPriority = value.Priority;
            NewTaskCategory = value.Category;
            NewTaskDueDate = value.DueDate;

            ButtonText = "Update";
        }
        else
        {
            ButtonText = "Add";
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
            SelectedTask.Priority = NewTaskPriority;
            SelectedTask.Category = NewTaskCategory;
            
            _jsonDataService.UpdateAsync(SelectedTask);
            SelectedTask = null;
        }
        else
        {
            var newTask = new TodoItem
            {
                Title = NewTaskTitle.Trim(),
                Description = NewTaskDescription?.Trim() ?? string.Empty,
                DueDate = NewTaskDueDate,
                Priority = NewTaskPriority,
                Category = NewTaskCategory
            };

            Tasks.Add(newTask);
            _jsonDataService.CreateAsync(newTask);
            
            UpdateStats();
            UpdateFilteredTasks();
        }

        // Clear form
        NewTaskTitle = string.Empty;
        NewTaskDescription = string.Empty;
        NewTaskDueDate = DateTime.Today.AddDays(1);
        NewTaskPriority = TaskPriority.Medium;
        NewTaskCategory = TaskCategory.Academic;
    }

    [RelayCommand]
    private void CancelTask()
    {
        SelectedTask = null;
    }

    [RelayCommand]
    private void DeleteTask(TodoItem task)
    {
        Tasks.Remove(task);
        _jsonDataService.DeleteAsync(task.Id);
    }

    [RelayCommand]
    private void ClearAllTasks()
    {
        Tasks.Clear();
    }

    partial void OnFilterIndexChanged(int value)
    {
        UpdateFilteredTasks();
        UpdateStats();
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

    private async Task LoadTasksAsync()
    {
        try
        {
            var items = await _jsonDataService.GetAllAsync();
            
            foreach (var item in items)
            {
                Tasks.Add(item);
            }
            
            UpdateFilteredTasks();
            UpdateStats();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading tasks: {ex.Message}");
        }
    }
}