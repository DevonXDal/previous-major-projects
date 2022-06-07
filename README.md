# Previous Major Projects

This repository is meant to provide access to projects that I completed in the previous classes that I have taken. 
These projects involve various languages such as Java, C#, Dart, and Rust. These projects can each be used for showcases at the college or whereever else it is desired.

## Labyrinth of Devon - Fall 2019 & Spring 2020 (CS 121/122)

This was a Java project meant to help students prove their knowledge as a skills final for the first two programming classes. As the first final, it is meant to be a
simple command line game from the World of Zuul, which is itself derived from an older game. Desiring to have this be one of the best submissions for my CS 121 class.
I added partial randomization of the layout, along with randomization of enemies and items along its three floors. It also includes one of a few boss monsters on the
final floor. The plan was to finish it as part of the second final, but burn out at the end of the semester caused it not to happen. The first semester also saw it 
feature armor and weapons as equipment, consumable items, room navigation (revamped from the starting sample code), and classes. Each class had a few stats that made it
special, but because these were never fleshed out, it ended up being more of a difficulty system as no magic or stealth was ever added. Because its theme was 
randomization, in order to provide some level of replayability, attacks did a certain amount of damage and could be dodged or blocked (same case for the enemies).
Armor also blocked a certain range of damage. Leveling up was also included as in additional feature.

In the second semester, some parts of the code structure was improved. It also saw the inclusion of graphical elements using Java AWT and Swing. Actions could be
performed by either menu buttons, hotkeys, or using the command line. It was still mostly text based in nature. Certain status information was also displayed such as
health and experience progress to the next level.

## FreeCell Solitaire - Fall 2020 (CS 220)

This is a Java implementation of FreeCell Solitaire, graphically implemented using AWT and Swing. It was built as a skills final for my data structures class. It does not
feature drag-and-drop functionality. It also was poorly written to still run off a general while loop like a command-line implmentation instead of being event driven.
It uses a custom configuration file as I was not that knowledgeable about XML, JSON, etc.

## Unit Converter - Fall 2020 (CS 221)

Unlike others on this list, this was not a final project, but my first project to learn how to make a Web application using .NET Core 2.1. It features a few simple unit
conversions in addition to some more humorous ones such as converting the power released by tons of TNT to how many phones with a battery of 4000mAh being charged.

## Weather Mobile App - Spring 2022 (CS 320)

This application has a number of bugs as it was quickly written and rewritten in two weeks. It is a Flutter (Dart) app that pulls information about the weather from
an online API and displays it to the user. Part of its unfinished nature comes from switching the state handling pattern from BLoC to ChangeProvider. This was done
as while BLoC may be a good enterprise way to handle state, it is too much boilerplate for a small, one creator app to be useful. It features accessibility features
and support for multiple languages.

## Psuedo-FS - Fall 2021 (CS 221)

This is a mock filesystem, written in Rust, that can store data saved to it as a JSON file. It includes some XML features, but they do not work passed formatting an XML
file. It supports a few very basic commands that allow the reading of files from the filesystem and copying files to and from the filesystem. It has a very basic support 
for the use of variables in the commands that it uses. It makes use of the concepts of inodes, blocks, and the super-block.

## Gin Rummy - Spring 2022 (CS 320)

The current version may include non-completed features. This description will describe the features of the finished version. This is a Web-based version of the card game
Gin Rummy, written using .NET 6 (C#) and Blazor. It features drag-and-drop capabilities, reorganizing the hand, a computer player, and more. What's more special about this
project is that it was programmed from a design journal, which was the real purpose of that class. The eventual plan is to extend this site and create a digital portfolio.
Once the digital portfolio is done, it will be on its own public GitHub repository. The majority of this application was programmed in two weeks following the design
phase. It was meant as a prototype to show that the design could be implemented.

