using System;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using TodoListApp.ViewModels;

namespace TodoListApp.Models;

public partial class TodoItem : ViewModelBase
{
    [ObservableProperty] [JsonPropertyName("id")] 
    private int _id;
    
    [ObservableProperty] [JsonPropertyName("title")]
    private string _title = string.Empty;

    [ObservableProperty] [JsonPropertyName("description")]
    private string _description = string.Empty;

    [ObservableProperty] [JsonPropertyName("isCompleted")]
    private bool _isCompleted;

    [ObservableProperty] [JsonPropertyName("dueDate")]
    private DateTime _dueDate = DateTime.Today;

    [ObservableProperty] [JsonPropertyName("priority")]
    private TaskPriority _priority = TaskPriority.Medium;

    [ObservableProperty] [JsonPropertyName("category")]
    private TaskCategory _category = TaskCategory.Academic;

    [ObservableProperty] [JsonPropertyName("createdDate")]
    private DateTime _createdDate = DateTime.Now;
    
    public bool IsOverdue => !IsCompleted && DueDate < DateTime.Today;

    public bool IsDueToday => DueDate.Date == DateTime.Today;

    public bool IsDueSoon => !IsCompleted && DueDate <= DateTime.Today.AddDays(3) && DueDate >= DateTime.Today;

    partial void OnIsCompletedChanged(bool value)
    {
        OnPropertyChanged(nameof(IsOverdue));
        OnPropertyChanged(nameof(IsDueSoon));
    }

    partial void OnDueDateChanged(DateTime value)
    {
        OnPropertyChanged(nameof(IsOverdue));
        OnPropertyChanged(nameof(IsDueToday));
        OnPropertyChanged(nameof(IsDueSoon));
    }
}