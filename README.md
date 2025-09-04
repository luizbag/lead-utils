# lead-utils

**lead-utils** is a collection of command-line utilities designed to assist team leaders with common daily tasks, such as managing daily meetings.

---

### Getting Started

#### Prerequisites
To use this tool, you must have the **.NET SDK** installed on your system.

#### Installation
You can install the tool globally using the .NET command-line interface:

```sh
dotnet tool install --global lead-utils
```

### Usage
This tool provides a set of commands for different utilities.

#### Daily Meeting Utility
To start the daily meeting utility, use the `daily` command:

```sh
utils daily
```

This command will prompt you for a team and them randomize the list of members. It will then show names one at a time, asking if the person is present and for their update. At the end of the process, it saves all the meeting notes.