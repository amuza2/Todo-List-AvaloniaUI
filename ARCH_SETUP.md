# AppImage Setup for Arch Linux / EndeavourOS

## Required Dependencies

AppImages on Arch-based systems require FUSE2:

```bash
# Install FUSE2 (required for AppImage)
sudo pacman -S fuse2

# Optional: Install AppImageLauncher for better desktop integration
yay -S appimagelauncher
```

## Quick Fix

If you don't want to install FUSE, you can extract and run the AppImage:

```bash
# Extract the AppImage
./StudyBuddy-1.0.0-x86_64.AppImage --appimage-extract

# Run the extracted version
cd squashfs-root
./AppRun
```

## Building on Arch/EndeavourOS

The build process is the same:

```bash
./build-appimage.sh
```

## Running the AppImage

### Method 1: With FUSE (Recommended)

```bash
# Install FUSE2 first
sudo pacman -S fuse2

# Then run
chmod +x StudyBuddy-1.0.0-x86_64.AppImage
./StudyBuddy-1.0.0-x86_64.AppImage
```

### Method 2: Without FUSE (Extract)

```bash
# Extract
./StudyBuddy-1.0.0-x86_64.AppImage --appimage-extract

# Run
./squashfs-root/AppRun
```

### Method 3: Use the Helper Script

```bash
./run-appimage.sh
```

This script automatically detects if FUSE is missing and extracts the AppImage for you.

## System Dependencies

Avalonia apps on Arch need these libraries (usually already installed):

```bash
sudo pacman -S libx11 libice libsm fontconfig
```

## Desktop Integration

For better integration with your desktop environment:

```bash
# Install AppImageLauncher (AUR)
yay -S appimagelauncher

# Or use appimaged for automatic integration
yay -S appimaged
sudo systemctl enable --now appimaged
```

## Troubleshooting

### AppImage won't run

1. **Check FUSE2:**
   ```bash
   pacman -Qs fuse2
   ```

2. **Install if missing:**
   ```bash
   sudo pacman -S fuse2
   ```

3. **Or extract and run:**
   ```bash
   ./StudyBuddy-1.0.0-x86_64.AppImage --appimage-extract
   ./squashfs-root/AppRun
   ```

### Missing libraries error

```bash
# Install common dependencies
sudo pacman -S libx11 libice libsm fontconfig harfbuzz

# Check what's missing
ldd squashfs-root/usr/bin/TodoListApp | grep "not found"
```

### Permission denied

```bash
chmod +x StudyBuddy-1.0.0-x86_64.AppImage
```

## Creating a Desktop Entry (Manual)

If you want to add it to your application menu:

```bash
# Create desktop entry
cat > ~/.local/share/applications/studybuddy.desktop << 'EOF'
[Desktop Entry]
Name=StudyBuddy
Exec=/path/to/StudyBuddy-1.0.0-x86_64.AppImage
Icon=todolistapp
Type=Application
Categories=Utility;Office;Education;
Comment=Your personal productivity companion
Terminal=false
EOF

# Update desktop database
update-desktop-database ~/.local/share/applications/
```

## Notes for Arch Users

- **FUSE2 vs FUSE3:** AppImages currently require FUSE2, not FUSE3
- **AUR helpers:** Use `yay` or `paru` for AUR packages
- **Wayland:** AppImages work on both X11 and Wayland
- **File location:** Store AppImages in `~/Applications` or `~/.local/bin`

## Quick Commands

```bash
# Install FUSE2
sudo pacman -S fuse2

# Make executable
chmod +x StudyBuddy-1.0.0-x86_64.AppImage

# Run
./StudyBuddy-1.0.0-x86_64.AppImage

# Or use helper
./run-appimage.sh
```
