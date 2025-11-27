# TodoListApp - Deployment Guide

## Prerequisites

- .NET 9.0 SDK installed
- Linux system (for building)

## Deployment Options

### Linux Deployment

#### Option 1: AppImage (Recommended)
Universal package that works on all Linux distributions.

```bash
./build-appimage.sh
```

**Output:** `StudyBuddy-1.0.0-x86_64.AppImage`

See [APPIMAGE_GUIDE.md](APPIMAGE_GUIDE.md) for detailed instructions.

#### Option 2: Standalone Executable
Standard Linux executable.

```bash
./build-linux.sh
```

**Output:** `publish/linux-x64/TodoListApp`

### Windows Deployment (Cross-compiled from Linux)

Build Windows executable from your Linux system:

```bash
./build-windows.sh
```

**Output:** `publish/win-x64/TodoListApp.exe`

See [WINDOWS_DEPLOYMENT.md](WINDOWS_DEPLOYMENT.md) for detailed instructions.

### Build All Platforms

```bash
./build-all.sh
```

Builds both Linux and Windows versions.

## Manual Build

If you prefer to build manually:

```bash
dotnet publish TodoListApp/TodoListApp.csproj \
    -c Release \
    -r linux-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -o ./publish/linux-x64
```

## Running the Application

After building:

```bash
cd publish/linux-x64
chmod +x TodoListApp
./TodoListApp
```

## Distribution

The published application is a single executable file located at:
```
publish/linux-x64/TodoListApp
```

You can distribute this single file to other Linux x64 systems. No .NET runtime installation is required on the target system.

## Application Data

The application stores its data in:
```
~/.todoapp/todo.json
```

## Troubleshooting

### Missing Dependencies

If you encounter missing library errors, install the required dependencies:

**Ubuntu/Debian:**
```bash
sudo apt-get update
sudo apt-get install -y libx11-6 libice6 libsm6 libfontconfig1
```

**Fedora/RHEL:**
```bash
sudo dnf install -y libX11 libICE libSM fontconfig
```

### Permission Denied

Make sure the executable has execute permissions:
```bash
chmod +x TodoListApp
```

## Build Configurations

The application is built with:
- **Self-contained**: Includes .NET runtime
- **Single file**: All dependencies bundled into one executable
- **Trimmed**: Unused code removed to reduce size
- **AOT Compilation**: Ahead-of-time compilation for better performance

## File Size

The published executable is approximately 40-60 MB due to:
- Bundled .NET runtime
- AvaloniaUI framework
- All application dependencies
