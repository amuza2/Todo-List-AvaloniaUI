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
    private readonly IJsonDataService? _jsonDataService;
    
    [ObservableProperty]
    private ObservableCollection<TodoItem> _tasks = new();

    [ObservableProperty]
    private ObservableCollection<TodoItem> _filteredTasks = new();

    [ObservableProperty]
    private string _newTaskTitle = string.Empty;

    [ObservableProperty]
    private string _newTaskDescription = string.Empty;

    [ObservableProperty] private DateTime _newTaskDueDate;

    public Array TaskPriorityList { get; } = Enum.GetValues<TaskPriority>();

    [ObservableProperty]
    private TaskPriority _newTaskPriority = TaskPriority.Medium;

    public Array TaskCategoriesList { get; } = Enum.GetValues<TaskCategory>();
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
    private TodoItem? _previousSelectedTask;
    
    [ObservableProperty] private string _buttonText;

    public MainWindowViewModel(IJsonDataService jsonDataService)
    {
        _jsonDataService = jsonDataService;
        _newTaskDueDate = DateTime.UtcNow.Date;
        Tasks.CollectionChanged += OnTasksCollectionChanged;
        ButtonText = "Add";
        
        // Add some sample tasks for demonstration
        AddSampleTasks();
        if (_jsonDataService != null)
        {
            _ = LoadTasksAsync();
        }
    }

    public MainWindowViewModel() : this(null!){ }
    
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
        if (sender is TodoItem task && e.PropertyName == nameof(TodoItem.IsCompleted))
        {
            if (_jsonDataService != null)
            {
                _ = _jsonDataService.UpdateAsync(task);
            }
            UpdateStats();
            UpdateFilteredTasks();
        }
    }

    partial void OnSelectedTaskChanging(TodoItem? value)
    {
        _previousSelectedTask = _selectedTask;
    }

    partial void OnSelectedTaskChanged(TodoItem? value)
    {
        if(_previousSelectedTask != null)
            _previousSelectedTask.IsSelected = false;
        
        if (value is not null)
        {
            value.IsSelected = true;
            
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
    private async Task AddTask()
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
            
            if (_jsonDataService != null)
            {
                await _jsonDataService.UpdateAsync(SelectedTask);
            }
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
            if (_jsonDataService != null)
            {
                await _jsonDataService.CreateAsync(newTask);
            }
            
            UpdateStats();
            UpdateFilteredTasks();
        }

        ClearForm();
    }

    private void ClearForm()
    {
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
    private async Task DeleteTask(TodoItem task)
    {
        Tasks.Remove(task);
        if (_jsonDataService != null)
        {
            await _jsonDataService.DeleteAsync(task.Id);
        }
        
        ClearForm();
    }

    [RelayCommand]
    private void ClearAllTasks()
    {
        Tasks.Clear();
        ClearForm();
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
            if (_jsonDataService == null) return;
            
            var items = await _jsonDataService.GetAllAsync();
            
            Tasks.Clear();
            
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