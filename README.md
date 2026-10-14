# DirectoryAccessMatrixManipulator

A Windows console app that guards a directory tree with an access matrix
(discretionary access control model).

## How it works

- The app works on the fixed directory `C:\Matrix`. Create it and put some
  test files (and subfolders) inside before running.
- On start it opens every file under `C:\Matrix` with an exclusive lock, so
  other programs can't touch them while it runs, and writes the access
  matrix to `C:\Matrix\matrix`: one row per file, one column per subject
  (`admin`, `user`, `guest`).
- Rights are derived from the file's creation time:

  | File created           | admin | user | guest |
  |------------------------|:-----:|:----:|:-----:|
  | less than 1 hour ago   | yes   | yes  | yes   |
  | 1 hour to 1 day ago    | yes   | yes  | no    |
  | more than 1 day ago    | yes   | no   | no    |

  The matrix file itself is admin-only.
- Log in by typing a user name (`admin`, `user` or `guest`, no password) and
  browse with the built-in console file explorer (arrow keys, Enter to open,
  Esc to go back). Users only see the files they have access to and can view
  their contents.
- `admin` can also change a file's creation date, which moves it to another
  access class. The matrix is recalculated at the next login.
- Esc at the top level returns to the login prompt; `0` exits and deletes the
  matrix file.

## Tech

- C#, .NET Framework 4.5.2, console application
- Windows only (hard-coded `C:\Matrix` path)

## Build and run

Open `MBKS_Galkin_02_test.sln` in Visual Studio (originally Visual Studio
2015), build and run.

## Context

University project, Omsk State University, 2017: lab 2 of the
"Security Models of Computer Systems" (MBKS) course. Labs 1 and 3:
[FileAccessManipulator](https://github.com/megarohas/FileAccessManipulator),
[DirectoryAccessMandatoryManipulator](https://github.com/megarohas/DirectoryAccessMandatoryManipulator).
