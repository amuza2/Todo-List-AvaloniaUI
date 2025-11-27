# TodoListApp - Flatpak Deployment Guide

## What is Flatpak?

Flatpak is a universal Linux package format that:
- ✅ Works across all major Linux distributions
- ✅ Sandboxed for security
- ✅ Automatic updates via Flathub
- ✅ Desktop integration (app menu, icons, etc.)
- ⚠️ More complex to set up than AppImage
- ⚠️ Requires Flatpak runtime on user's system

## Flatpak vs AppImage

| Feature | Flatpak | AppImage |
|---------|---------|----------|
| Installation | Requires Flatpak runtime | No installation needed |
| Sandboxing | Yes (secure) | No |
| Auto-updates | Yes (via Flathub) | No |
| File size | Smaller (shared runtime) | Larger (self-contained) |
| Setup complexity | High | Low |
| Distribution | Flathub or custom repo | Direct download |
| Desktop integration | Automatic | Manual/AppImageLauncher |

## Key Considerations for Flatpak

### 1. **Sandboxing & Permissions**

Flatpak apps run in a sandbox. You need to declare permissions:

- **File access:** Your app writes to `~/.todoapp/` - need home directory access
- **Network:** If you add online features later
- **Tray icon:** Needs special permissions
- **Notifications:** If you add them

### 2. **Runtime Dependencies**

Flatpak uses shared runtimes instead of bundling everything:
- **freedesktop runtime** - Base Linux libraries
- **.NET SDK extension** - For .NET apps

### 3. **Application ID**

Must follow reverse-DNS format:
- ✅ `io.github.amuza2.StudyBuddy`
- ❌ `StudyBuddy` or `todolistapp`

### 4. **Data Storage**

Flatpak apps have restricted filesystem access:
- App data: `~/.var/app/io.github.amuza2.StudyBuddy/data/`
- Config: `~/.var/app/io.github.amuza2.StudyBuddy/config/`

**Important:** You'll need to update the app to use Flatpak-specific paths when running in Flatpak.

### 5. **Build Process**

Flatpak builds are:
- Reproducible (same input = same output)
- Sandboxed during build
- Network access controlled
- Must declare all dependencies

## Prerequisites

### On EndeavourOS

```bash
# Install Flatpak and flatpak-builder
sudo pacman -S flatpak flatpak-builder

# Add Flathub repository
flatpak remote-add --if-not-exists flathub https://flathub.org/repo/flathub.flatpakrepo

# Install required runtimes
flatpak install flathub org.freedesktop.Platform//23.08
flatpak install flathub org.freedesktop.Sdk//23.08
flatpak install flathub org.freedesktop.Sdk.Extension.dotnet8//23.08
```

## Flatpak Manifest

Create `io.github.amuza2.StudyBuddy.yml`:

```yaml
app-id: io.github.amuza2.StudyBuddy
runtime: org.freedesktop.Platform
runtime-version: '23.08'
sdk: org.freedesktop.Sdk
sdk-extensions:
  - org.freedesktop.Sdk.Extension.dotnet8
command: studybuddy
finish-args:
  # Filesystem access
  - --filesystem=home
  # X11 and Wayland support
  - --socket=x11
  - --socket=wayland
  # Audio (if you add sound notifications)
  - --socket=pulseaudio
  # System tray
  - --talk-name=org.kde.StatusNotifierWatcher
  - --talk-name=org.freedesktop.Notifications
  # GPU acceleration
  - --device=dri
  # Persist data
  - --persist=.todoapp

modules:
  - name: studybuddy
    buildsystem: simple
    build-options:
      append-path: /usr/lib/sdk/dotnet8/bin
      append-ld-library-path: /usr/lib/sdk/dotnet8/lib
      env:
        DOTNET_CLI_TELEMETRY_OPTOUT: 'true'
    build-commands:
      # Restore and build
      - dotnet publish TodoListApp/TodoListApp.csproj -c Release -r linux-x64 --self-contained false -o /app/bin
      
      # Install executable
      - install -Dm755 /app/bin/TodoListApp /app/bin/studybuddy
      
      # Install desktop file
      - install -Dm644 io.github.amuza2.StudyBuddy.desktop /app/share/applications/io.github.amuza2.StudyBuddy.desktop
      
      # Install icon
      - install -Dm644 TodoListApp/Assets/icons8-tasklist-48.png /app/share/icons/hicolor/48x48/apps/io.github.amuza2.StudyBuddy.png
      
      # Install appdata
      - install -Dm644 io.github.amuza2.StudyBuddy.appdata.xml /app/share/metainfo/io.github.amuza2.StudyBuddy.appdata.xml
    
    sources:
      - type: dir
        path: .
```

## Required Files

### 1. Desktop File: `io.github.amuza2.StudyBuddy.desktop`

```ini
[Desktop Entry]
Name=StudyBuddy
Comment=Your personal productivity companion
Exec=studybuddy
Icon=io.github.amuza2.StudyBuddy
Type=Application
Categories=Office;Utility;Education;
Terminal=false
StartupWMClass=StudyBuddy
```

### 2. AppData/MetaInfo: `io.github.amuza2.StudyBuddy.appdata.xml`

