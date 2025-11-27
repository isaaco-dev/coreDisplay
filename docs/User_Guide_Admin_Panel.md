# User Guide - CoreDisplay Admin Panel

Welcome to the CoreDisplay Admin Panel. This guide will help you manage your digital signage fleet effectively.

## 1. Dashboard Overview

The Dashboard is your command center. It provides a real-time view of your entire fleet.

### Understanding Device Status
*   **🟢 Online**: The device is connected and sending heartbeats normally (last seen < 2 mins ago).
*   **🔴 Offline**: The device has stopped communicating (last seen > 5 mins ago). Check network or power.
*   **🟡 Error**: The device is reporting hardware or software errors (e.g., storage full).
*   **🔵 Maintenance**: The device is in maintenance mode and will not play content.

### Key Metrics
*   **CPU/RAM**: Monitor these to ensure your content isn't overloading the hardware. High usage (>90%) may cause stuttering playback.
*   **Storage**: Ensure devices have enough free space to download new media files.

---

## 2. Managing Content (Media & Playlists)

### Uploading Media
1.  Navigate to the **Media Library** section.
2.  Click **"Upload New"**.
3.  Select your images (.jpg, .png) or videos (.mp4).
4.  Wait for the upload to complete. The system will automatically generate a hash to ensure integrity.

### Creating a Playlist
1.  Go to **Playlists**.
2.  Click **"Create Playlist"**.
3.  Drag and drop media assets from the library into the timeline.
4.  Set the duration for images (default: 10s). Videos play for their full duration.
5.  **Save** the playlist.

---

## 3. Scheduling

Once you have a playlist, you need to tell the system *when* and *where* to play it.

1.  Go to **Schedules**.
2.  Click **"New Schedule"**.
3.  **Select Target**: Choose a specific Device or a Device Group (e.g., "Lobby Screens").
4.  **Select Playlist**: Choose the content you want to show.
5.  **Set Time**:
    *   **Always**: Plays 24/7.
    *   **Custom**: Set Start/End dates and daily hours (e.g., 09:00 - 18:00).
6.  **Publish**: The command will be sent to the devices to download the new content.

---

## 4. Troubleshooting

### Common Issues

**"Device is Offline"**
*   Check if the physical screen is powered on.
*   Verify the internet connection on the device (Wi-Fi/Ethernet).
*   If using the Windows Client, check if the "CoreDisplay Agent" service is running.

**"Content Not Updating"**
*   Check the **Storage Free** metric. If the disk is full, the device cannot download new files.
*   Verify the device is "Online". Updates are queued but only applied when the device connects.
