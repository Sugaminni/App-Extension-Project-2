Project 2 – Inventory, Combat, and Stats System

Overview
This project implements a player system in Unity that includes an inventory, quick slots, combat, and a stats display. The system is designed using item names (strings) instead of direct object references, allowing items to be managed and instantiated dynamically.

Features

Inventory System

Stores items using string names and quantities

Supports multiple item types such as health items, buffs, and ammo

Displays items in a UI grid

Quick Slot System

Four quick slots

Items can be assigned by selecting a slot and clicking an inventory item

Items can be used with number keys (1–4)

Slot updates when item quantity reaches zero

Combat System

Player can shoot enemies using different bullet types

Bullet types include Red, Green, and Blue bullets with different damage values

Enemies take damage and are disabled on death

Stats System

Tracks Health, Damage, Speed, and Kills

Stats are displayed on screen and toggled with the Tab key

Stats update dynamically during gameplay

Stats are saved and loaded using PlayerPrefs

Extra Credit – Item Database

Items are stored as strings (item names) instead of GameObject references

An ItemDatabase maps item names to prefabs

Items can be instantiated dynamically using their name

Demonstrated using a test key that spawns items by name

Controls

WASD – Move
Mouse – Look / Shoot
Space – Jump
1–4 – Use quick slots
Tab – Toggle stats display
P – Spawn item by name (testing feature)

Notes

All systems are connected through string-based item handling

No direct GameObject references are stored in inventory or quick slots

The system is modular and easy to extend with new item types