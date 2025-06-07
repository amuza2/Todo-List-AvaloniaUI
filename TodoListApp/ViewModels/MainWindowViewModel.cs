using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoListApp.Models;

namespace TodoListApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string _newTodoTitle = string.Empty;

    [ObservableProperty] private string _newTodoDescription = string.Empty;

    [ObservableProperty] private TodoItem? _selectedTodoItem;

    public ObservableCollection<TodoItem> TodoItems { get; } = new();



    [RelayCommand]
    private void AddTodo()
    {
        if (string.IsNullOrWhiteSpace(NewTodoTitle))
            return;

        var newTodo = new TodoItem(NewTodoTitle, NewTodoDescription);
        TodoItems.Add(newTodo);

        // Clear the input fields
        NewTodoTitle = string.Empty;
        NewTodoDescription = string.Empty;
    }

    [RelayCommand]
    private void DeleteTodo(TodoItem? todoItem)
    {
        if (todoItem != null && TodoItems.Contains(todoItem))
        {
            TodoItems.Remove(todoItem);

            // Clear selection if the deleted item was selected
            if (SelectedTodoItem == todoItem)
                SelectedTodoItem = null;
        }
    }

    [RelayCommand]
    private void ToggleComplete(TodoItem? todoItem)
    {
        if (todoItem != null)
        {
            todoItem.IsCompleted = !todoItem.IsCompleted;
        }
    }

    [RelayCommand]
    private void ClearCompleted()
    {
        var completedItems = TodoItems.Where(x => x.IsCompleted).ToList();
        foreach (var item in completedItems)
        {
            TodoItems.Remove(item);
        }
    }

    // Computed properties for statistics
    public int TotalItems => TodoItems.Count;
    public int CompletedItems => TodoItems.Count(x => x.IsCompleted);
    public int PendingItems => TodoItems.Count(x => !x.IsCompleted);

    public MainWindowViewModel()
    {
        // Subscribe to collection changes to update statistics
        TodoItems.CollectionChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(TotalItems));
            OnPropertyChanged(nameof(CompletedItems));
            OnPropertyChanged(nameof(PendingItems));

            // Also subscribe to item property changes for completion status
            if (e.NewItems != null)
            {
                foreach (TodoItem item in e.NewItems)
                {
                    SubscribeToItemPropertyChanges(item);
                }
            }
        };

        // Add sample data
        TodoItems.Add(new TodoItem("Learn Avalonia UI", "Understand the basics of Avalonia UI framework"));
        TodoItems.Add(new TodoItem("Practice MVVM", "Get comfortable with MVVM pattern"));
        TodoItems.Add(new TodoItem("Build Todo App", "Create a functional todo application"));
    }
    
    private void SubscribeToItemPropertyChanges(TodoItem item)
    {
        item.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(TodoItem.IsCompleted))
            {
                OnPropertyChanged(nameof(CompletedItems));
                OnPropertyChanged(nameof(PendingItems));
            }
        };
    }
}