Municipal Services Application (Part 2 extension)

Overview

This Windows Forms application provides municipal services features including reporting issues (Part 1) and Local Events & Announcements (Part 2). The Part 2 extension adds event display, searching, data-structure demonstrations and a simple recommendation system.

Prerequisites

- Visual Studio 2022 or later
- .NET 8 SDK

How to build and run

1. Open the solution folder in Visual Studio.
2. Build the solution (Build -> Build Solution).
3. Run the application. The existing entry point `Program.Main()` launches the main menu (`MainMenuForm`).

Main Menu

- `Report Issues` opens the existing `ReportIssuesForm` (unchanged behavior).
- `Local Events and Announcements` opens the new `LocalEventsForm`.
- `Service Request Status` remains disabled for Task 3.

Local Events and Announcements

- Browse events in the main grid. Double-click a row to view details.
- Search by keywords, select a category, and optionally use From and To date pickers.
- Click `Search` to filter results or `Clear Filters` to restore the full list.
- `Previous Search` demonstrates a search history stack (LIFO). Use it to reapply earlier searches.
- Select an event and click `Enqueue` to add it to a FIFO queue. Click `Dequeue` to process the next item.
- Click `Process Priority` to process the highest-priority item from a simple priority queue (High &gt; Medium &gt; Low).
- Recommendations are shown in the `Recommended for You` list and update when you search or view events.

Data structures used

- Stack: `SearchHistoryService` uses a `Stack<SearchFilter>` to allow previous searches to be revisited (LIFO).
- Queue: `EventRepository` and the form maintain a `Queue<MunicipalEvent>` for upcoming events (FIFO enqueue/dequeue demonstrated in UI).
- Priority queue: `SimplePriorityQueue<T>` is a small custom implementation using `SortedDictionary<int, Queue<T>>` to ensure higher-priority items are processed first.
- Hashtable: `EventRepository` maintains a `Hashtable` mapping event IDs to events and a snapshot is shown in the UI.
- Dictionary: `EventRepository` maintains a `Dictionary<string, MunicipalEvent>` for fast lookups by ID.
- SortedDictionary: events are indexed in a `SortedDictionary<DateTime, List<MunicipalEvent>>` to support date-ordered lookups.
- Sets: `HashSet<string>` is used to track unique categories for the category filter without duplicates.

Recommendation algorithm

- The `SearchHistoryService` tracks recent search categories and keywords (session memory only).
- `RecommendationService` computes a score per event based on:
  - Category preference (weight 0.4)
  - Keyword relevance (weight 0.3)
  - Search-interest frequency (weight 0.2)
  - Date relevance for upcoming events (weight 0.1)
- Scores are combined to rank events; expired events are excluded. If insufficient history exists, fallback recommendations are shown (upcoming/high-priority events).

Running tests / verification

- Build and run the solution in Visual Studio.
- From the main menu, open `Report Issues` and verify submission still stores reports in memory.
- Open `Local Events and Announcements`, try searches, previous search, enqueue/dequeue, process priority and observe recommendations update.

Limitations

- All data is stored in memory for the session only; there is no persistent database.
- Recommendation data is session-scoped and not persisted between runs.

Files added or modified

- Modified: `MainMenuForm.Designer.cs`, `MainMenuForm.cs`
- Added: `MunicipalEvent.cs`, `SimplePriorityQueue.cs`, `EventRepository.cs`, `SearchHistoryService.cs`, `RecommendationService.cs`, `LocalEventsForm.cs`, `LocalEventsForm.Designer.cs`, `README.md`

