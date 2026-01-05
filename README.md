# AppsWindowsPositionManager

A Windows application that automatically records and restores window positions when your screen configuration changes.

## Features

- Records each visible window position for every screen configuration
- Automatically restores window positions when screen configuration changes (e.g., docking/undocking a laptop)
- Remembers all previous screen configurations
- Available as both a Windows Service and a GUI application

## Projects

- **WinPositionLib** - Core library for managing window positions
- **WindowsPositionKeeper** - Windows Service for automatic monitoring
- **WindowsPositionManager** - GUI application for testing and manual control
- **Utils** - Shared utility classes and extensions

## How It Works

The application monitors your screen configuration every 10 seconds. When it detects a change in the number of monitors:
- **Adding monitors**: Restores windows to their previously saved positions for that configuration
- **Removing monitors**: Saves the current window layout for future restoration

## Requirements

- Windows operating system
- .NET Framework 4.7.2 or later

## References

Found useful information at: https://xcalibursystems.com/accessing-monitor-information-with-c-part-2-getting-a-monitor-associated-with-a-window-handle/