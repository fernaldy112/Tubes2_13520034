<div id="top"></div>
<br />
<div align="center">
  <a href="https://github.com/fernaldy112/Tubes2_13520034">
    <img src="logo.png" alt="Logo" width="200" height="200">
  </a>

<h3 align="center">Folder Crawler</h3>

  <p align="center">
    A visualization application of file searching using depth-first search
    or breadth-first search algorithm.
    <br />
    <a href="">View Demo</a>
    ·
    <a href="">Documentation</a>
  </p>
</div>

#### Table of Contents
- [About The Project](#about-the-project)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Contributors](#contributors)

## About The Project

Folder Crawler is an application to visualize file searchig using
depth-first search or breadth-first search algorithm through graph
visualization. User can provide a starting directory, file name, and
searching mode (algorithm) to begin searching. The application will
start to search and show graph visualization of directories and files
it traverse in real time. This project is made to fulfill
`Tugas Besar 2 IF2211 Strategi Algoritma`.

## Getting Started

### Prerequisites

To build the application from scratch, you will need
[Visual Studio](https://visualstudio.microsoft.com/downloads/)
with .NET desktop development workload as well as
[.NET SDK and Runtime](https://dotnet.microsoft.com/en-us/download).

You will also need to install all the dependencies used by
the application:
- AutomaticGraphLayout.WpfGraphControl
- WindowsAPICodePack

### Installation

To build the application, you will need to do the following steps.

1. Open `Tubes2_13520034.sln` in Visual Studio.
2. Choose `Build`, then press `Build Solution`.
3. Wait until the build process is finished.
4. The binary executable will be available in `bin/release` directory.

## Usage

To visualize file searching with graph visualization, you will need to
choose the searching process algorithm and starting directory.
You will also need to provide the file name you want to search.
The application lets you to choose whether the seaching should be _exhaustive_
, i.e. traversing every existing files, or not.
If all has been set up, you can click the `SEARCH` and the application will
start to do its job.

## Contributors
This project is made by:
- [Bryan Bernigen](https://github.com/bryanbernigen)(13520034)
- [Fernaldy](https://github.com/fernaldy112) (13520112)
- [Raden Rifqi Rahman](https://github.com/Radenz/) (13520166)