```xml
<?xml version="1.0" encoding="UTF-8"?>
<component type="desktop-application">
  <id>io.github.amuza2.StudyBuddy</id>
  <name>StudyBuddy</name>
  <summary>Your personal productivity companion</summary>
  <metadata_license>CC0-1.0</metadata_license>
  <project_license>MIT</project_license>
  
  <description>
    <p>
      StudyBuddy is a beautiful and modern todo list application designed to help you stay organized and productive.
    </p>
    <p>Features:</p>
    <ul>
      <li>Clean and intuitive interface</li>
      <li>Task categories and priorities</li>
      <li>Due dates and reminders</li>
      <li>System tray integration</li>
      <li>Dark theme</li>
    </ul>
  </description>
  
  <screenshots>
    <screenshot type="default">
      <caption>Main window</caption>
      <image>https://raw.githubusercontent.com/amuza2/Todo-List-AvaloniaUI/main/screenshot.png</image>
    </screenshot>
  </screenshots>
  
  <url type="homepage">https://github.com/amuza2/Todo-List-AvaloniaUI</url>
  <url type="bugtracker">https://github.com/amuza2/Todo-List-AvaloniaUI/issues</url>
  
  <releases>
    <release version="1.0.0" date="2025-11-27">
      <description>
        <p>Initial release</p>
      </description>
    </release>
  </releases>
  
  <content_rating type="oars-1.1" />
</component>
```

## Code Changes Needed

### Update Data Path for Flatpak

The app needs to detect if it's running in Flatpak and adjust paths:

```csharp
// In App.axaml.cs or a helper class
private static string GetDataDirectory()
{
    // Check if running in Flatpak
    var flatpakInfo = "/app/manifest.json";
    if (File.Exists(flatpakInfo))
    {
        // Running in Flatpak - use XDG_DATA_HOME
        var xdgDataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
        if (!string.IsNullOrEmpty(xdgDataHome))
        {
            return Path.Combine(xdgDataHome, "todoapp");
        }
    }
    
    // Default: use home directory
    var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    return Path.Combine(homeDirectory, ".todoapp");
}
```

## Building the Flatpak

### Local Build

```bash
# Build
flatpak-builder --force-clean build-dir io.github.amuza2.StudyBuddy.yml

# Install locally
flatpak-builder --user --install --force-clean build-dir io.github.amuza2.StudyBuddy.yml

# Run
flatpak run io.github.amuza2.StudyBuddy
```

### Create a Bundle

```bash
# Build and export to repository
flatpak-builder --repo=repo --force-clean build-dir io.github.amuza2.StudyBuddy.yml

# Create a single-file bundle
flatpak build-bundle repo StudyBuddy.flatpak io.github.amuza2.StudyBuddy

# Users can install with:
flatpak install StudyBuddy.flatpak
```

## Publishing to Flathub

To publish on Flathub (official Flatpak repository):

1. **Fork the Flathub repository**
   ```bash
   # Create a new repository on GitHub: flathub/io.github.amuza2.StudyBuddy
   ```

2. **Submit the manifest**
   - Add your manifest file
   - Add required metadata files
   - Submit a pull request to Flathub

3. **Review process**
   - Flathub maintainers review your submission
   - May request changes
   - Once approved, auto-published

4. **Users install with:**
   ```bash
   flatpak install flathub io.github.amuza2.StudyBuddy
   ```

## Permissions Explained

```yaml
finish-args:
  # Allow access to home directory (for .todoapp folder)
  - --filesystem=home
  
  # Display server access (required for GUI)
  - --socket=x11
  - --socket=wayland
  
  # Audio (if you add notification sounds)
  - --socket=pulseaudio
  
  # System tray support
  - --talk-name=org.kde.StatusNotifierWatcher
  - --talk-name=org.freedesktop.Notifications
  
  # GPU acceleration (for smooth UI)
  - --device=dri
  
  # Persist .todoapp data outside sandbox
  - --persist=.todoapp
```

## Testing

```bash
# Build and install
flatpak-builder --user --install --force-clean build-dir io.github.amuza2.StudyBuddy.yml

# Run
flatpak run io.github.amuza2.StudyBuddy

# Check logs
flatpak run --command=sh io.github.amuza2.StudyBuddy
journalctl --user -f

# Uninstall
flatpak uninstall io.github.amuza2.StudyBuddy
```

## Pros and Cons

### ✅ Advantages

- **Security:** Sandboxed environment
- **Updates:** Automatic via Flathub
- **Integration:** Better desktop integration
- **Discovery:** Users can find it in software centers
- **Size:** Smaller (shared runtime)

### ❌ Disadvantages

- **Complexity:** More complex to set up
- **Build time:** Longer build process
- **Dependencies:** Requires Flatpak runtime
- **Permissions:** Need to carefully manage sandbox permissions
- **Debugging:** Harder to debug issues

## Recommendation

### For Your Use Case:

**Start with AppImage:**
- ✅ Simpler to create and distribute
- ✅ No runtime requirements
- ✅ Direct download and run
- ✅ Good for initial releases

**Add Flatpak Later:**
- When you have more users
- When you want Flathub distribution
- When you need automatic updates
- For better Linux ecosystem integration

### Hybrid Approach:

Offer both:
- **AppImage** - For quick downloads and testing
- **Flatpak** - For users who prefer package managers

## Next Steps

If you want to proceed with Flatpak:

1. I'll create the manifest files
2. Update the app to detect Flatpak environment
3. Create build scripts
4. Test locally
5. (Optional) Submit to Flathub

Would you like me to set this up for you?

## Resources

- [Flatpak Documentation](https://docs.flatpak.org/)
- [Flathub Submission Guide](https://github.com/flathub/flathub/wiki/App-Submission)
- [Flatpak .NET Guide](https://github.com/flatpak/flatpak-builder-tools/tree/master/dotnet)
- [Flatpak Best Practices](https://docs.flatpak.org/en/latest/conventions.html)
