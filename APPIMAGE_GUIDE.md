# TodoListApp - AppImage Deployment Guide

## What is AppImage?

AppImage is a universal Linux package format that:
- ✅ Works on all Linux distributions
- ✅ Requires no installation
- ✅ Is self-contained (includes all dependencies)
- ✅ Can be distributed as a single file

## Quick Start

### Build AppImage

```bash
./build-appimage.sh
```

This will create: `StudyBuddy-1.0.0-x86_64.AppImage`

### Run AppImage

```bash
chmod +x StudyBuddy-1.0.0-x86_64.AppImage
./StudyBuddy-1.0.0-x86_64.AppImage
```

## Project Structure

```
Todo-List-AvaloniaUI/
├── TodoListApp/                    # Source code
│   ├── TodoListApp.csproj
│   └── Assets/
│       └── icons8-tasklist-48.png  # App icon
├── TodoListApp.AppDir/             # AppImage structure
│   ├── AppRun                      # Startup script
│   ├── TodoListApp.desktop         # Desktop entry
│   ├── todolistapp.png             # Icon
│   ├── .DirIcon -> todolistapp.png # Icon symlink
│   └── usr/
│       ├── bin/
│       │   └── TodoListApp         # Executable (copied during build)
│       ├── lib/
│       │   └── *.so                # Shared libraries (copied during build)
│       └── share/
│           ├── applications/
│           │   └── TodoListApp.desktop
│           └── icons/
│               └── hicolor/
│                   └── 256x256/
│                       └── apps/
│                           └── todolistapp.png
├── build-appimage.sh               # AppImage build script
└── build-linux.sh                  # Standard Linux build script
```

## Build Scripts

### 1. Standard Linux Build (`build-linux.sh`)
Creates a standalone executable in `publish/linux-x64/`

```bash
./build-linux.sh
```

**Output:** `publish/linux-x64/TodoListApp`

### 2. AppImage Build (`build-appimage.sh`)
Creates a universal AppImage package

```bash
./build-appimage.sh
```

**Output:** `StudyBuddy-1.0.0-x86_64.AppImage`

## AppImage Components

### 1. AppRun Script
Located at: `TodoListApp.AppDir/AppRun`

This script:
- Sets up library paths
- Configures data directories
- Launches the application

### 2. Desktop Entry File
Located at: `TodoListApp.AppDir/TodoListApp.desktop`

Defines:
- Application name: **StudyBuddy**
- Icon: todolistapp
- Categories: Utility, Office, Education
- Description: Your personal productivity companion

### 3. Icon
- Source: `TodoListApp/Assets/icons8-tasklist-48.png`
- Copied to multiple locations for proper integration

## Manual Build Steps

If you want to build manually:

### 1. Publish the Application

```bash
dotnet publish TodoListApp/TodoListApp.csproj \
  -c Release \
  -r linux-x64 \
  --self-contained \
  -p:PublishSingleFile=true \
  -p:PublishTrimmed=true \
  -o ./publish/linux-x64
```

### 2. Create AppDir Structure

```bash
mkdir -p TodoListApp.AppDir/usr/bin
mkdir -p TodoListApp.AppDir/usr/lib
mkdir -p TodoListApp.AppDir/usr/share/applications
mkdir -p TodoListApp.AppDir/usr/share/icons/hicolor/256x256/apps
```

### 3. Copy Files

```bash
# Copy executable
cp ./publish/linux-x64/TodoListApp TodoListApp.AppDir/usr/bin/

# Copy shared libraries
cp ./publish/linux-x64/*.so TodoListApp.AppDir/usr/lib/

# Copy icon
cp TodoListApp/Assets/icons8-tasklist-48.png TodoListApp.AppDir/todolistapp.png
cp TodoListApp/Assets/icons8-tasklist-48.png TodoListApp.AppDir/usr/share/icons/hicolor/256x256/apps/

# Create icon symlink
cd TodoListApp.AppDir
ln -sf todolistapp.png .DirIcon
cd ..

# Copy desktop file
cp TodoListApp.AppDir/TodoListApp.desktop TodoListApp.AppDir/usr/share/applications/

# Make AppRun executable
chmod +x TodoListApp.AppDir/AppRun
```

### 4. Build AppImage

```bash
# Download appimagetool if needed
wget https://github.com/AppImage/AppImageKit/releases/download/continuous/appimagetool-x86_64.AppImage
chmod +x appimagetool-x86_64.AppImage

# Build
ARCH=x86_64 ./appimagetool-x86_64.AppImage TodoListApp.AppDir StudyBuddy-1.0.0-x86_64.AppImage
```

## Distribution

### Testing

Test on different Linux distributions:
- Ubuntu/Debian
- Fedora/RHEL
- Arch Linux
- openSUSE

```bash
# Make executable
chmod +x StudyBuddy-1.0.0-x86_64.AppImage

# Run
./StudyBuddy-1.0.0-x86_64.AppImage
```

### Publishing to GitHub Releases

1. Create a release tag:
```bash
git tag v1.0.0
git push origin v1.0.0
```

2. Upload the AppImage to GitHub Releases

3. Users can download and run:
```bash
wget https://github.com/yourusername/Todo-List-AvaloniaUI/releases/download/v1.0.0/StudyBuddy-1.0.0-x86_64.AppImage
chmod +x StudyBuddy-1.0.0-x86_64.AppImage
./StudyBuddy-1.0.0-x86_64.AppImage
```

## Application Data

The app stores its data in:
```
~/.todoapp/todo.json
```

This location is properly configured in the AppRun script to work with AppImage's read-only filesystem.

## Troubleshooting

### AppImage Won't Start

1. Check if AppRun is executable:
```bash
chmod +x TodoListApp.AppDir/AppRun
```

2. Test AppRun directly:
```bash
./TodoListApp.AppDir/AppRun
```

### Missing Libraries

Ensure all `.so` files are copied:
```bash
find ./publish/linux-x64 -name "*.so"
```

Common Avalonia libraries:
- `libSkiaSharp.so`
- `libHarfBuzzSharp.so`
- `libAvaloniaNative.so`

### Icon Not Showing

1. Verify icon files exist:
```bash
ls -la TodoListApp.AppDir/todolistapp.png
ls -la TodoListApp.AppDir/.DirIcon
```

2. Check desktop file icon name matches

### Data Not Persisting

The AppRun script sets proper XDG directories. If data isn't persisting:
- Check `~/.todoapp/` directory exists
- Verify app has write permissions

## File Sizes

Expected sizes:
- Standard build: ~40-60 MB
- AppImage: ~45-65 MB

The AppImage is slightly larger due to packaging overhead but remains portable and self-contained.

## Resources

- [AppImage Documentation](https://docs.appimage.org/)
- [Avalonia Documentation](https://docs.avaloniaui.net/)
- [AppImageKit GitHub](https://github.com/AppImage/AppImageKit)

## Version Information

- **App Name:** StudyBuddy (TodoListApp)
- **Version:** 1.0.0
- **Target:** Linux x86_64
- **Framework:** .NET 9.0
- **UI Framework:** AvaloniaUI 11.3.1
