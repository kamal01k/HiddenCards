# Unity Hidden Cards - Core Architecture

This project serves as a demonstration of a high-performance, decoupled **Event-Bus System** (`MessageCenter`) in Unity. The architecture is designed for scalability, making it incredibly simple to add new features without modifying existing logic.

## 🚀 Core Engine: [BoilerCore](https://github.com/kamal01k/BoilerCore)
The foundational messaging system is powered by **BoilerCore**. In this project, we are specifically utilizing its **Event-Bus System** to act as the "nervous system" of the game, proving how a robust core can simplify complex interactions.

### **Key Advantages**
- **Complete Decoupling:** Components (UI, Audio, Game Logic) never talk to each other directly.
- **Type Safety:** Using `MsgID<T>` ensures messages carry correct data, eliminating casting errors.
- **Async Ready:** Built-in support for `async/await` patterns for complex sequences.
- **Scalability:** Adding new features is as simple as adding a new Listener.

## 🎮 Game Source: [UnityCardGame](https://github.com/kamal01k/UnityCardGame)
The gameplay and logic were originally developed in the **UnityCardGame** repository. By comparing this project with the original, you can clearly see the architectural evolution:
- **Before:** Tight coupling and complex references.
- **After:** A clean, reactive, and message-driven architecture using the Event-Bus.

---

## 🏗️ Project Structure
- **Core/**: The foundational Event-Bus system extracted from BoilerCore.
- **HiddenCards/**: The evolved memory game implementation.

---
*This project demonstrates the power of modular design by combining BoilerCore's architecture with UnityCardGame's logic.*
