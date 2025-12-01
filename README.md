# Advent of Code
My personal sollutions and entries for [Advent of Code](https://adventofcode.com) written in C#. Created using the [adventofcode-template](https://github.com/encse/adventofcode).

Due to copyright reasons it is not allowed to include input files and problem descriptions
within a repository. To resolve this, the input is encrypted using `git-crypt`
which automatically encrypts and decrypts the input files on pull/push.

## Dependencies

- Based on `.NET 10` and `C# 14` 
- `AngleSharp` is used for problem download
- `git-crypt` to store the input files in an encrypted form
- the optional `Memento Inputs` extension for Visual Studio Code

## Quick-Start

1. Clone the repo
2. Install .NET
3. Place the `aoc-crypt.key` file in the root of the repo
4. Install git-crypt and unlock the repo:

```
> Set-ExecutionPolicy RemoteSigned -Scope CurrentUser # Only run this once
> irm get.scoop.sh | iex
> scoop install git-crypt
> git-crypt unlock ./aoc-crypt.key
```

5. in `bash` export your SESSION cookie from the adventofcode.com site in your terminal as an env variable:

```
> export SESSION=djsaksjakshkja...
```

6. Get help with `dotnet run` and start coding.
