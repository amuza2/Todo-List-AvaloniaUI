# TodoListApp - Windows Deployment Guide

## Cross-Compiling from Linux (EndeavourOS)

You can build Windows executables directly from your Linux system using .NET's cross-platform compilation.

## Prerequisites

- .NET 9.0 SDK installed on Linux
- No Windows system required for building!

## Building for Windows

### Quick Build

```bash
chmod +x build-windows.sh
./build-windows.sh
```

### Build All Platforms

```bash
chmod +x build-all.sh
./build-all.sh
```

This builds both Linux and Windows versions.

## Output

**Location:** `./publish/win-x64/`

**Main executable:** `TodoListApp.exe`

## Distribution

### Option 1: Distribute the Folder

Copy the entire `publish/win-x64` folder to Windows:

```bash
# Create a zip archive
cd publish
zip -r StudyBuddy-Windows-x64.zip win-x64/
```

Transfer `StudyBuddy-Windows-x64.zip` to Windows and extract it.

### Option 2: Single Executable

The build already creates a single-file executable:
- Just copy `TodoListApp.exe` from `publish/win-x64/`
- It includes all dependencies and .NET runtime

## Running on Windows

1. **No installation required** - just double-click `TodoListApp.exe`
2. **No .NET runtime needed** - it's self-contained
3. **Works on Windows 10/11** (x64)

## Application Data on Windows

The app stores its data in:
```
C:\Users\<Username>\.todoapp\todo.json
C:\Users\<Username>\.todoapp\logs\app.log
```

## File Size

Expected size: ~40-60 MB (includes .NET runtime)

## Windows Defender / SmartScreen

First-time users might see a Windows Defender SmartScreen warning because the executable is not signed.

**Users should:**
1. Click "More info"
2. Click "Run anyway"

**To avoid this (optional):**
- Sign the executable with a code signing certificate
- Build reputation by having many users download it

## Creating a Windows Installer (Optional)

If you want to create a proper installer, you can use:

### Using Inno Setup (from Windows)

1. Install Inno Setup on Windows
2. Create an installer script
3. Package the `publish/win-x64` folder

### Using WiX Toolset

```bash
# Install WiX on Linux (via Wine) or use a Windows VM
# Create MSI installer
```

## Testing on Windows

### Without a Windows Machine

Use a VM or Wine:

```bash
# Install Wine on EndeavourOS
sudo pacman -S wine

# Test the executable
wine ./publish/win-x64/TodoListApp.exe
```

**Note:** Wine testing is limited. Real Windows testing is recommended.

### With a Windows Machine

1. Copy `publish/win-x64/TodoListApp.exe` to Windows
2. Run it
3. Test all features

## Troubleshooting

### "Windows cannot access the specified device..."

This is Windows SmartScreen. Click "More info" → "Run anyway"

### Missing DLL errors

The build should be self-contained. If you see DLL errors:
- Ensure you used `--self-contained true`
- Check that `PublishSingleFile` is true

### Application won't start

Check Windows Event Viewer for errors:
1. Open Event Viewer
2. Windows Logs → Application
3. Look for errors from TodoListApp

## Manual Build Command

If you want to build manually:

```bash
dotnet publish TodoListApp/TodoListApp.csproj \
    -c Release \
    -r win-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -o ./publish/win-x64
```

## Supported Windows Versions

- ✅ Windows 10 (1607+)
- ✅ Windows 11
- ✅ Windows Server 2016+

**Architecture:** x64 only (64-bit)

## GitHub Release Example

When publishing to GitHub:

```bash
# Create archives
cd publish
zip -r StudyBuddy-1.0.0-Windows-x64.zip win-x64/
tar -czf StudyBuddy-1.0.0-Linux-x64.tar.gz linux-x64/

# Upload to GitHub Releases
# - StudyBuddy-1.0.0-Windows-x64.zip
# - StudyBuddy-1.0.0-Linux-x64.tar.gz
# - StudyBuddy-1.0.0-x86_64.AppImage
```

## Comparison: Linux vs Windows Build

| Feature | Linux | Windows |
|---------|-------|---------|
| Build from Linux | ✅ Native | ✅ Cross-compile |
| File size | ~40-60 MB | ~40-60 MB |
| Single file | ✅ | ✅ |
| Self-contained | ✅ | ✅ |
| System tray | ✅ | ✅ |
| Data location | `~/.todoapp/` | `C:\Users\<User>\.todoapp\` |

## Advanced: Code Signing (Optional)

To avoid SmartScreen warnings, sign your executable:

1. **Get a code signing certificate** (costs money)
2. **Sign the executable:**
   ```bash
   # On Windows with signtool
   signtool sign /f certificate.pfx /p password TodoListApp.exe
   ```

## Resources

- [.NET Cross-Platform Publishing](https://learn.microsoft.com/en-us/dotnet/core/deploying/)
- [Windows Runtime Identifiers](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)
- [Avalonia on Windows](https://docs.avaloniaui.net/)
