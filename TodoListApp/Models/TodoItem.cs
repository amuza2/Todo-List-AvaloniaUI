using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoListApp.Models;

public partial class TodoItem : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private bool _isCompleted;

    [ObservableProperty]
    private DateTime _dueDate = DateTime.Today;

    [ObservableProperty]
    private TaskPriority _priority = TaskPriority.Medium;

    [ObservableProperty]
    private TaskCategory _category = TaskCategory.Academic;

    [ObservableProperty]
    private DateTime _createdDate = DateTime.Now;

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string PriorityDisplay => Priority switch
    {
        TaskPriority.High => "🔥 High",
        TaskPriority.Medium => "⚡ Medium",
        TaskPriority.Low => "🌱 Low",
        _ => "⚡ Medium"
    };

    public string CategoryDisplay => Category switch
    {
        TaskCategory.Academic => "📚 Academic",
        TaskCategory.Work => "💼 Work",
        TaskCategory.Personal => "🏠 Personal",
        TaskCategory.Health => "💪 Health",
        _ => "📚 Academic"
    };

    public bool IsOverdue => !IsCompleted && DueDate < DateTime.Today;

    public bool IsDueToday => DueDate.Date == DateTime.Today;

    public bool IsDueSoon => !IsCompleted && DueDate <= DateTime.Today.AddDays(3) && DueDate >= DateTime.Today;

    partial void OnIsCompletedChanged(bool value)
    {
        // You can add completion logic here if needed
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