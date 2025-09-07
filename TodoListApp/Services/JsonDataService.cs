using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TodoListApp.Models;

namespace TodoListApp.Services;

public class JsonDataService : IJsonDataService
{
    private readonly ILogger<JsonDataService> _logger;
    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

    public JsonDataService(ILogger<JsonDataService> logger)
    {
        _logger = logger;

        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var appDataDirectory = Path.Combine(homeDirectory, ".todoapp");

        Directory.CreateDirectory(appDataDirectory);
        _filePath = Path.Combine(appDataDirectory, "todo.json");

        _logger.LogInformation("Data file path: {FilePath}", _filePath);
    }

    public async Task<List<TodoItem>> GetAllAsync()
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            _logger.LogInformation("Loading all tasks items...");
            return await LoadItemsAsync();
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Error loading items from {FilePath}", _filePath);
            throw new Exception("Could not read Tasks From storage", e);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private async Task<List<TodoItem>> LoadItemsAsync()
    {
        if (!File.Exists(_filePath))
            return new List<TodoItem>();

        var json = await File.ReadAllTextAsync(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return new List<TodoItem>();

        var items = JsonSerializer.Deserialize<List<TodoItem>>(json, TodoJsonContext.Default.ListTodoItem);
        
        return items ?? new List<TodoItem>();
    }

    public async Task<TodoItem> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Loading task item by ID...");
            var items = await LoadItemsAsync();
            return items.FirstOrDefault(t => t.Id == id)!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error finding that item from {FilePath}", _filePath);
            throw new Exception("Could not find task");
        }
    }

    public async Task<bool> CreateAsync(TodoItem todoItem)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            _logger.LogInformation("Create task item...");
            var items = await LoadItemsAsync();

            if (todoItem.Id == 0)
            {
                var nextId = items.Count > 0 ? items.Max(t => t.Id) + 1 : 1;
                todoItem.Id = nextId;
            }

            items.Add(todoItem);
            return await SaveChangesAsync(items);
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Error creating new task");
            throw new Exception("Could not create task");
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private async Task<bool> SaveChangesAsync(List<TodoItem> items)
    {
        try
        {
            _logger.LogInformation("Save task item...");
            
            var json = JsonSerializer.Serialize(items, TodoJsonContext.Default.ListTodoItem);

            var tempFile = _filePath + ".tmp";

            await File.WriteAllTextAsync(tempFile, json);

            File.Move(tempFile, _filePath, overwrite: true);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error saving items to {FilePath}", _filePath);
            throw new Exception("Could not save items");
        }
    }

    public async Task<bool> UpdateAsync(TodoItem todoItem)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            _logger.LogInformation("Update task item...");
            var items = await LoadItemsAsync();
            var index = items.FindIndex(t => t.Id == todoItem.Id);

            if (index == -1)
                return false;

            items[index] = todoItem;
            return await SaveChangesAsync(items);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating task");
            throw new Exception("Could not update task");
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            _logger.LogInformation("Delete task item...");
            var items = await LoadItemsAsync();
            var index = items.FindIndex(t => t.Id == id);
            if (index == -1)
                return false;

            items.RemoveAt(index);
            return await SaveChangesAsync(items);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting task");
            throw new Exception("Could not delete task");
            ;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}