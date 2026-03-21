# Hidden Cards: A Reactive Memory Game

**Hidden Cards** is a fully functional 2D memory-matching game built on a decoupled, event-driven architecture. 

### **Evolution from [UnityCardGame](https://github.com/kamal01k/UnityCardGame)**
The game's original logic was picked from the **UnityCardGame** repository. You can compare the two to see how the implementation of the **Event-Bus System** (extracted from [BoilerCore](https://github.com/kamal01k/BoilerCore)) has transformed a standard Unity project into a modular, reactive masterpiece.

## 🎮 Game Features

### **1. Dynamic Grid Generation**
The `BoardManager` calculates and generates a grid based on the current level, using **Object Pooling** for high performance.

### **2. Smooth Async Animations**
Using `async/await` for animations (`RotateY`, `HideMatched`) allows game logic to wait for visual cues without complex Coroutines.

### **3. Reactive UI System**
UI elements (`LevelText`, `MatchesText`, `TurnText`) are independent and listen for updates via the `MessageCenter` from BoilerCore.

### **4. Integrated Audio Management**
The `AudioManager` is entirely decoupled, reacting to Event-Bus messages without needing references to game logic.

## 🏗️ Technical Highlights

- **Event-Driven Architecture:** Minimal direct coupling between scripts.
- **Powered by BoilerCore:** Using its robust and type-safe **Event-Bus System**.
- **Object Pooling:** Optimized memory management for UI-based cards.
- **ScriptableObject Data:** Centralized sprite management for easy customization.

## 🛠️ How it Works (Under the Hood)
1.  **Input:** `CardInput` triggers a `CardNote.OnClick` message.
2.  **Logic:** `GameManager` hears the click, checks for a match, and sends `AudioNote.PlayFlip`.
3.  **UI/Audio:** `AudioManager` plays the flip sound, and `TurnText` updates its display simultaneously.

This project demonstrates how simple it is to build a complex, reactive game by leveraging the right architectural tools.

---
*Powered by the Event-Bus System from [BoilerCore](https://github.com/kamal01k/BoilerCore).*
