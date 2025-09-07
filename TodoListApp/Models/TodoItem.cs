using System;
using System.Text.Json.Serialization;
using TodoListApp.ViewModels;

namespace TodoListApp.Models;

public partial class TodoItem : ViewModelBase
{
    private int _id;
    private string _title = string.Empty;
    private string _description = string.Empty;
    private bool _isCompleted;
    private DateTime _dueDate = DateTime.Today;
    private TaskPriority _priority = TaskPriority.Medium;
    private TaskCategory _category = TaskCategory.Academic;
    private DateTime _createdDate = DateTime.Now;
    private bool _isSelected;

    [JsonPropertyName("id")]
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    [JsonPropertyName("title")]
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    [JsonPropertyName("description")]
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    [JsonPropertyName("isCompleted")]
    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            if (SetProperty(ref _isCompleted, value))
            {
                OnPropertyChanged(nameof(IsOverdue));
                OnPropertyChanged(nameof(IsDueSoon));
            }
        }
    }

    [JsonPropertyName("dueDate")]
    public DateTime DueDate
    {
        get => _dueDate;
        set
        {
            if (SetProperty(ref _dueDate, value))
            {
                OnPropertyChanged(nameof(IsOverdue));
                OnPropertyChanged(nameof(IsDueToday));
                OnPropertyChanged(nameof(IsDueSoon));
            }
        }
    }

    [JsonPropertyName("priority")]
    public TaskPriority Priority
    {
        get => _priority;
        set => SetProperty(ref _priority, value);
    }

    [JsonPropertyName("category")]
    public TaskCategory Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate
    {
        get => _createdDate;
        set => SetProperty(ref _createdDate, value);
    }

    [JsonIgnore]
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    [JsonIgnore]
    public bool IsOverdue => !IsCompleted && DueDate < DateTime.Today;
    
    [JsonIgnore]
    public bool IsDueToday => DueDate.Date == DateTime.Today;
    
    [JsonIgnore]
    public bool IsDueSoon => !IsCompleted && DueDate <= DateTime.Today.AddDays(3) && DueDate >= DateTime.Today;
}